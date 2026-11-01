using UnityEngine;

// Smoothly follows the player from behind and above.
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 6f, -9f);
    public float followSharpness = 6f;   // higher = snappier (6 feels like the old 0.1 at 60 FPS)

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;

        // A fixed Lerp(…, 0.1f) closes 10% of the gap per FRAME, so it catches up faster
        // at high FPS. This version closes the same share of the gap per SECOND.
        float t = 1f - Mathf.Exp(-followSharpness * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desired, t);
        transform.LookAt(target.position + Vector3.forward * 4f);
    }
}
