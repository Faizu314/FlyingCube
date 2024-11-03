using UnityEngine;
using Phezu.Derek;

public class SampleIniter : MonoBehaviour
{
    [SerializeField] private DialogueController m_DialogueController;
    [SerializeField] private DialogueData m_DialogueData;

    private void Start() {
        m_DialogueController.SetDialogueData(m_DialogueData);
    }
}
