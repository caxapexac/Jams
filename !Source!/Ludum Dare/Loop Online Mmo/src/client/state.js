import leaderboard from './leaderboard'
import stateInterpolation from './state_interpolation'

// The "current" state will always be RENDER_DELAY ms behind server time.
// This makes gameplay smoother and lag less noticeable.
const RENDER_DELAY = 100

const gameUpdates = []
let gameStart = 0
let firstServerTimestamp = 0

export function initState () {
    gameStart = 0
    firstServerTimestamp = 0
}

export function processGameUpdate (update) {
    if (!firstServerTimestamp) {
        firstServerTimestamp = update.t
        gameStart = Date.now()
    }
    gameUpdates.push(update)
    leaderboard.updateLeaderboard(update.leaderboard, update.me)
    // Keep only one game update before the current server time
    const base = getBaseUpdate()
    if (base > 0) {
        gameUpdates.splice(0, base)
    }
}

export function getCurrentState () {
    if (!firstServerTimestamp) return {}
    const base = getBaseUpdate()
    const serverTime = currentServerTime()
    // If base is the most recent update we have, use its state.
    // Otherwise, interpolate between its state and the state of (base + 1).
    if (base < 0 || base === gameUpdates.length - 1) {
        return gameUpdates[gameUpdates.length - 1]
    } else {
        const baseUpdate = gameUpdates[base]
        const next = gameUpdates[base + 1]
        const ratio = (serverTime - baseUpdate.t) / (next.t - baseUpdate.t)
        return {
            me: stateInterpolation.interpolateObject(baseUpdate.me, next.me, ratio),
            others: stateInterpolation.interpolateObjectArray(baseUpdate.others, next.others, ratio),
            bondages: next.bondages,
            spikes: stateInterpolation.interpolateObjectArray(baseUpdate.spikes, next.spikes, ratio),
            stars: stateInterpolation.interpolateObjectArray(baseUpdate.stars, next.stars, ratio),
            playersCount: next.playersCount
        }
    }
}

function currentServerTime () {
    return firstServerTimestamp + (Date.now() - gameStart) - RENDER_DELAY
}

// Returns the index of the base update, the first game update before
// current server time, or -1 if N/A.
function getBaseUpdate () {
    const serverTime = currentServerTime()
    for (let i = gameUpdates.length - 1; i >= 0; i--) {
        if (gameUpdates[i].t <= serverTime) {
            return i
        }
    }
    return -1
}

export default {
    initState,
    processGameUpdate,
    getCurrentState
}
