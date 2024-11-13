using UnityEngine;

namespace Phezu.Derek {

    public class PlaneController : MonoBehaviour {

        [SerializeField] private PlaneInput m_PlaneInput;
        [SerializeField] private PlaneMotor m_PlaneMotor;

        [Space(5)]
        [Header("Configuration")]
        [SerializeField] private float m_RollingSpeed;
        [SerializeField] private float m_PitchingSpeed;
        [SerializeField] private float m_TurningSpeed;
        [SerializeField] private float m_MovementSpeed;

        private float m_OriginalHeight;

        private float m_TargetRoll;
        private float m_CurrRoll;

        private float m_TargetPitch;
        private float m_CurrPitch;

        private float m_CurrTurn;

        private void Start() {
            m_OriginalHeight = transform.position.y;
        }

        private void Update() {
            ApplyInput();
        }

        private void ApplyInput() {
            Vector2 input = m_PlaneInput.InputCommand;

            m_TargetRoll = input.x;

            if (input.y < 0f)
                m_TargetPitch = input.y;
            else if (transform.position.y < m_OriginalHeight)
                m_TargetPitch = 1f;
            else
                m_TargetPitch = 0f;

            m_CurrRoll = Mathf.Lerp(m_CurrRoll, m_TargetRoll, Time.deltaTime * m_RollingSpeed);
            m_CurrPitch = Mathf.Lerp(m_CurrPitch, m_TargetPitch, Time.deltaTime * m_PitchingSpeed);
            m_CurrTurn += input.x * Time.deltaTime * m_TurningSpeed;

            m_PlaneMotor.Pitch = m_CurrPitch;
            m_PlaneMotor.Roll = m_CurrRoll;
            m_PlaneMotor.Turn = m_CurrTurn;
            m_PlaneMotor.Speed = m_MovementSpeed;
        }
    }
}