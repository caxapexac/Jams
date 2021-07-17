const Constants = require('../shared/constants')
const Player = require('./objects/player')
const utilsArray = require('../shared/utils_array')
const utilsString = require('../shared/utils_string')
const botSystem = require('./systems/bot_system')
const boundsSystem = require('./systems/bounds_system')
const chargeSystem = require('./systems/charge_system')
const collisionSystem = require('./systems/collision_system')
const deathSystem = require('./systems/death_system')
const jointSystem = require('./systems/joint_system')
const motionSystem = require('./systems/motion_system')
const networkUpdateSystem = require('./systems/network_update_system')
const scoreByTimeSystem = require('./systems/score_by_time_system')
const starSpawnSystem = require('./systems/star_spawn_system')
const spikeSpawnSystem = require('./systems/spike_spawn_system')

class Game {
    constructor (io) {
        // Networking
        this.io = io
        this.sockets = {}
        // Entities
        this.players = {}
        this.spikes = []
        this.bondages = []
        this.stars = []
        this.counter = 0 // For bots
        this.leaderboard = []
        this.leadersCount = 0
        // Game loop
        this.lastUpdateTime = Date.now()
        this.shouldSendUpdate = false
        setInterval(() => this.update(), 1000 / 60)
    }

    addSpectator (socket, username) {
        const safeUsername = utilsString.validateNickname(username)
        if (socket === null) {
            socket = { id: safeUsername, emit: () => {} }
        }
        this.sockets[socket.id] = socket
        const x = Constants.MAP_SIZE * (0.25 + Math.random() * 0.5)
        const y = Constants.MAP_SIZE * (0.25 + Math.random() * 0.5)
        const player = new Player(socket.id, x, y, safeUsername)
        player.spectator = true
        console.log(`Added spectator ${username} ${socket.id}`)
        this.players[socket.id] = player
    }

    addPlayer (socket, username) {
        const safeUsername = utilsString.validateNickname(username)
        if (socket === null) {
            socket = { id: safeUsername, emit: () => {} }
        }
        this.sockets[socket.id] = socket
        let x = 0
        let y = 0
        if (this.bondages.length === 0) {
            x = Constants.MAP_SIZE * (0.25 + Math.random() * 0.5)
            y = Constants.MAP_SIZE * (0.25 + Math.random() * 0.5)
            this.bondages.push([socket.id, socket.id])
        } else {
            const bondage = utilsArray.pick(this.bondages)
            x = (this.players[bondage[0]].x + this.players[bondage[1]].x) * 0.5
            y = (this.players[bondage[0]].y + this.players[bondage[1]].y) * 0.5
            this.bondages.push([bondage[0], socket.id])
            this.bondages.push([bondage[1], socket.id])
        }
        const player = new Player(socket.id, x, y, safeUsername)
        console.log(`Added ${username} ${socket.id} at ${x};${y}`)
        this.players[socket.id] = player
    }

    removePlayer (socket) {
        const player = this.players[socket.id]
        if (player === undefined) return
        const toReplace = []
        this.bondages = this.bondages.filter((value, index, arr) => {
            if (value[0] === player.id && value[1] === player.id) {
                return false
            } else if (value[0] === player.id) {
                toReplace.push(value[1])
                return false
            } else if (value[1] === player.id) {
                toReplace.push(value[0])
                return false
            } else {
                return true
            }
        })
        if (toReplace.length === 2) {
            this.bondages.push([toReplace[0], toReplace[1]])
        } else if (toReplace.length !== 0) {
            console.error(`toReplace is ${toReplace.length} length`)
        }
        if (socket.id.startsWith('bot') === false) {
            this.leadersCount++
            this.leaderboard.push({ username: player.username || 'Looper', score: player.score })
            this.leaderboard = this.leaderboard.sort((p1, p2) => p2.score - p1.score)
            this.leaderboard = this.leaderboard.slice(0, 10)
            this.io.emit(Constants.MSG_OUT.LEADERBOARD_UPDATE, { count: this.leadersCount, data: this.leaderboard })
        }
        console.log(`Removed ${player.username} ${socket.id}`)
        delete this.sockets[socket.id]
        delete this.players[socket.id]
    }

    changeDirection (socket, dir) {
        const player = this.players[socket.id]
        if (player !== undefined) {
            player.direction = dir
        }
    }

    startCharge (socket, dir) {
        const player = this.players[socket.id]
        if (player !== undefined) {
            player.direction = dir
            player.charging = true
            // console.log(`${socket.id} is changing`)
        }
    }

    releaseCharge (socket, dir) {
        const player = this.players[socket.id]
        if (player !== undefined) {
            player.direction = dir
            player.charging = false
            // console.log(`${socket.id} is boom`)
        }
    }

    chatPush (socket, message) {
        const player = this.players[socket.id]
        if (player === undefined) return
        const safeMessage = utilsString.validateChat(message)
        if (safeMessage.length === 0) return
        const username = player.username || 'Looper'
        console.log(`M ${player.username}:${message}`)
        this.io.emit(Constants.MSG_OUT.CHAT_UPDATE, {
            t: `${(new Date()).getHours()}:${(new Date()).getMinutes()}`,
            sender: username,
            message: safeMessage
        })
    }

    globalLeaderboard (socket) {
        socket.emit(Constants.MSG_OUT.LEADERBOARD_UPDATE, { count: this.leadersCount, data: this.leaderboard })
    }

    addBot (socket) {
        // TODO isadmin
        if (Object.values(this.players).length > 8) return
        this.counter++
        this.addPlayer(null, `bot${this.counter}`)
    }

    update () {
        const now = Date.now()
        const dt = (now - this.lastUpdateTime) / 1000
        this.lastUpdateTime = now
        chargeSystem(this, dt)
        jointSystem(this, dt)
        collisionSystem(this, dt)
        motionSystem(this, dt)
        boundsSystem(this, dt)
        botSystem(this, dt)
        deathSystem(this, dt)
        starSpawnSystem(this, dt)
        spikeSpawnSystem(this, dt)
        scoreByTimeSystem(this, dt)
        // Send updates every other frame to save bandwith
        if (this.shouldSendUpdate) {
            networkUpdateSystem(this, dt)
            this.shouldSendUpdate = false
        } else {
            this.shouldSendUpdate = true
        }
    }
}

module.exports = Game
