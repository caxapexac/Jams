function particles_update() {
    if (particles_destructor_enabled) {
        particles_destructor_enabled = false;
        for(var k = 0; k < particles_destructor_count; k++) {
            for (var i = 0; i < particles_max_count; i++) {
                 if (particles_arr_free[i] == true) {
                     create_particle(i);
                     break;
                 }
            }
        }
    }
    
    if (particles_emitter_enabled) {
        if (random(1) < particles_emitter_chance) {
            for (var i = 0; i < particles_max_count; i++) {
                 if (particles_arr_free[i] == true) {
                     create_particle(i);
                     break;
                 }
            }
        }
    }
    
    for (var i = 0; i < particles_max_count; i++) {
        if (particles_arr_free[i] == false) {
            tick_particle(i);
        }
    }
    
    frames_now += 1;
}

function create_particle(free_index) {
    particles_arr_free[free_index] = false;
    
    particles_arr_x[free_index] = x + random_range(-particles_displacement_x, particles_displacement_x);
    particles_arr_y[free_index] = y + random_range(-particles_displacement_y, particles_displacement_y);
    
    particles_arr_lifetime[free_index] = random_range(particles_lifetime_min, particles_lifetime_max);
    particles_arr_lifetime_start[free_index] = frames_now;
    
    particles_arr_subsprite_left[free_index] = random_range(0, particles_subsprite_width - particles_subsprite_width * particles_subsprite_scale - 1);
    particles_arr_subsprite_top[free_index] = random_range(0, particles_subsprite_height - particles_subsprite_height * particles_subsprite_scale - 1);
    particles_arr_subsprite_width[free_index] = particles_subsprite_width * particles_subsprite_scale;
    particles_arr_subsprite_height[free_index] = particles_subsprite_height * particles_subsprite_scale;
    
    particles_arr_speed_start_x[free_index] = random_range(particles_speed_start_min_x, particles_speed_start_max_x)
    particles_arr_speed_start_y[free_index] = random_range(particles_speed_start_min_x, particles_speed_start_max_x)
    particles_arr_speed_end_x[free_index] = random_range(particles_speed_end_min_x, particles_speed_end_max_x)
    particles_arr_speed_end_y[free_index] = random_range(particles_speed_end_min_y, particles_speed_end_max_y)
    particles_arr_scale_start[free_index] = random_range(particles_scale_start_min, particles_scale_start_max)
    particles_arr_scale_end[free_index] = random_range(particles_scale_end_min, particles_scale_end_max)
}

function tick_particle(free_index) {
    var current_lifetime = frames_now - particles_arr_lifetime_start[free_index];
    var t = current_lifetime / particles_arr_lifetime[free_index];
    if (t >= 1) {
        particles_arr_free[free_index] = true;
    }
    else {
        particles_arr_x[free_index] += lerp(particles_arr_speed_start_x[free_index], particles_arr_speed_end_x[free_index], t);
        particles_arr_y[free_index] += lerp(particles_arr_speed_start_y[free_index], particles_arr_speed_end_y[free_index], t);
        particles_arr_y[free_index] += lerp(particles_gravity_start, particles_gravity_end, t) * current_lifetime;
        
        var particle_scale = lerp(particles_arr_scale_start[free_index], particles_arr_scale_end[free_index], t)
        
        draw_sprite_part_ext(
            particles_sprite_id,
            0,
            particles_arr_subsprite_left[free_index],
            particles_arr_subsprite_top[free_index],
            particles_arr_subsprite_width[free_index],
            particles_arr_subsprite_height[free_index],
            particles_arr_x[free_index],
            particles_arr_y[free_index],
            particle_scale,
            particle_scale,
            c_white,
            1 - t
        );
    }
}

function lerp(a, b, t) {
    return a + (b - a) * t;
}