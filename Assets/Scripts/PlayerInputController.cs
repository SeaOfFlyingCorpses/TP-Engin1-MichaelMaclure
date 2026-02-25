using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Checker
{
    public class InputController : MonoBehaviour
    {
        public event Action<Vector2> OnMove;
        public event Action OnDash;
        public event Action<Vector2> OnScroll;

        private PlayerInputActions actions;

        private void Awake()
        {
            if (actions == null)
                actions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            if (actions == null)
                actions = new PlayerInputActions();

            
            actions.Player.Enable();
                actions.Camera.Enable();

            
            actions.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
            actions.Player.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);
            actions.Player.Dash.performed += _ => OnDash?.Invoke();

          
            
    
                actions.Camera.MouseWheel.performed += OnMouseWheelPerformed;
                actions.Camera.MouseWheel.canceled += OnMouseWheelPerformed;
            
        }

        private void OnDisable()
        {
            actions.Player.Move.performed -= ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
            actions.Player.Move.canceled -= ctx => OnMove?.Invoke(Vector2.zero);
            actions.Player.Dash.performed -= _ => OnDash?.Invoke();
            
           
                actions.Camera.MouseWheel.performed -= OnMouseWheelPerformed;
                actions.Camera.MouseWheel.canceled -= OnMouseWheelPerformed;
                actions.Camera.Disable();
           
            actions.Player.Disable();
        }

        private void OnMouseWheelPerformed(InputAction.CallbackContext ctx)
        {
            Vector2 scrollValue = ctx.ReadValue<Vector2>();
            OnScroll?.Invoke(scrollValue);
        }
    }
}
