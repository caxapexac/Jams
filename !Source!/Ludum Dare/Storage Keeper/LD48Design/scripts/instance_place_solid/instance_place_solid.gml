function instance_place_solid(x, y, obj) {
	ds_list_clear(global.buffer_collision_list);
	instance_place_list(x, y, obj, global.buffer_collision_list, true);
	var count = ds_list_size(global.buffer_collision_list);
	
	for(var i = 0; i < count; i++) {
		if(global.buffer_collision_list[|i].realy_solid)
			return true;
	}
	
	return false;
}