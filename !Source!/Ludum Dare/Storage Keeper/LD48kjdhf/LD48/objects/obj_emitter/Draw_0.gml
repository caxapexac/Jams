if (emitter_enabled && random(1) < parts_chance) {
    var free_index = -1;
    for (var i = 0; i < parts_count; i++) {
         if (parts_free[i] == true) {
             free_index = i;
             break;
         }
    }
    
    if (free_index != -1) {
        parts_free[free_index] = false;
        parts_x[free_index] = x + random_range(-parts_start_range, parts_start_range);
        parts_y[free_index] = y + random_range(-parts_start_range, parts_start_range);
        parts_speed_x[free_index] = random_range(-parts_start_speed_x, parts_start_speed_x);
        parts_speed_y[free_index] = random_range(-parts_start_speed_y, parts_start_speed_y);
        parts_subsprite_left[free_index] = random_range(0, subsprite_width - subsprite_width * parts_size - 1);
        parts_subsprite_top[free_index] = random_range(0, subsprite_height - subsprite_height * parts_size - 1);
        parts_subsprite_width[free_index] = subsprite_width * parts_size;
        parts_subsprite_height[free_index] = subsprite_width * parts_size;
        parts_lifetime[free_index] = frames_now;
    }
}

for (var i = 0; i < parts_count; i++) {
    if (parts_free[i] == true) continue;
    parts_speed_y[i] = parts_speed_y[i] + gravity_speed;
    
    parts_x[i] = parts_x[i] + parts_speed_x[i];
    parts_y[i] = parts_y[i] + parts_speed_y[i];
    
    var lifetime = (frames_now - parts_lifetime[i]) / frames_lifetime;
    if (lifetime > 1) {
         parts_free[i] = true;
    }
    else {
        draw_sprite_part_ext(
            sprite_source,
            0,
            parts_subsprite_left[i],
            parts_subsprite_top[i],
            parts_subsprite_width[i],
            parts_subsprite_height[i],
            parts_x[i],
            parts_y[i],
            parts_scale,
            parts_scale,
            c_white,
            1 - lifetime
        );
    }
}

frames_now += 1;
