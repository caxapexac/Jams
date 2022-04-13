instance_create_layer(x, y, "Regions", obj_fx_took);
audio_play_sound_at(snd_take_item, x, y, 0, 100, 300, 1, false, 100);
on_take();
instance_destroy();