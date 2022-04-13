function resolv_collision(_vx, _vy){
	if (place_meeting(x, y, obj_solid)) {
		do {
			x -= sign(_vx);
			y -= sign(_vy);
		} until(!place_meeting(x, y, obj_solid));
	}
}