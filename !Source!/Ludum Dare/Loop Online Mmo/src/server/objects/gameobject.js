class GameObject {
    constructor (id, x, y, direction) {
        this.id = id
        this.x = x
        this.y = y
        this.direction = direction
    }

    serializeForUpdate () {
        return {
            id: this.id,
            x: this.x,
            y: this.y,
            direction: this.direction
        }
    }
}

module.exports = GameObject
