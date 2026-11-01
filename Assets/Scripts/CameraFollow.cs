using UnityEngine;

// Smoothly follows the player from behind and above.
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 6f, -9f);
    public float smoothing = 0.1f;    // how much of the gap to close each frame

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, smoothing);
        transform.LookAt(target.position + Vector3.forward * 4f);
    }
}
