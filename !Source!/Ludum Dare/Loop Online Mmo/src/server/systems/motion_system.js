const Constants = require('../../shared/constants')

module.exports = function (game, dt) {
    Object.keys(game.sockets).forEach(playerID => {
        const player = game.players[playerID]
        if (player.charging) {
            player.x += dt * player.speed * Math.sin(player.direction) * Constants.PLAYER_CHARGE_MULTIPILER
            player.y -= dt * player.speed * Math.cos(player.direction) * Constants.PLAYER_CHARGE_MULTIPILER
        } else {
            player.x += dt * player.speed * Math.sin(player.direction)
            player.y -= dt * player.speed * Math.cos(player.direction)
        }
    })
    game.stars.forEach(star => {
        star.x += dt * star.speed * Math.sin(star.direction)
        star.y -= dt * star.speed * Math.cos(star.direction)
    })
    game.spikes.forEach(spike => {
        spike.x += dt * Constants.SPIKE_SPEED * Math.sin(spike.direction)
        spike.y -= dt * Constants.SPIKE_SPEED * Math.cos(spike.direction)
    })
}
