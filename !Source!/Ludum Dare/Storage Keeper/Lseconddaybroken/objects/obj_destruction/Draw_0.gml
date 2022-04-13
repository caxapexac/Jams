for (var i = 0; i < parts_count; i++) {
    
    parts_speed_y[i] = parts_speed_y[i] + gravity_speed;
    
    parts_x[i] = parts_x[i] + parts_speed_x[i];
    parts_y[i] = parts_y[i] + parts_speed_y[i];
    
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
        (1 - frames_now / frames_lifetime) * parts_random_scale[i]
    );
}

frames_now += 1;
if (frames_now >= frames_lifetime) {
    instance_destroy();
}