up =  keyboard_check(ord("W"));
down = keyboard_check(ord("S"));
left = keyboard_check(ord("A"));
right = keyboard_check(ord("D"));

grab = mouse_check_button_pressed(mb_left);
grab |= keyboard_check_pressed(vk_shift);
grab |= keyboard_check_pressed(vk_enter);

jump = keyboard_check(ord(" "));



for(var i = 0, count = gamepad_get_device_count(); i < count; i++) {
	left |= gamepad_button_check(i, gp_padl);
	right |= gamepad_button_check(i, gp_padr);
	up |= gamepad_button_check(i, gp_padu);
	down |= gamepad_button_check(i, gp_padd);
	jump |= gamepad_button_check_pressed(i, gp_face1);
	grab |= gamepad_button_check_pressed(i, gp_face3);
}