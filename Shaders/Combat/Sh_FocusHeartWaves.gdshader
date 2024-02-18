shader_type canvas_item;

uniform float animationOffset;
uniform float verticalAnimationSpeed;
uniform float verticalAnimationStrength;
uniform float horizontalAnimationSpeed;
uniform float horizontalAnimationStrength;

void vertex() {
	VERTEX = vec2(
		VERTEX.x + cos((TIME + animationOffset) * horizontalAnimationSpeed) * horizontalAnimationStrength, 
		VERTEX.y + sin((TIME + animationOffset) * verticalAnimationSpeed) * verticalAnimationStrength
	);
}
