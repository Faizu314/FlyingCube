using UnityEngine;
using UnityEngine.Events;

namespace Phezu.Derek {

    public class LandingController : MonoBehaviour {

        [SerializeField] private PlaneInput m_PlaneInput;
        [SerializeField] private PlaneMotor m_PlaneMotor;
        [Tooltip("Minimum Y value of the plane model. Will be compared with GroundY to check for collision.")]
        [SerializeField] private Transform m_PlaneBottom;

        [Space(5)]
        [Header("Configuration")]
        [SerializeField] private float m_GroundY;
        [SerializeField] [Range(-1f, 0f)] private float m_MinLandingMotorPitch;
        [SerializeField] private float m_PitchingSpeed;
        [SerializeField] private float m_MovementSpeed;
        [SerializeField] private float m_LandedDecceleration;

        private float m_TargetPitch;
        private float m_CurrPitch;

        private float m_HeightAtCollision;
        private bool m_IsLanding = false;
        private bool m_HasCrashed = false;

        public UnityEvent OnLanded;
        public UnityEvent OnCrashed;

        private void Update() {
            if (m_HasCrashed)
                return;

            ApplyInput();

            if (m_IsLanding) {
                LandingTick();
            }
            else {
                ProcessInput();

                if (m_PlaneBottom.position.y <= m_GroundY) {
                    OnGroundHit();
                }
            }
        }

        private void ProcessInput() {
            Vector2 input = m_PlaneInput.InputCommand;

            if (input.y < 0f)
                m_TargetPitch = input.y;
            else
                m_TargetPitch = 0f;

            m_CurrPitch = EasingFunction.EaseInSine(m_CurrPitch, m_TargetPitch, Time.deltaTime * m_PitchingSpeed);
        }

        private void ApplyInput() {
            m_PlaneMotor.Pitch = m_CurrPitch;
            m_PlaneMotor.Speed = m_MovementSpeed;
        }

        private void LandingTick() {
            m_CurrPitch = EasingFunction.EaseInSine(m_CurrPitch, 0f, Time.deltaTime * m_PitchingSpeed);
            m_MovementSpeed -= m_LandedDecceleration * Time.deltaTime;

            var pos = m_PlaneMotor.transform.position;
            pos.y = m_HeightAtCollision;
            m_PlaneMotor.transform.position = pos;
        }

        private void OnGroundHit() {
            if (m_CurrPitch <= m_MinLandingMotorPitch) {
                OnCrashed?.Invoke();
                m_HasCrashed = true;
            }
            else {
                OnLanded?.Invoke();
                m_IsLanding = true;
                m_HeightAtCollision = m_PlaneMotor.transform.position.y;
            }
        }
    }
}