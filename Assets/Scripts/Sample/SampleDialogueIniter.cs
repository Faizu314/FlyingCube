using UnityEngine;

///Everything in the sample namespace is a dummy implementation of features not in the scope of the order.

namespace Phezu.Derek.Sample {

    /// <summary>
    /// SampleDialogueIniter just loads dummy data to the dialogue controller.
    /// </summary>
    public class SampleDialogueIniter : MonoBehaviour {
        [SerializeField] private DialogueController m_DialogueController;
        [SerializeField] private DialogueData m_DialogueData;

        private void Start() {
            m_DialogueController.SetDialogueData(m_DialogueData);
        }
    }
}