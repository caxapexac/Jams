//instance_create_layer(x, y, layer, obj_animation).set_sprite(spr_break_box);

var place_x = (bbox_left + bbox_right) * 0.5;
var place_y = (bbox_top + bbox_bottom) * 0.5;

instance_create_layer(place_x, place_y, layer, obj_fx_destruction).sprite_source = sprite_index;

if(content != noone) {
	instance_activate_object(content);
	content.dropped = true;
	content.x = x;
	content.y = y;
}

if(is_visible()) {
	audio_play_sound_at(snd_destroy_box, x, y, 0, 100, 300, 1, false, 100);
}


//audio_play_sound_at(snd_destroy_box, x, y, 0, 100, 300, 1, false, 1);

//audio_play_sound(snd_destroy_box, 0, false);
//audio_play_sound_at(snd_destroy_box, x, y, 0, 0.1, 100, 1, false, 0);