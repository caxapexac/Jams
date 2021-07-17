const Constants = require('../../shared/constants')
const utilsObject = require('../../shared/utils_object')
const Spike = require('../objects/spike')

let counter = 0

module.exports = function (game, dt) {
    if (game.spikes.length < Constants.SPIKES_COUNT) {
        const x = Constants.MAP_SIZE * (0.05 + Math.random() * 0.9)
        const y = Constants.MAP_SIZE * (0.05 + Math.random() * 0.9)
        const spike = new Spike(`spike${counter}`, x, y)
        counter++
        game.spikes.push(spike)
    }
}
