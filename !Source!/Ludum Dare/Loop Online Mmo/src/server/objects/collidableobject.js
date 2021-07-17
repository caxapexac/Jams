const GameObject = require('./gameobject')
const Constants = require('../../shared/constants')

class CollidableObject extends GameObject {
    constructor (id, x, y, direction, radius) {
        super(id, x, y, direction)
        this.radius = radius
    }

    serializeForUpdate () {
        return {
            ...(super.serializeForUpdate()),
            radius: this.radius
        }
    }
}

module.exports = CollidableObject
