const Constants = require('../../shared/constants')
const utilsObject = require('../../shared/utils_object')

function removeItemOnce (arr, value) {
    var index = arr.indexOf(value)
    if (index > -1) {
        arr.splice(index, 1)
    }
    return arr
}

module.exports = function (game, dt) {
    Object.keys(game.sockets).forEach(playerID => {
        const player = game.players[playerID]
        if (player.spectator !== true) {
            game.spikes.forEach(spike => {
                const { dx, dy, dist } = utilsObject.vecDistance(player, spike)
                if (dist < player.radius + spike.radius) {
                    player.hp -= Constants.SPIKE_DAMAGE
                    player.chargeCooldown = Constants.PLAYER_CHANGE_MAX
                    player.x += dx * Constants.SPIKE_BUMP_FORCE / dist
                    player.y += dy * Constants.SPIKE_BUMP_FORCE / dist
                    spike.x -= dx * Constants.SPIKE_BUMP_FORCE / dist / 2
                    spike.y -= dy * Constants.SPIKE_BUMP_FORCE / dist / 2
                    // Damage
                }
            })
            const toRemove = []
            game.stars.forEach(star => {
                const { dx, dy, dist } = utilsObject.vecDistance(player, star)
                if (dist < player.radius + star.radius) {
                    player.score += 100
                    player.speed += Constants.STAR_SPEED_BONUS
                    player.hp += Constants.STAR_HP_BONUS
                    player.radius += Constants.STAR_RADIUS_BONUS
                    player.chargeCooldown = 0
                    toRemove.push(star)
                }
            })
            toRemove.forEach(element => removeItemOnce(game.stars, element))
        }
    })
}
