// Blinn vertex shader
varying vec3 n, l, h;
uniform vec3 EyePos, LightPos;

void main()
{
vec3 p = vec3(gl_ModelViewMatrix * gl_Vertex);
n = gl_NormalMatrix * gl_Normal;
l = LightPos - p;
h = l + (EyePos - p);
gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
}