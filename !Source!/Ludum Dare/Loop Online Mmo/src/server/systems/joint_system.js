const Constants = require('../../shared/constants')
const utilsObject = require('../../shared/utils_object')

module.exports = function (game, dt) {
    const playersArray = Object.values(game.players)
    // console.log(playersArray)
    // console.log(game.bondages)
    for (let i = 0; i < playersArray.length; i++) {
        const player = playersArray[i]
        if (player.spectator === true) continue
        let forceX = 0
        let forceY = 0
        for (let k = 0; k < playersArray.length; k++) {
            if (i === k) continue
            const enemy = playersArray[k]
            const { dx, dy, dist } = utilsObject.vecDistance(player, enemy)
            // if (dist < Constants.LOOP_PUSH_RADIUS) {
            //     forceX += (dx * Constants.LOOP_PUSH_FORCE) / dist
            //     forceY += (dy * Constants.LOOP_PUSH_FORCE) / dist
            // } else if (dist > Constants.LOOP_PULL_RADIUS) {
            //     // TODO
            // }
            if (dist !== 0) {
                forceX += (dx * Constants.LOOP_PUSH_FORCE) / dist ^ 2
                forceY += (dy * Constants.LOOP_PUSH_FORCE) / dist ^ 2
            }
        }
        for (let k = 0; k < game.bondages.length; k++) {
            const bondage = game.bondages[k]
            let enemy = null
            if (bondage[0] === player.id && bondage[1] !== player.id) {
                enemy = game.players[bondage[1]]
            } else if (bondage[1] === player.id && bondage[0] !== player.id) {
                enemy = game.players[bondage[0]]
            } else {
                continue
            }
            const { dx, dy, dist } = utilsObject.vecDistance(player, enemy)
            // if (dist < Constants.LOOP_PUSH_RADIUS) {
            //     // TODO
            // } else if (dist > Constants.LOOP_PULL_RADIUS) {
            //     forceX -= dx * Constants.LOOP_PULL_FORCE / dist
            //     forceY -= dy * Constants.LOOP_PULL_FORCE / dist
            // }
            forceX -= dx * Constants.LOOP_PULL_FORCE * dist
            forceY -= dy * Constants.LOOP_PULL_FORCE * dist
        }
        if (Math.abs(forceX) < 0.1 && Math.abs(forceY) < 0.1) {
            forceX = 0
            forceY = 0
        }
        player.forceX = forceX
        player.forceY = forceY
        player.x += dt * player.forceX // TODO
        player.y += dt * player.forceY // TODO
        // console.log(`${player.forceX} ${player.forceY}`)
    }
}
