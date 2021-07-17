exports.distance = function (object1, object2) {
    const dx = object1.x - object2.x
    const dy = object1.y - object2.y
    return Math.sqrt(dx * dx + dy * dy)
}

exports.vecDistance = function (object1, object2) {
    const dx = object1.x - object2.x
    const dy = object1.y - object2.y
    return {
        dx: dx,
        dy: dy,
        dist: Math.sqrt(dx * dx + dy * dy)
    }
}
