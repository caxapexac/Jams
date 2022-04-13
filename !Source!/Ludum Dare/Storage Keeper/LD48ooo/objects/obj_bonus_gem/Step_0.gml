default_motion(speed_x, speed_y, 1);
speed_y = clamp(speed_y + 2, -8, 8);
speed_x *= 0.98;