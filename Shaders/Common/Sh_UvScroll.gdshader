shader_type canvas_item;
uniform vec2 scroll;
uniform vec2 zoom = vec2(1, 1);

void vertex() {
	UV += scroll * TIME;
	UV *= zoom;
}