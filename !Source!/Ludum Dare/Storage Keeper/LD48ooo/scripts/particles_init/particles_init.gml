function particles_init(sprite_source, subsprite_scale, max_count) {
    particles_sprite_id = sprite_source;
    particles_subsprite_height = sprite_get_height(sprite_source);
    particles_subsprite_width = sprite_get_width(sprite_source);
    particles_subsprite_scale = subsprite_scale;
    particles_max_count = max_count;
    frames_now = 0;
    
    if (!variable_instance_exists(id, "particles_destructor_count")) particles_destructor_count = 16;
    if (!variable_instance_exists(id, "particles_destructor_enabled")) particles_destructor_enabled = false;
    if (!variable_instance_exists(id, "particles_displacement_x")) particles_displacement_x = 0;
    if (!variable_instance_exists(id, "particles_displacement_y")) particles_displacement_y = 10;
    if (!variable_instance_exists(id, "particles_emitter_chance")) particles_emitter_chance = 0.2;
    if (!variable_instance_exists(id, "particles_emitter_count")) particles_emitter_enabled = false;
    if (!variable_instance_exists(id, "particles_gravity_end")) particles_gravity_end = 0;
    if (!variable_instance_exists(id, "particles_gravity_start")) particles_gravity_start = 0.2;
    if (!variable_instance_exists(id, "particles_lifetime_max")) particles_lifetime_max = 2;
    if (!variable_instance_exists(id, "particles_lifetime_min")) particles_lifetime_min = 1;
    if (!variable_instance_exists(id, "particles_scale_end_min")) particles_scale_end_min = 0;
    if (!variable_instance_exists(id, "particles_scale_end_max")) particles_scale_end_max = 0.5;
    if (!variable_instance_exists(id, "particles_scale_start_max")) particles_scale_start_max = 1.5;
    if (!variable_instance_exists(id, "particles_scale_start_min")) particles_scale_start_min = 0.5;
    if (!variable_instance_exists(id, "particles_speed_end_max_x")) particles_speed_end_max_x = 0.5;
    if (!variable_instance_exists(id, "particles_speed_end_max_y")) particles_speed_end_max_y = 0.5;
    if (!variable_instance_exists(id, "particles_speed_end_min_x")) particles_speed_end_min_x = -0.5;
    if (!variable_instance_exists(id, "particles_speed_end_min_y")) particles_speed_end_min_y = -0.5;
    if (!variable_instance_exists(id, "particles_speed_start_max_x")) particles_speed_start_max_x = 2;
    if (!variable_instance_exists(id, "particles_speed_start_max_y")) particles_speed_start_max_y = 2;
    if (!variable_instance_exists(id, "particles_speed_start_min_x")) particles_speed_start_min_x = -2;
    if (!variable_instance_exists(id, "particles_speed_start_min_y")) particles_speed_start_min_y = -2;
    
    particles_arr_free = array_create(particles_max_count, true);
    particles_arr_lifetime = array_create(particles_max_count, 0);
    particles_arr_lifetime_start = array_create(particles_max_count, 0);
    
    particles_arr_subsprite_left = array_create(particles_max_count, 0);
    particles_arr_subsprite_top = array_create(particles_max_count, 0);
    particles_arr_subsprite_width = array_create(particles_max_count, 0);
    particles_arr_subsprite_height = array_create(particles_max_count, 0);
    
    particles_arr_x = array_create(particles_max_count, 0);
    particles_arr_y = array_create(particles_max_count, 0);
    particles_arr_speed_start_x = array_create(particles_max_count, 0);
    particles_arr_speed_start_y = array_create(particles_max_count, 0);
    particles_arr_speed_end_x = array_create(particles_max_count, 0);
    particles_arr_speed_end_y = array_create(particles_max_count, 0);
    particles_arr_scale_start = array_create(particles_max_count, 0);
    particles_arr_scale_end = array_create(particles_max_count, 0);
}