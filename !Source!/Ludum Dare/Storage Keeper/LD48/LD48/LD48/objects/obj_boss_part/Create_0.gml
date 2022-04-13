function hit_player() {
	obj_player.apply_damage(0, 5, 20, 25, 1);
}

function on_hit(_vx, _vy, _damage) {
	if(!immortal)
		obj_boss.on_hit(_vx, _vy, _damage);
}