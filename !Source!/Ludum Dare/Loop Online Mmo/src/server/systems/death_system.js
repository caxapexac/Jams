const Constants = require('../../shared/constants')
const Star = require('../objects/star')

let counter = 0

module.exports = function (game, dt) {
    Object.keys(game.sockets).forEach(playerID => {
        const player = game.players[playerID]
        if (player.hp <= 0) {
            for (let i = 0; i < player.score / 300; i++) {
                const star = new Star(`stard${counter}`, player.x, player.y)
                star.radius /= 2
                star.speed /= 5
                counter++
                game.stars.push(star)
            }
            for (const bondage of game.bondages) {
                if (bondage[0] === playerID && bondage[1] !== playerID) {
                    const enemy = game.players[bondage[1]]
                    if (enemy !== undefined) {
                        enemy.score += Math.round(player.score * 0.1)
                        // enemy.speed += player.speed / 4
                        // enemy.radius += player.radius / 4
                    }
                } else if (bondage[1] === playerID && bondage[0] !== playerID) {
                    const enemy = game.players[bondage[0]]
                    if (enemy !== undefined) {
                        enemy.score += Math.round(player.score * 0.1)
                        // enemy.speed += player.speed / 4
                        // enemy.radius += player.radius / 4
                    }
                }
            }
            const socket = game.sockets[playerID]
            socket.emit(Constants.MSG_OUT.GAME_OVER)
            game.removePlayer(socket)
        }
    })
}
