import io from 'socket.io-client'
import { throttle } from 'throttle-debounce'
import state from './state'
import domElements from './dom_elements'
import leaderboard from './leaderboard'
const Constants = require('../shared/constants')

const socketProtocol = (window.location.protocol.includes('https')) ? 'wss' : 'ws'
const socket = io(`${socketProtocol}://${window.location.host}`, { reconnection: false })

const connectedPromise = new Promise(resolve => {
    socket.on('connect', () => {
        console.log('Connected to server!')
        resolve()
    })
})

const connect = onGameOver => (
    connectedPromise.then(() => {
    // Register callbacks
        socket.on(Constants.MSG_OUT.GAME_UPDATE, state.processGameUpdate)
        socket.on(Constants.MSG_OUT.GAME_OVER, onGameOver)
        socket.on(Constants.MSG_OUT.CHAT_UPDATE, onChatUpdate)
        socket.on(Constants.MSG_OUT.LEADERBOARD_UPDATE, onLeaderboardUpdate)
        socket.on('disconnect', () => {
            console.log('Disconnected from server.')
            domElements.setHidden(domElements.disconnectModal, false)
        })
        socket.emit(Constants.MSG_IN.GLOBAL_LEADERBOARD)
    })
)

const spectate = username => {
    socket.emit(Constants.MSG_IN.SPECTATE_GAME, username)
}

const play = username => {
    socket.emit(Constants.MSG_IN.JOIN_GAME, username)
}

const updateDirection = throttle(20, dir => {
    socket.emit(Constants.MSG_IN.CHANGE_DIRECTION, dir)
})

const startCharging = throttle(20, dir => {
    socket.emit(Constants.MSG_IN.START_CHARGE, dir)
})

const releaseCharging = throttle(20, dir => {
    socket.emit(Constants.MSG_IN.RELEASE_CHARGE, dir)
})

domElements.chatButton.addEventListener('click', (e) => {
    if (domElements.chatInput.length < 1) return
    socket.emit(Constants.MSG_IN.CHAT_PUSH, domElements.chatInput.value)
    domElements.chatInput.value = ''
})

domElements.addBotButton.addEventListener('click', (e) => {
    socket.emit(Constants.MSG_IN.ADD_BOT)
})

const onChatUpdate = (chatLine) => {
    const newline = document.createElement('li')
    newline.innerHTML = `<b>${chatLine.sender}</b>:${chatLine.message}`
    if (domElements.chatList.childNodes.length > 10) {
        domElements.chatList.removeChild(domElements.chatList.childNodes[0])
    }
    domElements.chatList.appendChild(newline)
}

const onLeaderboardUpdate = (data) => {
    console.log('on update')
    leaderboard.updateGlobalLeaderboard(data)
}

// TODO WASD

export default {
    connect,
    spectate,
    play,
    updateDirection,
    startCharging,
    releaseCharging
}
