enum character_state {
	idle,
	hold,
	run_left,
	run_right,
	
	jump,
	fall
}

global.buffer_collision_list = ds_list_create();