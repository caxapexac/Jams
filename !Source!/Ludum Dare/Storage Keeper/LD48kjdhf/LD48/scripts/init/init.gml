enum character_state {
	idle,
	run,
	sit,
	jump_up,
	jump_down,
	die,
	stun
}

global.buffer_collision_list = ds_list_create();