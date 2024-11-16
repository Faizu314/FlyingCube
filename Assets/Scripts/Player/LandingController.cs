using Phezu.Util;
using UnityEngine;
using UnityEngine.Events;

namespace Phezu.Derek {

    public class LandingController : MonoBehaviour {

        [Header("References")]
        [SerializeField] private Animator m_Animator;
        [SerializeField] private PlaneInput m_PlaneInput;
        [SerializeField] private PlaneMotor m_PlaneMotor;
        [SerializeField] private LandingPlaneForward m_CustomForward;
        [Tooltip("Minimum Y value of the plane model. Will be compared with GroundY to check for collision.")]
        [SerializeField] private Transform m_PlaneBottom;

        [Space(5)]
        [Header("Configuration")]
        [SerializeField] private Vector3 m_StartPosition;
        [SerializeField] private float m_TotalWidth;
        [SerializeField] private float m_GroundY;
        [SerializeField] [Range(-1f, 0f)] private float m_MinLandingMotorPitch;
        [SerializeField] private float m_PitchingSpeed;
        [SerializeField] private float m_MovementSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_UpWeightWhileTurning = 0.2f;
        [SerializeField] private float m_LandedDecceleration;
        [SerializeField] [SceneField] private string m_SceneAfterLanding;
        [SerializeField] [SceneField] private string m_SceneAfterCrashing;
        [SerializeField] [SceneField] private string m_SceneAfterExiting;

        public Vector3 ComponentSpeed = Vector3.one;

        private float m_TargetPitch;
        private float m_CurrPitch;

        private float m_HeightAtCollision;
        private bool m_IsLanding = false;
        private bool m_HasCrashed = false;
        private bool m_HasExited = false;
        private bool m_IsTurning = false;

        public UnityEvent OnLanded;
        public UnityEvent OnCrashed;

        private void Update() {
            if (m_HasExited)
                return;

            if (m_IsTurning) {
                m_PlaneMotor.ComponentNormalizedSpeed = ComponentSpeed;
                return;
            }

            ApplyInput();
            CheckExit();

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

        public void OnTurnAnimationComplete() {
            m_IsTurning = false;
            m_CustomForward.UpWeight = 1f;
        }

        private void ProcessInput() {
            Vector2 input = m_PlaneInput.InputCommand;

            if (input.y < 0f)
                m_TargetPitch = input.y;
            else
                m_TargetPitch = 0f;

            m_CurrPitch = Mathf.Lerp(m_CurrPitch, m_TargetPitch, Time.deltaTime * m_PitchingSpeed);
            m_Animator.enabled = m_IsTurning = input.x != 0;

            if (input.x < 0 && Mathf.Abs(m_PlaneMotor.Turn) % 360f < 1f) {
                m_Animator.Play("PlaneTurnLeft");
            }
            else if (input.x > 0 && Mathf.Abs(m_PlaneMotor.Turn - 180f) % 360f < 1f) {
                m_Animator.Play("PlaneTurnRight");
            }
            else
                m_Animator.enabled = m_IsTurning = false;

            m_CustomForward.UpWeight = m_IsTurning ? m_UpWeightWhileTurning : 1f;
        }

        private void ApplyInput() {
            m_PlaneMotor.Pitch = m_CurrPitch;
            m_PlaneMotor.Speed = m_MovementSpeed;
            m_PlaneMotor.ComponentNormalizedSpeed = ComponentSpeed;
        }

        private void CheckExit() {
            float sqrDistFromStart = 
                Vector2.SqrMagnitude(new Vector2(transform.position.x, transform.position.z) - new Vector2(m_StartPosition.x, m_StartPosition.z));

            if (sqrDistFromStart > m_TotalWidth * m_TotalWidth / 4f) {
                SceneLoader.Instance.LoadScene(m_SceneAfterExiting);
                m_HasExited = true;
            }
        }

        private void LandingTick() {
            m_CurrPitch = Mathf.Lerp(m_CurrPitch, 0f, Time.deltaTime * m_PitchingSpeed);
            m_MovementSpeed -= m_LandedDecceleration * Time.deltaTime;
            if (m_MovementSpeed < 0.2f && !m_HasCrashed)
                SceneLoader.Instance.LoadScene(m_SceneAfterLanding);
            else if (m_MovementSpeed < 0.2f && m_HasCrashed)
                SceneLoader.Instance.LoadScene(m_SceneAfterCrashing);
            else if (m_MovementSpeed < 0f)
                m_MovementSpeed = 0f;

            var pos = m_PlaneMotor.transform.position;
            pos.y = m_HeightAtCollision;
            m_PlaneMotor.transform.position = pos;
        }

        private void OnGroundHit() {
            if (m_CurrPitch <= m_MinLandingMotorPitch) {
                m_HasCrashed = true;
                OnCrashed?.Invoke();
            }
            else {
                OnLanded?.Invoke();
            }

            m_IsLanding = true;
            m_HeightAtCollision = m_PlaneMotor.transform.position.y;
        }
    }
}