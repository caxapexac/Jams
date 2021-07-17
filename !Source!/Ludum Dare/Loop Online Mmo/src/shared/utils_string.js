exports.validateNickname = function (nickname) {
    return nickname.replace(/^[^\w]*$/, '').replace(/(<([^>]+)>)/ig, '').trim()
}

// Убирает теги
exports.validateChat = function (message) {
    return message.replace(/(<([^>]+)>)/ig, '').trim()
}
