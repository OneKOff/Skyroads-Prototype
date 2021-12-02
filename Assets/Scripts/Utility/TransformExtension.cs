using UnityEngine;

public static class TransformExtension
{
    // Set position by axis
    public static void SetPosX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
    public static void SetPosZ(this Transform transform, float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    // Set rotation by axis
    public static void SetRotZ(this Transform transform, float z)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, z);
    }
}

