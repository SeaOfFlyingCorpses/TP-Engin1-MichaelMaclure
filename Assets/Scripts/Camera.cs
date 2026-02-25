using UnityEngine;

namespace Checker
{
    public class Camera : MonoBehaviour
    {
        [Header("References")]
        public Transform target;                 
        public InputController inputController; 

        [Header("Follow Settings")]
        public Vector3 defaultOffset = new Vector3(0f, 7f, -12f);
        public float followSmoothTime = 0.15f;

        [Header("Scroll Settings")]
        public Vector3 topDownOffset = new Vector3(0f, 25f, -15f); 
        public float scrollSpeed = 10f;                            

        private Vector3 offset;
        private Vector3 currentVelocity;

        private bool isWhiteTurn = true;

        [Header("Rotation Settings")]
        public float rotationSmoothSpeed = 3f;
        public float followLerpSpeed = 4f;

        private void Awake()
        {
            offset = defaultOffset;

            if (inputController == null)
                inputController = FindObjectOfType<InputController>();
        }

        private void OnEnable()
        {
            if (inputController != null)
                inputController.OnScroll += HandleScroll;
        }

        private void OnDisable()
        {
            if (inputController != null)
                inputController.OnScroll -= HandleScroll;
        }

        private void LateUpdate()
        {
            FollowTarget();
            SmoothLookAtTarget();
        }

        private void FollowTarget()
        {
            Vector3 turnOffset = isWhiteTurn ? offset : new Vector3(-offset.x, offset.y, -offset.z);

            Vector3 desiredPosition = target.position + turnOffset;
            
            transform.position = Vector3.Lerp(
                transform.position,
                desiredPosition,
                1f - Mathf.Exp(-followLerpSpeed * Time.deltaTime)
            );
        }

        private void SmoothLookAtTarget()
        {
            Vector3 direction = target.position - transform.position;
            if (direction.sqrMagnitude < 0.001f) return;

            Quaternion desiredRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                desiredRotation,
                rotationSmoothSpeed * Time.deltaTime
            );
        }

        private void HandleScroll(Vector2 scrollValue)
        {
            float scrollAmount = scrollValue.y;
            if (Mathf.Abs(scrollAmount) < 0.01f) return;

            Vector3 targetOffset = scrollAmount > 0 ? topDownOffset : defaultOffset;
            offset = Vector3.Lerp(offset, targetOffset, scrollSpeed * Time.deltaTime);
        }
        
        public void RotateForTurn(PlayerTurn turn)
        {
            isWhiteTurn = turn == PlayerTurn.White;
        }
    }
}