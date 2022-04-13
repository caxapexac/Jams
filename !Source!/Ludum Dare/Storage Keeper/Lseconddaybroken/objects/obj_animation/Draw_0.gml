draw_self();
animation_index += animation_speed;
image_index = animation_index;
if animation_index >= image_number
	instance_destroy();