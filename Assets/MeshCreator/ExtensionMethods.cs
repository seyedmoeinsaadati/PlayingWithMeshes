using UnityEngine;

public static class ExtensionMethods
{
    /// <summary>
    /// Rotate 2D point (XY)
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="angle"></param>
    /// <param name="clockwise"></param>
    /// <returns></returns>
    public static Vector3 RotateXY(this Vector3 vector, float angle, bool clockwise) //angle in radians
    {
        if (clockwise)
        {
            angle = 2 * Mathf.PI - angle;
        }

        float xVal = vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle);
        float yVal = vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle);
        return new Vector3(xVal, yVal, 0);
    }

    /// <summary>
    /// Rotate 2D point (XZ)
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="angle"></param>
    /// <param name="clockwise"></param>
    /// <returns></returns>
    public static Vector3 RotateXZ(this Vector3 vector, float angle, bool clockwise) //angle in radians
    {
        if (clockwise)
        {
            angle = 2 * Mathf.PI - angle;
        }

        float xVal = vector.x * Mathf.Cos(angle) - vector.z * Mathf.Sin(angle);
        float yVal = vector.x * Mathf.Sin(angle) + vector.z * Mathf.Cos(angle);
        return new Vector3(xVal, 0, yVal);
    }

    /// <summary>
    /// Rotate 2D point (YZ)
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="angle"></param>
    /// <param name="clockwise"></param>
    /// <returns></returns>
    public static Vector3 RotateYZ(this Vector3 vector, float angle, bool clockwise) // angle in radians
    {
        if (clockwise)
        {
            angle = 2 * Mathf.PI - angle;
        }

        float xVal = vector.y * Mathf.Cos(angle) - vector.z * Mathf.Sin(angle);
        float yVal = vector.y * Mathf.Sin(angle) + vector.z * Mathf.Cos(angle);
        return new Vector3(0, xVal, yVal);
    }
}