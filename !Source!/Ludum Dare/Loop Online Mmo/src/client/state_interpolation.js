export function interpolateObject (object1, object2, ratio) {
    if (!object2) {
        return object1
    }

    const interpolated = {}
    Object.keys(object1).forEach(key => {
        if (key === 'direction') {
            interpolated[key] = interpolateDirection(object1[key], object2[key], ratio)
        } else if (key === 'id' || key === 'charging' || key === 'username' || key === 'score') {
            interpolated[key] = object2[key]
        } else {
            interpolated[key] = object1[key] + (object2[key] - object1[key]) * ratio
        }
    })
    return interpolated
}

export function interpolateObjectArray (objects1, objects2, ratio) {
    return objects1.map(o => interpolateObject(o, objects2.find(o2 => o.id === o2.id), ratio))
}

// Determines the best way to rotate (cw or ccw) when interpolating a direction.
// For example, when rotating from -3 radians to +3 radians, we should really rotate from
// -3 radians to +3 - 2pi radians.
export function interpolateDirection (d1, d2, ratio) {
    const absD = Math.abs(d2 - d1)
    if (absD >= Math.PI) {
    // The angle between the directions is large - we should rotate the other way
        if (d1 > d2) {
            return d1 + (d2 + 2 * Math.PI - d1) * ratio
        } else {
            return d1 - (d2 - 2 * Math.PI - d1) * ratio
        }
    } else {
    // Normal interp
        return d1 + (d2 - d1) * ratio
    }
}

export default {
    interpolateObject,
    interpolateObjectArray,
    interpolateDirection
}
