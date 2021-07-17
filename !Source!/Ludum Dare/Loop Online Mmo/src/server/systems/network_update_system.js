const Constants = require('../../shared/constants')
const utilsObject = require('../../shared/utils_object')

function getLeaderboard (game) {
    return Object.values(game.players)
        .filter(player => player.spectator === false)
        .sort((p1, p2) => p2.score - p1.score)
        .slice(0, 5)
        .map(player => ({ username: player.username, score: Math.round(player.score) }))
}

function getPlayerUpdate (game, player, leaderboard) {
    const nearbyEnemies = Object.values(game.players).filter(
        enemy => enemy !== player && utilsObject.distance(player, enemy) <= Constants.MAP_SIZE / 2
    )
    const nearbySpikes = game.spikes.filter(
        spike => utilsObject.distance(player, spike) <= Constants.MAP_SIZE / 2
    )
    return {
        t: Date.now(),
        me: player.serializeForUpdate(),
        others: nearbyEnemies.map(enemy => enemy.serializeForUpdate()),
        spikes: nearbySpikes.map(spike => spike.serializeForUpdate()),
        stars: game.stars,
        playersCount: Object.values(game.players).filter((enemy) => !enemy.id.startsWith('bot')).length,
        bondages: game.bondages,
        leaderboard
    }
}

module.exports = function (game, dt) {
    const leaderboard = getLeaderboard(game)
    Object.keys(game.sockets).forEach(playerID => {
        const socket = game.sockets[playerID]
        const player = game.players[playerID]
        socket.emit(Constants.MSG_OUT.GAME_UPDATE, getPlayerUpdate(game, player, leaderboard))
    })
}
