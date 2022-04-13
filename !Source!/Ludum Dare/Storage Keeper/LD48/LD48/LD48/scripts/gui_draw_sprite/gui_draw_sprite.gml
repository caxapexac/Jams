function gui_draw_sprite(_sprite, _x, _y){
	var offset_x = sprite_get_xoffset(_sprite);
	var offset_y = sprite_get_yoffset(_sprite);
	draw_sprite(_sprite, 0, _x + offset_x, _y + offset_y);
}