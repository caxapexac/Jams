dx = 0

run_speed = 5;

grounded = place_meeting(x, y + 1, obj_solid);

if grounded && Input.jump {
	dy = -15;
	grounded = false;
} else if !grounded {
	dy = clamp(dy + 1, -20, 20)
}

if Input.left
	dx -= run_speed;

if Input.right
	dx += run_speed;

if dx != 0 {
	is_look_right = dx > 0;
	fix = sign(dx);
	while(place_meeting(x + dx, y, obj_solid))
		dx -= fix;
	x = x + dx;
}

if dy != 0 {
	if place_meeting(x, y + dy, obj_solid) {
		fix = sign(dy);		
		while(instance_place_solid(x, y + dy, obj_solid))
			dy -= fix;
	}
	y = y + dy;
}

if Input.grab {
	if holded == noone {
		side_object = instance_place(x + 1, y, obj_solid)
		if side_object != noone
		{
			holded = side_object;
			holded.realy_solid = false;
			state = character_state.hold;
		}
	} else {
		holded.solid = true;
		if is_look_right
			holded.hspeed = 10;
		else
			holded.hspeed = -10;
		holded.thrown = true;
		holded = noone;
		state = character_state.idle;
	}
}

switch(state) {
	case character_state.idle:
		sprite_index = spr_player_idle_left;
	break;
	case character_state.hold:
		sprite_index = spr_player_hold_left;
	break;
}

if is_look_right
	image_xscale = 1;
else
	image_xscale = -1;

/*
switch state {
	case caracter_state.idle:
		if Input.right && !Input.left
			state = caracter_state.run_left;
		if !Input.right && Input.left
			state = caracter_state.run_right;
	break;
	case caracter_state.run_left:
			
	break;
	case caracter_state.jump:
	break;
	case caracter_state.fall:
	break;
}

switch state {
	case caracter_state.run_left:
		if !Input.left
			
		if speed_x != 0 && !place_meeting(x - speed_x, y, obj_box)
			x = x - speed_x;
		break;
	case caracter_state.run_right:
		if speed_x != 0 && !place_meeting(x + speed_x, y, obj_box)
			x = x + speed_x;
		break;
}
*/