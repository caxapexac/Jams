const playMenu = document.getElementById('play-menu')
const playButton = document.getElementById('play-button')
const spectateButton = document.getElementById('spectate-button')
const usernameInput = document.getElementById('username-input')
const chat = document.getElementById('chat')
const chatInput = document.getElementById('chat-input')
const chatButton = document.getElementById('chat-button')
const chatList = document.getElementById('chat-list')
const chatHideButton = document.getElementById('chat-hide-button')
const addBotButton = document.getElementById('add-bot-button')
const playersCount = document.getElementById('players-count')
const score = document.getElementById('score')
const leaderboard = document.getElementById('leaderboard')
const globalLeaderboard = document.getElementById('global-leaderboard')
const totalPlays = document.getElementById('total-plays')
const disconnectModal = document.getElementById('disconnect-modal')
const reconnectButton = document.getElementById('reconnect-button')
const gameCanvas = document.getElementById('game-canvas')

reconnectButton.onclick = () => {
    window.location.reload()
}

chatInput.addEventListener('keyup', (e) => {
    if (e.key !== 'Enter') return
    chatButton.click()
    e.preventDefault()
})

chatHideButton.addEventListener('click', (e) => {
    changeHidden(chat)
})

function setHidden (domElement, hidden) {
    if (hidden) {
        domElement.classList.add('hidden')
    } else {
        domElement.classList.remove('hidden')
    }
}

function changeHidden (domElement) {
    if (!domElement.classList.contains('hidden')) {
        domElement.classList.add('hidden')
    } else {
        domElement.classList.remove('hidden')
    }
}

export default {
    setHidden,
    changeHidden,
    playMenu,
    playButton,
    spectateButton,
    usernameInput,
    chat,
    chatInput,
    chatButton,
    chatList,
    chatHideButton,
    addBotButton,
    playersCount,
    score,
    leaderboard,
    globalLeaderboard,
    totalPlays,
    disconnectModal,
    reconnectButton,
    gameCanvas
}
