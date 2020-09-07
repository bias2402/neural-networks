Shader "Vertex Colors" {
	SubShader {
		BindChannels {
			Bind "vertex", vertex
			Bind "color", color
		}
		Pass{}
	}
}