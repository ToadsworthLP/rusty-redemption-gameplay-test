shader_type canvas_item;

uniform float displacementStrength;
uniform float horizontalAnimationSpeed;
uniform float verticalAnimationSpeed;
uniform sampler2D displacementMap : filter_nearest;

void fragment() {
	vec2 inUV = vec2(
		UV.x,
		mod(UV.y + TIME * verticalAnimationSpeed, 1)
	);
	
	vec2 displUV = vec2(
		UV.x + ((texture(displacementMap, inUV).r - 0.5f) * 2.0f) * displacementStrength * sin(TIME * horizontalAnimationSpeed),
		UV.y
	);
	
	COLOR *= texture(TEXTURE, displUV);
}
