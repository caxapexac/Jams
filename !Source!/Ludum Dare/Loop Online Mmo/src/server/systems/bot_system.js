const Constants = require('../../shared/constants')
const utilsObject = require('../../shared/utils_object')

module.exports = function (game, dt) {
    Object.keys(game.sockets).forEach(playerID => {
        if (playerID.startsWith('bot')) {
            const bot = game.players[playerID]
            if (bot.lastChangedDir === undefined) bot.lastChangedDir = 0
            bot.lastChangedDir += dt
            if (bot.x === 0 || bot.x === Constants.MAP_SIZE || bot.y === 0 || bot.y === Constants.MAP_SIZE) {
                bot.direction = (Math.atan2(Constants.MAP_SIZE / 2 - bot.x, bot.y - Constants.MAP_SIZE / 2) + Math.PI * 2) % (Math.PI * 2)
                if (bot.chargeCooldown > Constants.PLAYER_CHANGE_MAX / 3) {
                    bot.charging = true
                }
            } else if (bot.lastChangedDir >= Constants.BOT_REACTION_TIME) {
                bot.lastChangedDir = 0
                game.spikes.forEach(spike => {
                    const { dx, dy, dist } = utilsObject.vecDistance(bot, spike)
                    if (dist < (bot.radius + spike.radius) * 2) {
                        if (bot.chargeCooldown > Constants.PLAYER_CHANGE_MAX / 2) {
                            bot.charging = true
                        }
                        bot.direction = (Math.atan2(dx, dy) + Math.PI * 2) % (Math.PI * 2)
                    }
                })
                if (bot.charging === false && bot.chargeCooldown > Constants.PLAYER_CHANGE_MAX / 5 * 4) {
                    bot.charging = true
                    bot.direction = Math.random() * 2 * Math.PI
                }
            }
        }
    })
}
