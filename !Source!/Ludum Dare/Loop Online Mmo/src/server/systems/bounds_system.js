const Constants = require('../../shared/constants')

module.exports = function (game, dt) {
    Object.keys(game.sockets).forEach(playerID => {
        const player = game.players[playerID]
        player.x = Math.max(0, Math.min(Constants.MAP_SIZE, player.x))
        player.y = Math.max(0, Math.min(Constants.MAP_SIZE, player.y))
        // if (player.x === 0 || player.x === Constants.MAP_SIZE || player.y === 0 || player.y === Constants.MAP_SIZE) {
        //     player.direction = (player.direction + Math.PI) % (Math.PI * 2)
        // }
    })
    game.stars.forEach(star => {
        star.x = Math.max(0, Math.min(Constants.MAP_SIZE, star.x))
        star.y = Math.max(0, Math.min(Constants.MAP_SIZE, star.y))
        if (star.x === 0 || star.x === Constants.MAP_SIZE || star.y === 0 || star.y === Constants.MAP_SIZE) {
            star.direction = (star.direction + Math.PI / 2) % (Math.PI * 2)
        }
    })
    game.spikes.forEach(spike => {
        spike.x = Math.max(0, Math.min(Constants.MAP_SIZE, spike.x))
        spike.y = Math.max(0, Math.min(Constants.MAP_SIZE, spike.y))
        if (spike.x === 0 || spike.x === Constants.MAP_SIZE || spike.y === 0 || spike.y === Constants.MAP_SIZE) {
            spike.direction = (spike.direction + Math.PI / 2) % (Math.PI * 2)
        }
    })
}
