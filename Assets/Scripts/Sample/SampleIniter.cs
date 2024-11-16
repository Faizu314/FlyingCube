using UnityEngine;

///Everything in the sample namespace is a dummy implementation of features not in the scope of the order.

namespace Phezu.Derek.Sample {

    /// <summary>
    /// SampleIniter ensures there is always a single InputManager
    /// irrespective of which scene you load first. In the actual
    /// game there will be a defined scene at the start and other
    /// scenes should not be responsible for checking if InputManager exists.
    /// </summary>
    public class SampleIniter : MonoBehaviour {
        [SerializeField] private InputManager m_InputManagerPrefab;

        private void Awake() {
            if (FindAnyObjectByType<InputManager>() != null)
                return;

            InputManager singleton = Instantiate(m_InputManagerPrefab);
            DontDestroyOnLoad(singleton);
        }
    }
}