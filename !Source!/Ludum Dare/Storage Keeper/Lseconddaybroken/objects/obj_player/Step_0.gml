// View side
if (Input.right) {
    image_xscale = abs(image_xscale);
    render_look_right = true;
}
else if (Input.left) {
    image_xscale = -abs(image_xscale);
    render_look_right = false;
}

// Self collider
update_collision_mask();

// Grab happens
if (Input.grab) {
    // Find nearest
    var nearest = noone;
    if (Input.down) {
        nearest = get_nearest_vertical_grabbable(true, false);
    }
    else {
        if (render_look_right) {
           nearest = get_nearest_horizontal_grabbable(true, false);
        }
        else {
           nearest = get_nearest_horizontal_grabbable(false, false);
        }
    }
    // Grab nearest
    if (can_grab(nearest)) {
        grab(nearest);
    }
}

// Crutch code sorry
update_collision_mask();

// Input

var input_x = (Input.right - Input.left) * run_speed;
var input_y = (Input.jump) * jump_force;
if (holded) {
    input_x *= run_holded_multipiler;
    input_y *= jump_holded_multipiler;
}


// Speed update
speed_x = input_x;

if (abs(speed_y) < 1 && place_meeting(x, y + 1, obj_solid)) {
    if (state == character_state.idle) speed_y += input_y;
    if (state == character_state.run) speed_y += input_y;
}
if (speed_y < 10) speed_y += gravity_speed;
if (state == character_state.die) {
    speed_x = 0;
    speed_y = 0;
}

// Check self collision
var bug_collisions = place_meeting(x, y, obj_solid);
if (bug_collisions != false) {
    throw ("");   
}


// Speed collide

// Horizontal
var iterations_x = floor(abs(speed_x) / collision_step);
if (iterations_x != 0) {
    var final_i = 0
    for (var i = 0; i <= iterations_x; i++) {
        var current_x = x + speed_x * i / iterations_x;
        var collisions = place_meeting(current_x, y, obj_solid);
        if (collisions == false) {
            final_i = i;
        }
        else {
            break;
        }
    }
    speed_x = speed_x * final_i / iterations_x;
    x = x + speed_x;
}

// Vertical
var iterations_y = floor(abs(speed_y) / collision_step);
if (iterations_y != 0) {
    var final_i = 0
    for (var i = 0; i <= iterations_y; i++) {
        var current_y = y + speed_y * i / iterations_y;
        var collisions = place_meeting(x, current_y, obj_solid);
        if (collisions == false) {
            final_i = i;
        }
        else {
            break;
        }
    }
    speed_y = speed_y * final_i / iterations_y;
    y = y + speed_y;
}


/*
var final_i = 0
var max_delta = max(abs(speed_x), abs(speed_y));
var iterations = floor(max_delta / collision_step);
if (iterations != 0) {
    
    // Diagonal
    for (var i = 0; i <= iterations; i++) {
        var current_x = x + speed_x * i / iterations;
        var current_y = y + speed_y * i / iterations;
        //var collisions = square_cast_xy(current_x, current_y);
        var collisions = place_meeting(current_x, current_y, obj_solid);
        if (collisions == false) {
            final_i = i;
        }
        else {
            break;
        }
    }

    speed_x = speed_x * final_i / iterations;
    speed_y = speed_y * final_i / iterations;
    
    x += speed_x;
    y += speed_y;
}*/

// Render state
if speed_y != 0 {
	if (speed_y > 0) {
		state = character_state.jump_up;
	} else {
		state = character_state.jump_down;
	}
} else {
	if (speed_x != 0) {
		state = character_state.run;
	} else {
		state = character_state.idle;
	}
}

// Render happens
switch(state) {
	case character_state.idle:
		if holded == noone {
			sprite_index = spr_player_idle;
		} else {
			sprite_index = spr_player_hold;
		}
        
	break;
    
	case character_state.run:
		if holded == noone {
			sprite_index = spr_player_run
		} else {
			sprite_index = spr_player_run_hold
		}
        
	break;
    
	case character_state.sit:
		if holded == noone {
			sprite_index = spr_player_sit
		} else {
			sprite_index = spr_player_sit_hold
		}
        
	break;
    
	case character_state.jump_up:
		if holded == noone {
			sprite_index = spr_player_jump_up
		} else {
			sprite_index = spr_player_jump_hold_up
		}
        
	break;
    
	case character_state.jump_down:
		if holded == noone {
			sprite_index = spr_player_jump_down
		} else {
			sprite_index = spr_player_jump_hold_down
		}
        
	break;
    
    case character_state.die:
		if holded == noone {
			sprite_index = spr_player_jump_down
		} else {
			sprite_index = spr_player_jump_hold_down
		}
        
	break;
}

// Change grabbable position
if holded != noone
{
	holded.x = x;
	holded.y = y - sprite_height;
}

// Camera position
camera_y += (y - (view_hport[0] / 4) - camera_y) * 0.05;
camera_set_view_pos(view_camera[0], 0, camera_y);
