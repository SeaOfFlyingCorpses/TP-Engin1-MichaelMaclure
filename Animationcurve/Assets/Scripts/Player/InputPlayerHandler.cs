using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputPlayerHandler : MonoBehaviour
    {
        [Header("Movement")]
        public Vector2 moveInput;

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }
       
    }
}
