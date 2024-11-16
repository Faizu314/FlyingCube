using UnityEngine;

public class LandingPlaneForward : MonoBehaviour
{
    [SerializeField] private Transform m_PitchTransform;

    public float UpWeight = 1f;

    private void Update() {
        var upProjection = Vector3.up * Vector3.Dot(Vector3.up, m_PitchTransform.forward);
        var forwardProjection = Vector3.forward * Vector3.Dot(Vector3.forward, m_PitchTransform.forward);

        transform.forward = (upProjection * 1f) + forwardProjection;
    }
}
