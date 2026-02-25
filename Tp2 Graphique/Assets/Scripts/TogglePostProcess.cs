using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class TogglePostProcess : MonoBehaviour
{
    private Volume volume;

    void Start()
    {
        volume = GetComponent<Volume>();
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            volume.enabled = !volume.enabled;
        }
    }
}