const Constants = require('../../shared/constants')

module.exports = function (game, dt) {
    Object.keys(game.sockets).forEach(playerID => {
        const player = game.players[playerID]
        if (player.charging && player.spectator === false) {
            player.chargeCooldown -= dt
            if (player.chargeCooldown <= 0) {
                player.charging = false
            }
        } else {
            player.chargeCooldown = Math.min(Constants.PLAYER_CHANGE_MAX, player.chargeCooldown + dt)
        }

        // if (player.charging) {
        //     player.chargeForce += dt * Constants.PLAYER_CHARGE_SPEED
        //     // TODO ma charge force auto psuh
        // } else if (player.chargeForce > 0) {
        //     player.x += dt * player.chargeForce * Math.sin(player.direction)
        //     player.y -= dt * player.chargeForce * Math.cos(player.direction)
        //     player.chargeForce = 0
        //     player.chargeCooldown = Constants.PLAYER_CHARGE_COOLDOWN
        // } else {

        // }
    })
}
