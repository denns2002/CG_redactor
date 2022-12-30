// Blinn fragment shader
varying vec3 n, l, h;
uniform float specPower;

void main()
{
vec4 diffColor = vec4(0.75, 0.0, 0.0, 1.0);
vec4 specColor = vec4(0.75, 0.75, 0.0, 1.0);
vec3 n2 = normalize(n);
vec4 diff = diffColor * max(dot(n2, normalize(l)), 0);
vec4 spec = specColor * pow(max(dot(n2, normalize(h)), 0), specPower);
gl_FragColor = diff + spec;
}