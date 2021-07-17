const Constants = require('../../shared/constants')
const utilsObject = require('../../shared/utils_object')
const Star = require('../objects/star')

let counter = 0

module.exports = function (game, dt) {
    if (game.stars.length < Object.values(game.players).length * 2) {
        const x = Constants.MAP_SIZE * (0.2 + Math.random() * 0.8)
        const y = Constants.MAP_SIZE * (0.2 + Math.random() * 0.8)
        const star = new Star(`star${counter}`, x, y)
        counter++
        game.stars.push(star)
    }
}
