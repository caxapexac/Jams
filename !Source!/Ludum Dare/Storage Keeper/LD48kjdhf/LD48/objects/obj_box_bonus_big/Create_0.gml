event_inherited();

function on_hit(_vx, _vy, _damage) {
	if(content != noone) {
		instance_activate_object(content);
		content.x = x;
		content.y = y;
		content.speed_x = -sign(_vx) * 3;
		content.speed_y = -sign(_vy) * 3;
		with(content) {
			resolv_collision(_vx, _vy);
		}
		content = noone;
		sprite_index = spr_c1_box_dark;
	}
}