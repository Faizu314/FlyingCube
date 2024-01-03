using UnityEngine;

namespace Phezu.Derek {

    public class PlayerMotor : MonoBehaviour {

        [SerializeField] private Transform m_TurnTransform;
        [SerializeField] private Transform m_PitchRollTransform;
        [SerializeField] private float m_Speed;

        [Tooltip("-1 is nose diving at 90 degrees, 1 is going vertically up. This transforms the pitch roll transform.")]
        [SerializeField][Range(-1f, 1f)] private float m_Pitch;

        [Tooltip("-1 is left 90 degrees, 1 is right 90 degrees. This transforms the pitch roll transform. This does not affect the direction of movement.")]
        [SerializeField][Range(-1f, 1f)] private float m_Roll;

        [Tooltip("Not the Yaw. 0 is world forward direction, 180 is world back. This transforms the TurnTransform")]
        [SerializeField] private float m_Turn;

        private Transform m_Transform;

        private void Start() {
            m_Transform = transform;
        }

        private void Update() {
            UpdateOrientation();
            MoveForward();
        }

        private void UpdateOrientation() {
            var pitchInDegrees = -Mathf.Lerp(-90f, 90f, (m_Pitch + 1f) / 2f);
            var rollInDegrees = -Mathf.Lerp(-90f, 90f, (m_Roll + 1f) / 2f);

            m_TurnTransform.rotation = Quaternion.Euler(new(0f, m_Turn, 0f));
            m_PitchRollTransform.localRotation = Quaternion.Euler(new(pitchInDegrees, 0f, rollInDegrees));
        }

        private void MoveForward() {
            m_Transform.Translate(m_Speed * Time.deltaTime * m_PitchRollTransform.forward);
        }
    }
}