using UnityEngine;
using UnityEngine.Rendering;

public class LensDistortionTrigger : MonoBehaviour
{
    private Volume volume;

    void Start()
    {
        volume = GetComponent<Volume>();
        volume.weight = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            volume.weight = 1f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            volume.weight = 0f;
        }
    }
}