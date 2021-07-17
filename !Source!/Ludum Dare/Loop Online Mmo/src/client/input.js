import networking from './networking'
import domElements from './dom_elements'

function onMouseMove (e) {
    networking.updateDirection(calculateDir(e.clientX, e.clientY))
}

function onMouseDown (e) {
    networking.startCharging(calculateDir(e.clientX, e.clientY))
}

function onMouseUp (e) {
    networking.releaseCharging(calculateDir(e.clientX, e.clientY))
}

function onTouchMove (e) {
    const touch = e.touches[0]
    if (!touch) return
    networking.updateDirection(calculateDir(touch.clientX, touch.clientY))

    e.stopPropagation && e.stopPropagation()
    e.cancelBubble = true
    e.returnValue = false

    // e.preventDefault()
}

function onTouchStart (e) {
    const touch = e.touches[0]
    if (!touch) return
    networking.startCharging(calculateDir(touch.clientX, touch.clientY))
    e.stopPropagation && e.stopPropagation()
    e.cancelBubble = true
    e.returnValue = false
    // e.preventDefault()
}

function onTouchEnd (e) {
    const touch = e.touches[0]
    if (!touch) return
    networking.releaseCharging(calculateDir(touch.clientX, touch.clientY))
    e.stopPropagation && e.stopPropagation()
    e.cancelBubble = true
    e.returnValue = false
    // e.preventDefault()
}

function calculateDir (x, y) {
    return Math.atan2(x - window.innerWidth / 2, window.innerHeight / 2 - y)
}

function onKeyDown (e) {
    if (e.code === 'Backquote') {
        domElements.changeHidden(domElements.chat)
    }
    // switch (e.code) {
    // case 'KeyA':
    // case 'ArrowLeft':
    //     tetris.move(-1)
    //     e.preventDefault ? e.preventDefault() : (e.returnValue = false)
    //     break
    // case 'KeyD':
    // case 'ArrowRight':
    //     tetris.move(1)
    //     e.preventDefault ? e.preventDefault() : (e.returnValue = false)
    //     break
    // case 'KeyW':
    // case 'ArrowUp':
    //     tetris.rotate(1)
    //     e.preventDefault ? e.preventDefault() : (e.returnValue = false)
    //     break
    // case 'KeyS':
    // case 'ArrowDown':
    //     if (!tetris.pause) tetris.push()
    //     e.preventDefault ? e.preventDefault() : (e.returnValue = false)
    //     break
    // case 'Escape':
    //     tetris.stop()
    //     break
    // case 'Enter':
    //     tetris.pause = !tetris.pause
    //     break
    // case 'KeyR':
    //     location.reload()
    //     break
    // }
}

function startCapturingInput () {
    window.addEventListener('mousemove', onMouseMove)
    window.addEventListener('mousedown', onMouseDown)
    window.addEventListener('mouseup', onMouseUp)
    domElements.gameCanvas.addEventListener('touchmove', onTouchMove, { passive: false })
    domElements.gameCanvas.addEventListener('touchstart', onTouchStart, { passive: false })
    domElements.gameCanvas.addEventListener('touchend', onTouchEnd, { passive: false })
    window.addEventListener('keydown', onKeyDown)
}

function stopCapturingInput () {
    window.removeEventListener('mousemove', onMouseMove)
    window.removeEventListener('mousedown', onMouseDown)
    window.removeEventListener('mouseup', onMouseUp)
    domElements.gameCanvas.removeEventListener('touchmove', onTouchMove)
    domElements.gameCanvas.removeEventListener('touchstart', onTouchStart)
    domElements.gameCanvas.removeEventListener('touchend', onTouchEnd)
    window.removeEventListener('keydown', onKeyDown)
}

export default {
    startCapturingInput,
    stopCapturingInput
}
