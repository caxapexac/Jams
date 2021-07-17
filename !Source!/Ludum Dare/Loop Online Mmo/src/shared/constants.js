module.exports = Object.freeze({
    PLAYER_RADIUS: 40,
    PLAYER_MAX_HP: 80,
    PLAYER_SPEED: 50,
    PLAYER_CHANGE_MAX: 2.5,
    PLAYER_CHARGE_MULTIPILER: 5,

    SCORE_PER_SECOND: 1,

    // LOOP_PUSH_RADIUS: 300, // Отталкивание начинается после этого расстояния
    LOOP_PUSH_FORCE: 20, // С этой силой
    // LOOP_PULL_RADIUS: 100, // Притягивание начинается после этого расстояния
    LOOP_PULL_FORCE: 0.002, // С этой силой

    MAP_SIZE: 3000,
    SPIKES_COUNT: 80,
    SPIKE_SPEED: 5,
    SPIKE_RADIUS: 40,
    SPIKE_RADIUS_DEVIATION: 0.5,
    SPIKE_DAMAGE: 10,
    SPIKE_BUMP_FORCE: 50,

    STAR_SPEED_BONUS: 20,
    STAR_RADIUS_BONUS: 5,
    STAR_HP_BONUS: 30,
    STAR_SPEED: 150,
    STAR_RADIUS: 30,
    STAR_RADIUS_DEVIATION: 0.2,

    BOT_REACTION_TIME: 0.8,

    MSG_IN: {
        SPECTATE_GAME: 'spectate_game',
        JOIN_GAME: 'join_game',
        CHANGE_DIRECTION: 'change_direction',
        START_CHARGE: 'start_charge',
        RELEASE_CHARGE: 'release_charge',
        CHAT_PUSH: 'chat_push',
        GLOBAL_LEADERBOARD: 'global_leaderboard',
        ADD_BOT: 'add_bot'
    },
    MSG_OUT: {
        CHAT_UPDATE: 'chat_update',
        GAME_INIT: 'game_init',
        GAME_UPDATE: 'update',
        GAME_OVER: 'dead',
        LEADERBOARD_UPDATE: 'leaderboard_update'
    }
})
