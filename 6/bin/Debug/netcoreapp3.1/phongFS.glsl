// Phong fragment shader
varying vec3 n, l, v;
uniform float specPower;

void main()
{
vec4 diffColor = vec4(0.75, 0.0, 0.0, 1.0);
vec4 specColor = vec4(0.75, 0.75, 0.0, 1.0);
vec3 n2 = normalize(n);
vec3 l2 = normalize(l);
vec3 r2 = reflect(-l2, n2);
vec4 diff = diffColor * max(dot(n2, l2), 0);
vec4 spec = specColor * pow(max(dot(normalize(v), r2), 0), specPower);
gl_FragColor = diff + spec;
}