using UnityEngine;

public class DayNignt : MonoBehaviour
{
    [Header("Rotation Settings")] public float rotationSpeed = 10f;

    void Update()
    {
        
        transform.Rotate(Vector3.right * (rotationSpeed * Time.deltaTime));
    }
}
