max_hp = max(max_hp, hp);

var offset_x = 5;
var offset_y = 5;

var layout_x = offset_x;
var layout_y = offset_y;

var hp_sprite = spr_gui_hp;
var hp_element_width = sprite_get_width(hp_sprite);
var hp_element_height = sprite_get_height(hp_sprite);

var hp_by_element = sprite_get_number(hp_sprite) - 1;
var hp_elements = ceil(max_hp / hp_by_element);
for(var i = 0; i < hp_elements; i++) {
	var local_hp = clamp(hp - i * hp_by_element, 0, hp_by_element);
	var frame_index = hp_by_element - local_hp;
	draw_sprite(hp_sprite, frame_index, layout_x, layout_y);
	layout_x += hp_element_width + offset_x;
}

layout_x = offset_x;
layout_y += hp_element_height + offset_y;

if(easy_jump) {
	var sprite = object_get_sprite(obj_bonus_easy_jump);	
	gui_draw_sprite(sprite, layout_x, layout_y);
	layout_x += offset_x + sprite_get_width(sprite);
}

if(take_under) {
	var sprite = object_get_sprite(obj_bonus_take_under);	
	gui_draw_sprite(sprite, layout_x, layout_y);
	layout_x += offset_x + sprite_get_width(sprite);
}

ui_score += (score - ui_score) * 0.1;
draw_set_color(c_yellow);
draw_set_font(fnt_score);
var draw_score = round(ui_score);

if(draw_score != score) {
	audio_resume_sound(score_sound);
} else {
	audio_pause_sound(score_sound);
}

var score_text = string(draw_score);
var text_x = view_get_wport(view_camera[0]) - string_width(score_text) - offset_x;
draw_text(text_x, 0, score_text);

//string_width(score_text)