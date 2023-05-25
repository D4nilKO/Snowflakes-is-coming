using UnityEngine;

namespace Project.Components.Scripts
{
    public class RotationController : MonoBehaviour
    {
        [SerializeField] private bool rotateEnabled;

        [SerializeField] [Range(1f, 500f)] private float rotationSpeed = 10f;

        private Quaternion targetRotation;

        private void Start()
        {
            targetRotation = transform.rotation;
        }

        private void FixedUpdate()
        {
            if (!rotateEnabled) return;
            var newRotation = targetRotation.eulerAngles.z + (rotationSpeed * Time.fixedDeltaTime);

            targetRotation = Quaternion.Euler(0f, 0f, newRotation);

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        public void RotateEnemy()
        {
            if (!rotateEnabled) return;
            var newRotation = targetRotation.eulerAngles.z + (rotationSpeed * Time.fixedDeltaTime);

            targetRotation = Quaternion.Euler(0f, 0f, newRotation);

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}