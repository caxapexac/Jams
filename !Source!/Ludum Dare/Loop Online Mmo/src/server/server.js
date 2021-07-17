const express = require('express')
const webpack = require('webpack')
const webpackDevMiddleware = require('webpack-dev-middleware')
const socketio = require('socket.io')

const Constants = require('../shared/constants')
const Game = require('./game')
const webpackConfig = require('../../webpack.dev.js')

const port = process.env.PORT || 3000
const app = express()

app.use(express.static('public'))

if (process.env.NODE_ENV === 'development') {
    const compiler = webpack(webpackConfig)
    app.use(webpackDevMiddleware(compiler))
} else {
    app.use(express.static('dist'))
}

const server = app.listen(port, () => {
    console.log(`Server listening on port ${port}`)
})

const io = socketio(server)
const game = new Game(io)

io.on('connection', socket => {
    console.log('Player connected!', socket.id)
    socket.on(Constants.MSG_IN.JOIN_GAME, (username) => game.addPlayer(socket, username))
    socket.on(Constants.MSG_IN.SPECTATE_GAME, (username) => game.addSpectator(socket, username))
    socket.on(Constants.MSG_IN.CHANGE_DIRECTION, (dir) => game.changeDirection(socket, dir))
    socket.on(Constants.MSG_IN.START_CHARGE, (dir) => game.startCharge(socket, dir))
    socket.on(Constants.MSG_IN.RELEASE_CHARGE, (dir) => game.releaseCharge(socket, dir))
    socket.on(Constants.MSG_IN.CHAT_PUSH, (message) => game.chatPush(socket, message))
    socket.on(Constants.MSG_IN.GLOBAL_LEADERBOARD, () => game.globalLeaderboard(socket))
    socket.on(Constants.MSG_IN.ADD_BOT, () => game.addBot(socket))
    socket.on('disconnect', () => game.removePlayer(socket))
})
