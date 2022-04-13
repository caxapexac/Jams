max_hp = 0;
room_hp = hp;
room_max_hp = max_hp;

ui_score = 0;

score_sound = audio_play_sound(snd_coins, 1, true);
audio_pause_sound(score_sound);