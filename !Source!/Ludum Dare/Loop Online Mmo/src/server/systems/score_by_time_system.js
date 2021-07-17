const Constants = require('../../shared/constants')

module.exports = function (game, dt) {
    Object.keys(game.sockets).forEach(playerID => {
        // const player = game.players[playerID]
        // player.score += dt * Constants.SCORE_PER_SECOND
    })
}
