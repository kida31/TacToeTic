shader_type canvas_item;

uniform float flashSpeed = 1.0;
uniform bool isFlashing = false;

void fragment() {
	if (isFlashing) {
		COLOR.a = mix(0.5, 1.0, (sin(TIME * flashSpeed) + 1.0) * .5);
	} else {
		COLOR.a = 1.0;
	}
}