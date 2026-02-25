using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")] 
        public InputPlayerHandler input;
        public Rigidbody rb;

        [Header("Movement")]
        public float speed = 5f;
        public float steerStrength = 5f; 
        
        [Header("Rotation")]
        public AnimationCurve rotationCurve;
        public float rotationSpeed = 10f;

        private float lifeTime;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

      
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.useGravity = false;
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }
        
        private void HandleMovement()
        {
            Vector3 velocity = transform.forward * speed;
            
            Vector3 inputDir = new Vector3(input.moveInput.x, 0f, input.moveInput.y);
            if (inputDir.sqrMagnitude > 0.01f)
            {
                inputDir.Normalize();
                velocity += inputDir * steerStrength;
            }

            rb.linearVelocity = velocity;

            RotateTowardsVelocity();
        }

        private void RotateTowardsVelocity()
        {
            Vector3 velocity = rb.linearVelocity;

            if (velocity.sqrMagnitude < 0.001f)
                return;

            Vector3 direction = velocity.normalized;

           
            float curveValue = rotationCurve.Evaluate(lifeTime);
            lifeTime += Time.fixedDeltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                curveValue * rotationSpeed * Time.fixedDeltaTime
            );
        }
        private void OnTriggerEnter(Collider other)
        {
            //Add SFX
                Destroy(other.gameObject);
        }
    }
}
