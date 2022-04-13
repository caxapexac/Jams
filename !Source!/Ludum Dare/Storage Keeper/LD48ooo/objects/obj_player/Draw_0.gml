if(immortality_timer > 0) {
	shader_set(shr_damaged);
	shader_set_uniform_i(u_invert, (immortality_timer / 5) % 2);
	draw_self();
	shader_reset();
} else {
	draw_self();
}




// DEBUG
//get_nearest_vertical_grabbable(true, true)
//get_nearest_horizontal_grabbable(true, true)
//get_nearest_horizontal_grabbable(false, true)

/*if holded != noone
{
	with holded
		draw_self();
}*/

//draw_set_color(c_white)
//draw_rectangle(rx1, ry1, rx2, ry2, true);
//draw_rectangle(bbox_left, bbox_top, bbox_right, bbox_bottom, true);
//draw_set_color(c_green)

