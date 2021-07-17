const CollidableObject = require('./collidableobject')
const Constants = require('../../shared/constants')

class Player extends CollidableObject {
    constructor (id, x, y, username) {
        super(id, x, y, Math.random() * 2 * Math.PI, Constants.PLAYER_RADIUS)
        this.username = username
        this.hp = Constants.PLAYER_MAX_HP
        this.score = 0
        this.charging = false
        this.chargeCooldown = 0
        this.speed = Constants.PLAYER_SPEED
        this.forceX = 0
        this.forceY = 0
        this.spectator = false
    }

    serializeForUpdate () {
        return {
            ...(super.serializeForUpdate()),
            username: this.username,
            hp: this.hp,
            score: this.score,
            charging: this.charging,
            chargeCooldown: this.chargeCooldown,
            spectator: this.spectator
            // chargeForce,
            // speed,
            // forceX,
            // forceY
        }
    }
}

module.exports = Player
