using UnityEngine;

// Spins an object so obstacles and pickups look alive.
public class Rotator : MonoBehaviour
{
    public Vector3 degreesPerFrame = new Vector3(0f, 3f, 0f);

    void Update()
    {
        transform.Rotate(degreesPerFrame);
    }
}
