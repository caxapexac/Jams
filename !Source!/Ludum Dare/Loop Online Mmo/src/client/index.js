import networking from './networking'
import render from './render'
import input from './input'
import assets from './assets'
import state from './state'
import domElements from './dom_elements'
import './css/bootstrap-reboot.css'
import './css/main.css'

Promise.all([
    networking.connect(onGameOver),
    assets.downloadAssets()
]).then(() => {
    domElements.setHidden(domElements.playMenu, false)
    domElements.setHidden(domElements.globalLeaderboard, false)
    domElements.usernameInput.focus()
    domElements.playButton.onclick = () => {
        domElements.setHidden(domElements.playMenu, true)
        domElements.setHidden(domElements.leaderboard, false)
        domElements.setHidden(domElements.chat, false)
        domElements.setHidden(domElements.globalLeaderboard, true)
        networking.play(domElements.usernameInput.value)
        state.initState()
        input.startCapturingInput()
        render.startRenderingGame()
    }
    domElements.spectateButton.onclick = () => {
        domElements.setHidden(domElements.playMenu, true)
        domElements.setHidden(domElements.leaderboard, false)
        domElements.setHidden(domElements.chat, false)
        domElements.setHidden(domElements.globalLeaderboard, true)
        networking.spectate(domElements.usernameInput.value)
        state.initState()
        input.startCapturingInput()
        render.startRenderingGame()
    }
}).catch(console.error)

function onGameOver () {
    input.stopCapturingInput()
    render.startRenderingMainMenu()
    domElements.setHidden(domElements.playMenu, false)
    domElements.setHidden(domElements.leaderboard, true)
    domElements.setHidden(domElements.chat, true)
    domElements.setHidden(domElements.globalLeaderboard, false)
}
