import { debounce } from 'throttle-debounce'
import assets from './assets'
import state from './state'
import domElements from './dom_elements'

const Constants = require('../shared/constants')

const { PLAYER_RADIUS, PLAYER_MAX_HP, SPIKE_RADIUS, MAP_SIZE, PLAYER_CHANGE_MAX } = Constants
const canvas = domElements.gameCanvas
const context = canvas.getContext('2d')

function setCanvasDimensions () {
    const scaleRatio = Math.max(1, 900 / window.innerWidth)
    canvas.width = scaleRatio * window.innerWidth
    canvas.height = scaleRatio * window.innerHeight
}

setCanvasDimensions()
window.addEventListener('resize', debounce(40, setCanvasDimensions))

function render () {
    const current = state.getCurrentState()
    window.AState = current
    if (!current.me) return
    // TODO
    domElements.playersCount.innerHTML = current.playersCount

    renderBackground(current.me.x, current.me.y)
    // Draw boundaries
    context.strokeStyle = 'black'
    context.lineWidth = 1
    context.strokeRect(canvas.width / 2 - current.me.x, canvas.height / 2 - current.me.y, MAP_SIZE, MAP_SIZE)
    // Draw all bullets
    // bullets.forEach(renderBullet.bind(null, me))
    current.bondages.forEach(renderBondage.bind(null, current.me, current.others))
    // Draw all players
    renderPlayer(current.me, current.me)
    current.others.forEach(renderPlayer.bind(null, current.me))
    current.spikes.forEach(renderSpike.bind(null, current.me))
    current.stars.forEach(renderStar.bind(null, current.me))
}

function renderBackground (x, y) {
    context.fillStyle = '#eeeeee'
    context.fillRect(0, 0, canvas.width, canvas.height)
    renderGrid(x, y)
    return
    // const backgroundX = MAP_SIZE / 2 - x + canvas.width / 2
    // const backgroundY = MAP_SIZE / 2 - y + canvas.height / 2
    // const backgroundGradient = context.createRadialGradient(
    //     backgroundX,
    //     backgroundY,
    //     MAP_SIZE / 10,
    //     backgroundX,
    //     backgroundY,
    //     MAP_SIZE / 2
    // )
    // backgroundGradient.addColorStop(0, 'black')
    // backgroundGradient.addColorStop(1, 'gray')
    const patternCanvas = document.createElement('canvas')
    patternCanvas.width = 32
    patternCanvas.height = 32
    // patternCanvas.imageSmoothingEnabled = true
    const patternContext = patternCanvas.getContext('2d')
    patternContext.fillStyle = '#F00'
    patternContext.fillRect(1, 1, 30, 30)
    var pattern = context.createPattern(patternCanvas, 'repeat')
    context.save()
    context.translate(x % 32, y % 32)
    context.fillStyle = pattern
    context.fillRect(0, 0, canvas.width, canvas.height)
    context.restore()
    // context.fillStyle = backgroundGradient
    // context.fillRect(0, 0, canvas.width, canvas.height)
}

function renderGrid (x, y) {
    context.lineWidth = 1
    context.strokeStyle = '#161616'
    context.globalAlpha = 0.15
    context.beginPath()

    const xoffset = -0
    const yoffset = -0
    for (var gx = xoffset - x; gx < canvas.width; gx += canvas.height / 18) {
        context.moveTo(gx, 0)
        context.lineTo(gx, canvas.height)
    }

    for (var gy = yoffset - y; gy < canvas.height; gy += canvas.height / 18) {
        context.moveTo(0, gy)
        context.lineTo(canvas.width, gy)
    }

    context.stroke()
    context.globalAlpha = 1
}

// Renders a ship at the given coordinates
function renderPlayer (me, player) {
    const { x, y, direction, username, score } = player
    const canvasX = canvas.width / 2 + x - me.x
    const canvasY = canvas.height / 2 + y - me.y
    // Draw ship
    context.save()
    context.translate(canvasX, canvasY)
    context.rotate(direction)
    context.drawImage(
        player.charging ? assets.getAsset('ship_charge.png') : assets.getAsset('ship.png'),
        -player.radius,
        -player.radius,
        player.radius * 2,
        player.radius * 2
    )
    context.restore()
    // Draw health bar
    context.fillStyle = 'red'
    context.fillRect(
        canvasX - player.radius,
        canvasY + player.radius + 8,
        player.radius * 2,
        4
    )
    context.fillStyle = 'lime'
    context.fillRect(
        canvasX - player.radius,
        canvasY + player.radius + 8,
        player.radius * 2 * (player.hp / PLAYER_MAX_HP),
        4
    )
    // Draw stamina bar
    context.fillStyle = 'yellow'
    context.fillRect(
        canvasX - player.radius,
        canvasY + player.radius + 12,
        player.radius * 2,
        4
    )
    context.fillStyle = 'black'
    context.fillRect(
        canvasX - player.radius + player.radius * 2 * player.chargeCooldown / PLAYER_CHANGE_MAX,
        canvasY + player.radius + 12,
        player.radius * 2 * (1 - player.chargeCooldown / PLAYER_CHANGE_MAX),
        4
    )
    // Draw name
    const name = (player.spectator ? 'Spectator ' : '') + (username || 'Looper')
    const fontSize = Math.max(player.radius / 2, 12)
    context.lineWidth = 3
    context.fillStyle = '#eeeeee'
    context.strokeStyle = '#000000'
    context.miterLimit = 1
    context.lineJoin = 'round'
    context.textAlign = 'center'
    context.textBaseline = 'middle'
    context.font = 'bold ' + fontSize + 'px sans-serif'

    context.strokeText(name, canvasX, canvasY - player.radius * 1.5)
    context.fillText(name, canvasX, canvasY - player.radius * 1.5)
    context.strokeText(score, canvasX, canvasY - player.radius * 2)
    context.fillText(score, canvasX, canvasY - player.radius * 2)

    // if (global.toggleMassState === 0) {
    //     context.strokeText(name, circle.x, circle.y)
    //     context.fillText(name, circle.x, circle.y)
    // } else {
    //     context.strokeText(name, circle.x, circle.y)
    //     context.fillText(name, circle.x, circle.y)
    //     context.font = 'bold ' + Math.max(fontSize / 3 * 2, 10) + 'px sans-serif'
    //     if (name.length === 0) fontSize = 0
    //     context.strokeText(Math.round(cellCurrent.mass), circle.x, circle.y + fontSize)
    //     context.fillText(Math.round(cellCurrent.mass), circle.x, circle.y + fontSize)
    // }
}

function renderBondage (me, others, bondage) {
    const canvasX = canvas.width / 2 - me.x
    const canvasY = canvas.height / 2 - me.y
    let first = null
    let second = null
    if (bondage[0] === me.id) {
        first = me
    } else {
        for (const element of others) {
            if (element.id === bondage[0]) {
                first = element
                break
            }
        }
    }
    if (bondage[1] === me.id) {
        second = me
    } else {
        for (const element of others) {
            if (element.id === bondage[1]) {
                second = element
                break
            }
        }
    }

    if (first !== null && second !== null) {
        context.beginPath()
        context.fillStyle = 'red'
        context.moveTo(canvasX + first.x, canvasY + first.y)
        context.lineTo(canvasX + second.x, canvasY + second.y)
        context.stroke()
    }
}

function renderSpike (me, spike) {
    context.drawImage(
        assets.getAsset('spike.png'),
        canvas.width / 2 + spike.x - me.x - spike.radius,
        canvas.height / 2 + spike.y - me.y - spike.radius,
        spike.radius * 2,
        spike.radius * 2
    )
}

function renderStar (me, star) {
    context.drawImage(
        assets.getAsset('star2.png'),
        canvas.width / 2 + star.x - me.x - star.radius,
        canvas.height / 2 + star.y - me.y - star.radius,
        star.radius * 2,
        star.radius * 2
    )
}

function renderMainMenu () {
    const t = Date.now() / 7500
    const x = MAP_SIZE / 2 + 800 * Math.cos(t)
    const y = MAP_SIZE / 2 + 800 * Math.sin(t)
    renderBackground(x, y)
}

let renderInterval = setInterval(renderMainMenu, 1000 / 60)

// Replaces main menu rendering with game rendering.
function startRenderingGame () {
    console.log('Rendering game')
    clearInterval(renderInterval)
    renderInterval = setInterval(render, 1000 / 60)
}

// Replaces game rendering with main menu rendering.
function startRenderingMainMenu () {
    console.log('Rendering menu')
    clearInterval(renderInterval)
    renderInterval = setInterval(renderMainMenu, 1000 / 60)
}

export default {
    startRenderingGame,
    startRenderingMainMenu
}
