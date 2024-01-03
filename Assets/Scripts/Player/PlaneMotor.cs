using UnityEngine;

namespace Phezu.Derek {

    public class PlaneMotor : MonoBehaviour {

        [SerializeField] private Transform m_TurnTransform;
        [SerializeField] private Transform m_PitchRollTransform;

        public float Speed;

        [Tooltip("This transforms the pitch roll transform.")]
        [Range(-1f, 1f)] public float Pitch;

        [Tooltip("This does not affect the direction of movement.")]
        [Range(-1f, 1f)] public float Roll;

        [Tooltip("Not the Yaw. 0 is world forward direction, 180 is world back. This transforms the TurnTransform")]
        public float Turn;

        [Header("Configuration")]
        [Tooltip("A value of 90 means plane can move vertically down and up.")]
        [SerializeField] [Range(0f, 90f)] private float m_MaxPitchInDegrees;
        [Tooltip("A value of 90 means plane can roll 90 degrees to the left and right.")]
        [SerializeField] [Range(0f, 90f)] private float m_MaxRollInDegrees;

        private Transform m_Transform;

        private void Start() {
            m_Transform = transform;
        }

        private void Update() {
            UpdateOrientation();
            MoveForward();
        }

        private void UpdateOrientation() {
            var pitchInDegrees = -Mathf.Lerp(-m_MaxPitchInDegrees, m_MaxPitchInDegrees, (Pitch + 1f) / 2f);
            var rollInDegrees = -Mathf.Lerp(-m_MaxRollInDegrees, m_MaxRollInDegrees, (Roll + 1f) / 2f);

            m_TurnTransform.rotation = Quaternion.Euler(new(0f, Turn, 0f));
            m_PitchRollTransform.localRotation = Quaternion.Euler(new(pitchInDegrees, 0f, rollInDegrees));
        }

        private void MoveForward() {
            m_Transform.Translate(Speed * Time.deltaTime * m_PitchRollTransform.forward);
        }
    }
}