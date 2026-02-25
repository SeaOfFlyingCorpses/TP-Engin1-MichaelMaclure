using UnityEngine;

namespace Checker
{
    public class AnimationHandler : MonoBehaviour
    {
        [Header("References")]
        public PlayerController playerController;
        public Animator animator;

        private void Awake()
        {
            if (playerController == null)
                playerController = GetComponent<PlayerController>();

            if (animator == null)
                animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (playerController == null || animator == null) return;

            Vector2 moveInput = playerController.GetMoveInput();
            float speed = new Vector3(moveInput.x, 0, moveInput.y).magnitude;

            animator.SetFloat("Speed", speed);
        }
        
        public void PlayDash()
        {
            if (animator == null) return;
            animator.SetTrigger("Dash");
        }
    }
}