if thrown {
	if (thrown && place_meeting(x, y, obj_solid)) {
		mask_index = base_mask_index;
		do {
			x -= sign(speed_x);
			y -= sign(speed_y);
		} until(!place_meeting(x, y, obj_solid));
		//instance_destroy();
	}
	other.on_hit(speed_x, speed_y, hit_damage);
	instance_destroy();
}