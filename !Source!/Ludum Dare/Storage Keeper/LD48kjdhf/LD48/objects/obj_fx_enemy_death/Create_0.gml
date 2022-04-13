particles_init(spr_particle_enemy_death, 0.2, 32);

particles_init_param_destructor(32)
var rad = 4
particles_init_param_displacement(-rad, rad, -32 - rad, -32 + rad)
particles_init_param_gravity(0.1, 0.2)
particles_init_param_lifetime(0.3, 0.6)
particles_init_param_scale(1, 3, 0, 1)
var s = 3;
particles_init_param_speed(-s, s, -s, s, -s, s, -s, s)
particles_emit()

alarm[0] = 3 * room_speed;