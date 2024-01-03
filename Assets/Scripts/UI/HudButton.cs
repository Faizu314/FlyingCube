using UnityEngine;
using UnityEngine.EventSystems;

namespace Phezu.Derek.UI {

    public class HudButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

        [HudButton] public int m_ButtonType;

        private InputManager m_InputManager;

        private void Start() {
            m_InputManager = InputManager.Instance;
        }

        public void OnPointerDown(PointerEventData eventData) {
            m_InputManager.SetHudButton(m_ButtonType, true);
        }

        public void OnPointerUp(PointerEventData eventData) {
            m_InputManager.SetHudButton(m_ButtonType, false);
        }
    }
}