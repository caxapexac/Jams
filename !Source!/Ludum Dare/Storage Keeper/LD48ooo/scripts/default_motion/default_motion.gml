enum default_motion_collision {
	nope = 0,
	vertical = 1,
	horizontal = 2
}

function default_motion(_dx, _dy, _steps){
	// Horizontal
	var collision_type = default_motion_collision.nope;
	var iterations_x = floor(abs(_dx) / _steps);
	if (iterations_x != 0) {
	    var final_i = 0
	    for (var i = 0; i <= iterations_x; i++) {
	        var current_x = x + _dx * i / iterations_x;
	        var collisions = place_meeting(current_x, y, obj_solid);
	        if (collisions == false) {
	            final_i = i;
	        } else {
				collision_type |= default_motion_collision.horizontal;
	            break;
	        }
	    }
	    _dx = _dx * final_i / iterations_x;
	    x = x + _dx;
	}

	// Vertical
	var iterations_y = floor(abs(_dy) / _steps);
	if (iterations_y != 0) {
	    var final_i = 0
	    for (var i = 0; i <= iterations_y; i++) {
	        var current_y = y + _dy * i / iterations_y;
	        var collisions = place_meeting(x, current_y, obj_solid);
	        if (collisions == false) {
	            final_i = i;
	        } else {
				collision_type |= default_motion_collision.vertical;
	            break;
	        }
	    }
	    _dy = _dy * final_i / iterations_y;
	    y = y + _dy;
	}
	return collision_type;
}