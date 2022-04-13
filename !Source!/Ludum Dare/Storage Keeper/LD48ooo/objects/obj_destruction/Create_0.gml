frames_lifetime = room_speed * parts_lifetime;
frames_now = 0;
subsprite_height = sprite_get_height(sprite_source);
subsprite_width = sprite_get_width(sprite_source);

parts_x = array_create(parts_count, 0);
parts_y = array_create(parts_count, 0);
parts_speed_x = array_create(parts_count, 0);
parts_speed_y = array_create(parts_count, 0);
parts_subsprite_left = array_create(parts_count, 0);
parts_subsprite_top = array_create(parts_count, 0);
parts_subsprite_width = array_create(parts_count, 0);
parts_subsprite_height = array_create(parts_count, 0);

for (var i = 0; i < parts_count; i++) {
    parts_x[i] = x + random_range(-parts_start_range, parts_start_range);
    parts_y[i] = y + random_range(-parts_start_range, parts_start_range);
    parts_speed_x[i] = random_range(-parts_start_speed, parts_start_speed);
    parts_speed_y[i] = random_range(-parts_start_speed, parts_start_speed);
    parts_subsprite_left[i] = random_range(0, subsprite_width - subsprite_width * parts_size - 1);
    parts_subsprite_top[i] = random_range(0, subsprite_height - subsprite_height * parts_size - 1);
    parts_subsprite_width[i] = subsprite_width * parts_size;
    parts_subsprite_height[i] = subsprite_width * parts_size;
}
