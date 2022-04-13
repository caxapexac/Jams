event_inherited();

function on_hit(_vx, _vy, _damage) {
	if(!stanned && !immortal) {
		//var scale_x = image_xscale;
		//var scale_y = image_yscale;
		sprite_index = spr_boss_curse;
		//image_xscale = scale_x;
		//image_xscale = scale_y;
		stanned = true;
		alarm[1] = 100 * max(1, _damage);
	}
}


if(!moving) {
	alarm[0] = 10;
}