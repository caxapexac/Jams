const canvas = document.getElementById('canvas')
const context = canvas.getContext('2d')

var ptrn = context.createPattern(canvas, 'repeat')
context.fillStyle = ptrn
context.fillRect(0, 0, canvas.width, canvas.height)
// context.fillStyle = backgroundGradient
