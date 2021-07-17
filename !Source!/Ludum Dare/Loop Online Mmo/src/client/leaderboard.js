import escape from 'lodash/escape'
import domElements from './dom_elements'

const leaderboardRows = document.querySelectorAll('#leaderboard table tr')
const globalLeaderboardRows = document.querySelectorAll('#global-leaderboard table tr')

function updateLeaderboard (data, me) {
    // This is a bit of a hacky way to do this and can get dangerous if you don't escape usernames
    // properly. You would probably use something like React instead if this were a bigger project.
    for (let i = 0; i < data.length; i++) {
        leaderboardRows[i + 1].innerHTML = `<td>${escape(data[i].username.slice(0, 15)) || 'Looper'}</td><td>${
            data[i].score
        }</td>`
    }
    for (let i = data.length; i < 5; i++) {
        leaderboardRows[i + 1].innerHTML = '<td>-</td><td>-</td>'
    }

    domElements.score.innerHTML = Math.round(me.score)
}

function updateGlobalLeaderboard (dataWithCount) {
    console.log(dataWithCount)
    for (let i = 0; i < dataWithCount.data.length; i++) {
        console.log('UPD')
        globalLeaderboardRows[i + 1].innerHTML = `<td>${escape(dataWithCount.data[i].username.slice(0, 32)) || 'Looper'}</td><td>${dataWithCount.data[i].score}</td>`
    }
    domElements.totalPlays.innerHTML = dataWithCount.count
}

export default {
    updateLeaderboard,
    updateGlobalLeaderboard
}
