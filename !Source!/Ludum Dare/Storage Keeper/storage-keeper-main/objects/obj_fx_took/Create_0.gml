
particles_init(spr_particle_took, 1, 6)
particles_init_param_destructor(6)
var rad = 12
particles_init_param_displacement(-rad, rad, -32 - rad, -32 + rad)
particles_init_param_gravity(0.1, 0.1)
particles_init_param_lifetime(0.2, 0.5)
particles_init_param_scale(0.5, 1, 0, 0.5)
var s = 1.5;
particles_init_param_speed(-s, s, -s, s, -s, s, -s, s)
particles_emit()

alarm[0] = 3 * room_speed