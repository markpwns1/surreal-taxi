inline float lerp_inv(float a, float b, float v) {
    return (v - a) / (b - a);
}

inline float clamp(float mn, float mx, float v) {
    return min(mx, max(v, mn));
}

inline float lerp_inv_01(float a, float b, float v) {
    return clamp(0, 1, lerp_inv(a, b, v));
}

inline float lerp_convert(float a, float b, float c, float d, float v) {
    return lerp(a, b, lerp_inv_01(c, d, v));
}

inline float3 plane_project(float3 vec, float3 normal)
{
    return vec - normal * (dot(vec, normal) / dot(normal, normal));
}
