using UnityEngine;

// Spins an object
public class Rotator : MonoBehaviour
{
    public Vector3 degreesPerSecond = new Vector3(0f, 180f, 0f);

    void Update()
    {
        transform.Rotate(degreesPerSecond * Time.deltaTime);
    }
}
