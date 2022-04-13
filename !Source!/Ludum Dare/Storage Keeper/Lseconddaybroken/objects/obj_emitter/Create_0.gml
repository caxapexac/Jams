frames_lifetime = room_speed * parts_lifetime;
frames_now = 0;
subsprite_height = sprite_get_height(sprite_source);
subsprite_width = sprite_get_width(sprite_source);

parts_free = array_create(parts_count, true);
parts_x = array_create(parts_count, 0);
parts_y = array_create(parts_count, 0);
parts_speed_x = array_create(parts_count, 0);
parts_speed_y = array_create(parts_count, 0);
parts_subsprite_left = array_create(parts_count, 0);
parts_subsprite_top = array_create(parts_count, 0);
parts_subsprite_width = array_create(parts_count, 0);
parts_subsprite_height = array_create(parts_count, 0);
parts_lifetime = array_create(parts_count, 0);
