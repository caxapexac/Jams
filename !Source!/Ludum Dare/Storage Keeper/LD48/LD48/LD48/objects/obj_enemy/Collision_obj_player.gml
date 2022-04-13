var dx = other.x - x;
var dy = other.y - y;
var mag = sqrt(dx*dx + dy*dy);
var player_push_force = 3;
var player_stan = 24;
var player_immortality = 50;
var player_damage = 1;
other.apply_damage(player_push_force * dx/mag, player_push_force * dy/mag, player_stan, player_immortality, player_damage)