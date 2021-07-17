exports.rand = function (array) {
    // "|" for a kinda "int div"
    return array[array.length * Math.random() | 0]
}

exports.pick = function (array, i) {
    return array.splice(i >= 0 ? i : Math.random() * array.length | 0, 1)[0]
}

exports.shuffle = function (array) {
    for (var i = array.length; i > 0; --i) { array.push(array.splice(Math.random() * i | 0, 1)[0]) }
    return array
}
