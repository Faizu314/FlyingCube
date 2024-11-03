using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Phezu.Derek {

    public class DialogueController : MonoBehaviour, IPointerDownHandler
    {
        [Header("References")]
        [SerializeField] private Image m_Background;
        [SerializeField] private Image m_LeftCharacter;
        [SerializeField] private Image m_RightCharacter;
        [SerializeField] private DialoguePlayer m_DialoguePlayer;

        private DialogueData m_DialogueData;

        public void SetDialogueData(DialogueData dialogueData) {
            m_DialogueData = dialogueData;

            UpdateDialogueCanvas();
        }

        private void UpdateDialogueCanvas() {
            m_Background.sprite = m_DialogueData.Background;
            m_LeftCharacter.sprite = m_DialogueData.LeftCharacter;
            m_RightCharacter.sprite = m_DialogueData.RightCharacter;

            m_DialoguePlayer.StartDialogue(m_DialogueData);
        }

        public void OnPointerDown(PointerEventData eventData) {
            m_DialoguePlayer.NextDialogue();
        }
    }
}