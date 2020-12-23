using UnityEngine;
/// <summary>
/// Central location for Vector Math operations
/// </summary>
public static class VectorMath
{
    /// <summary>
    /// Calculates a new Vector which is the rotation of
    /// the provided vector by the provided angle
    /// </summary>
    /// <param name="v0">Vector to Rotate</param>
    /// <param name="angle">Angle, in degrees, to rotate</param>
    /// <returns>Rotated Vector</returns>
    public static Vector3 RotateVector2D(Vector3 v0, float angle)
    {
        // 2D Vector Rotation Formula
        // x1 = x0*cos(a) - y0*sin(a)
        // y1 = x0*sin(a) + y0*cos(a)
        float x1 = v0.x * Mathf.Cos(angle) - v0.y * Mathf.Sin(angle);
        float y1 = v0.x * Mathf.Sin(angle) + v0.y * Mathf.Cos(angle);

        return new Vector3(x1, y1, v0.z);
    }

    /// <summary>
    /// Rotates a 2D Vector by 90 degrees
    /// </summary>
    /// <param name="v">Vector to rotate</param>
    /// <returns>Vector rotated by 90 degrees</returns>
    public static Vector2 RotateBy90(Vector2 v)
    {
        Vector2 arcMovement = new Vector2(v.y, -1 * v.x);
        return arcMovement;
    }
}
