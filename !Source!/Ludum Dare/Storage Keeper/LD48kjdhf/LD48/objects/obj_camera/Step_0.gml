if folow {
	x = 0;
	y += (folow.y - y) * 0.2;
}


camera_set_view_mat(camera, matrix_build_lookat(x, y, -10, x, y, 0, 0, 1, 0));