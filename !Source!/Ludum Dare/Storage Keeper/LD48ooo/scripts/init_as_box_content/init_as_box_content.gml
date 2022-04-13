function init_as_box_content(){
	var place_x = (bbox_left + bbox_right) * 0.5;
	var place_y = (bbox_top + bbox_bottom) * 0.5;

	var box = collision_point(place_x, place_y, obj_box, true, true);
	if(box != noone) {
		box.content = self;
		instance_deactivate_object(self);
	}
}