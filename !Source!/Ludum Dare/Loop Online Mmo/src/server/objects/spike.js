const CollidableObject = require('./collidableobject')
const Constants = require('../../shared/constants')

class Spike extends CollidableObject {
    constructor (id, x, y) {
        super(id, x, y, Math.random() * 2 * Math.PI, Constants.SPIKE_RADIUS * (1 + Math.random() * Constants.SPIKE_RADIUS_DEVIATION - Math.random() * Constants.SPIKE_RADIUS_DEVIATION))
        this.damage = Constants.SPIKE_DAMAGE
    }

    serializeForUpdate () {
        return {
            ...(super.serializeForUpdate()),
            damage: this.damage
        }
    }
}

module.exports = Spike
