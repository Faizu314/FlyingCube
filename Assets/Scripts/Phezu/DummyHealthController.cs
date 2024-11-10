using UnityEngine;

public class DummyHealthController : MonoBehaviour
{
    public void OnLanded() {
        Debug.Log("Landed Successfuly");
    }

    public void OnCrashed() {
        Debug.Log("Crash Landed");
    }
}
