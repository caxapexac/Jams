camera = camera_create();

var width = 
camera_set_view_mat(camera, matrix_build_lookat(x, y, -10, x, y, 0, 0, 1, 0));
camera_set_proj_mat(camera, matrix_build_projection_ortho(640, 480, 1, 10000));

view_camera[0] = camera;
folow = obj_player;