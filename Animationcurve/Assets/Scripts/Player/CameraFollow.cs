using UnityEngine;

namespace Player
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("References")]
        public Transform target;
        public Vector3 offset = new Vector3(0, 2, -10);

        [Header("Settings")]
        public float followSpeed = 5f;     
        public float rotationSpeed = 2f;   
        public bool rotateWithTarget = true;
        public float lookAheadDistance = 5f; 

        private void LateUpdate()
        {
            if (!target) return;

            
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

          
            if (rotateWithTarget)
            {
                Vector3 lookPoint = target.position + target.forward * lookAheadDistance;
                Quaternion desiredRotation = Quaternion.LookRotation(lookPoint - transform.position, Vector3.up);

                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
