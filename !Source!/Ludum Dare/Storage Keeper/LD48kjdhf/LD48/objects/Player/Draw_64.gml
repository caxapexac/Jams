max_hp = max(max_hp, hp);

var offset_x = 5;
var offset_y = 5;

var layout_x = offset_x;
var layout_y = offset_y;

var hp_sprite = spr_gui_hp;
var hp_element_width = sprite_get_width(hp_sprite);
//var hp_element_height = sprite_get_height(hp_sprite);

var hp_by_element = sprite_get_number(hp_sprite) - 1;
var hp_elements = ceil(max_hp / hp_by_element);
for(var i = 0; i < hp_elements; i++) {
	var local_hp = clamp(hp - i * hp_by_element, 0, hp_by_element);
	var frame_index = hp_by_element - local_hp;
	draw_sprite(hp_sprite, frame_index, layout_x, layout_y);
	layout_x += hp_element_width + offset_x;	
}