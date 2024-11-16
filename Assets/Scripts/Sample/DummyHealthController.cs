using UnityEngine;

///Everything in the sample namespace is a dummy implementation of features not in the scope of the order.

namespace Phezu.Derek.Sample {

    /// <summary>
    /// DummyHealthController just tests the OnLanded and OnCrashed events.
    /// </summary>
    public class DummyHealthController : MonoBehaviour {
        public void OnLanded() {
            Debug.Log("Landed Successfuly");
        }

        public void OnCrashed() {
            Debug.Log("Crash Landed");
        }
    }
}