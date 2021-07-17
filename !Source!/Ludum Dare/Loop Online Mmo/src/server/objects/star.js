const CollidableObject = require('./collidableobject')
const Constants = require('../../shared/constants')

class Star extends CollidableObject {
    constructor (id, x, y) {
        super(id, x, y, Math.random() * 2 * Math.PI, Constants.STAR_RADIUS * (1 + Math.random() * Constants.STAR_RADIUS_DEVIATION - Math.random() * Constants.STAR_RADIUS_DEVIATION))
        this.speed = Constants.STAR_SPEED
    }

    serializeForUpdate () {
        return {
            ...(super.serializeForUpdate())
        }
    }
}

module.exports = Star
