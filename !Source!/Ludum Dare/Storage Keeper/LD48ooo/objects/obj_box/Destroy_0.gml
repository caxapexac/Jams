//instance_create_layer(x, y, layer, obj_animation).set_sprite(spr_break_box);

var place_x = (bbox_left + bbox_right) * 0.5;
var place_y = (bbox_top + bbox_bottom) * 0.5;

instance_create_layer(place_x, place_y, layer, obj_destruction).sprite_source = sprite_index;

if(content != noone) {
	instance_activate_object(content);
	content.x = x;
	content.y = y;
}