//
// Simple passthrough fragment shader
//
varying vec2 v_vTexcoord;
varying vec4 v_vColour;

uniform bool u_invert;

void main()
{
    gl_FragColor = v_vColour * texture2D( gm_BaseTexture, v_vTexcoord );
	
	if(u_invert) {
		gl_FragColor = vec4(vec3(1) - gl_FragColor.rgb, gl_FragColor.a);
	}
}
