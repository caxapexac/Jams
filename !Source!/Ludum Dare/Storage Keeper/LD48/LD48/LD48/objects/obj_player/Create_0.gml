enum character_state {
	idle,
	run,
	sit,
	jump_up,
	jump_down,
	die,
	stun
}

if(!instance_exists(Player)) {
	instance_create_layer(0, 0, layer, Player);
}

immortality_timer = 0;
stun_timer = 0;
u_invert = shader_get_uniform(shr_damaged, "u_invert");
is_sitting = false;

emitter_leg = instance_create_layer(x, y, layer, obj_fx_leg_smoke);

function apply_damage(_vx, _vy, _stan, _immortality, _damage) {
	speed_x += _vx;
	speed_y += _vy;
	if(immortality_timer <= 0) {
		stun_timer = _stan;
		immortality_timer = _immortality;
		Player.hp -= _damage;
	}
}

function square_cast_xy(_x, _y) {
	return square_cast(
        _x - x + collider_x1,
        _y - y + collider_y1,
        _x - x + collider_x2,
        _y - y + collider_y2
    );
}

function square_cast(_x1, _y1, _x2, _y2) {
	ds_list_clear(global.buffer_collision_list);
	collision_rectangle_list(_x1, _y1, _x2, _y2, obj_solid, true, true, global.buffer_collision_list, false)
	var count = ds_list_size(global.buffer_collision_list);
	for(var i = 0; i < count; i++) {
		var instance = global.buffer_collision_list[|i];
		if(instance.realy_solid)
			return instance;
	}
	return noone;
}

function can_grab(_obj) {
    if (_obj == noone) return false;
    if (instance_exists(holded)) return false;
    return true;
    mask_index = spr_player_collision_hold
    var collided = place_meeting(x, y, obj_solid)
    update_collision_mask();
    return collided;
    //if (square_cast(collider_x1, collider_y1 - _obj.sprite_height, collider_x2, collider_y2)) return false;
    //return true;
}

function grab(_obj) {
	holded = _obj;
	holded.realy_solid = false;
	holded.grabbed = true;
    holded.mask_index = spr_empty;
}

function ungrab() {
	
	
	
    //holded.mask_index 
    // todo
}

function throw_grabbed(_vx, _vy) {
	var tile_y = floor(y / 32) - !is_sitting;
	
	holded.mask_index = spr_thrown;
	holded_mask_index = noone;
	holded.grabbed = false;
	holded.thrown = true;
	
	holded.x = x;
	holded.y = tile_y * 32;
	holded.speed_y = _vy;
	holded.speed_x = _vx;
	holded = noone;
	update_collision_mask()
}

function get_nearest_vertical_grabbable(_down, _debug) {
    // squarecast
    ds_list_clear(global.buffer_collision_list);
	var drx1 = bbox_left;
	var dry1 = bbox_bottom;
	var drx2 = bbox_right;
	var dry2 = bbox_bottom + hand_vertical_length;
	collision_rectangle_list(drx1, dry1, drx2, dry2, obj_solid, false, true, global.buffer_collision_list, false)
	var count = ds_list_size(global.buffer_collision_list);
	var center = (bbox_left + bbox_right) * 0.5;
    
    // find nearest
	var detected = noone;
	for(var i = 0; i < count; i++)
	{
		var other_detected = global.buffer_collision_list[|i];
		if (!other_detected.grabable) continue;
		if (detected == noone) {
			detected = other_detected;
		}
		else if (abs(center - (other_detected.bbox_left + other_detected.bbox_right) * 0.5) < abs(center - (detected.bbox_left + detected.bbox_right) * 0.5))
		{
			detected = other_detected;
		}
	}
	
	if _debug {
		draw_rectangle(drx1, dry1, drx2, dry2, true);
		if detected {
			draw_rectangle(detected.bbox_left, detected.bbox_top, detected.bbox_right, detected.bbox_bottom, true)
		}
	}
	
	return detected;
}

function get_nearest_horizontal_grabbable(_right, _debug) {
    // squarecast
    ds_list_clear(global.buffer_collision_list);
	var drx1;
	var dry1 = bbox_top;
	var drx2;
	var dry2 = bbox_bottom;
	if _right {
		drx1 = bbox_right;
		drx2 = bbox_right + hand_horizontal_length;
	} else {
		drx1 = bbox_left;
		drx2 = bbox_left - hand_horizontal_length;
	}
	collision_rectangle_list(drx1, dry1, drx2, dry2, obj_solid, false, true, global.buffer_collision_list, false)
	var count = ds_list_size(global.buffer_collision_list);
	var center = (bbox_top + bbox_bottom) * 0.5;
    
    // find nearest
	var detected = noone;
	for(var i = 0; i < count; i++)
	{
		var other_detected = global.buffer_collision_list[|i];
		if (!other_detected.grabable) continue;
		if (detected == noone) detected = other_detected;
		else if (abs(center - (other_detected.bbox_top + other_detected.bbox_bottom) * 0.5) < abs(center - (detected.bbox_top + detected.bbox_bottom) * 0.5))
		{
			detected = other_detected;
		}
	}
	
	if _debug {
		draw_rectangle(drx1, dry1, drx2, dry2, true);
		if detected {
			draw_rectangle(detected.bbox_left, detected.bbox_top, detected.bbox_right, detected.bbox_bottom, true)
		}
	}
	
	return detected;
}

function update_collision_mask() {
	if (is_sitting) {
		if (instance_exists(holded)) {
	        mask_index = spr_player_collision_hold;
	    }
	    else {
	        mask_index = spr_player_collision;
	    }
	} else {
	    if (instance_exists(holded)) {
	        mask_index = spr_player_collision_hold;
	    }
	    else {
	        mask_index = spr_player_collision;
	    }
	}
}

function get_camera_target_y() {
	return y - (camera_get_view_height(view_camera[0]) / 2);
}

function set_camera_y(_y) {
	_y = clamp(_y, 0, room_height - camera_get_view_height(view_camera[0]));
	camera_set_view_pos(view_camera[0], 0, _y);
}

camera_y = get_camera_target_y();
set_camera_y(camera_y);