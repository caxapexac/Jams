var collision = default_motion(motion_speed, 5, 1);
if (default_motion_collision.horizontal & collision) {
	motion_speed = -motion_speed;
}