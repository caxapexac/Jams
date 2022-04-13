if (dropped) {
	default_motion(speed_x, speed_y, 1);
	speed_y = clamp(speed_y + 2, -8, 8);
	speed_x *= 0.98;
} else {
	y = ystart + 3 * cos(current_time / 100 + xstart);
}