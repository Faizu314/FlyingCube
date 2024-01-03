using UnityEngine;

namespace Phezu.Derek {

    public class PlayerInput : MonoBehaviour {

        private Vector2 m_InputCommand;

        public Vector2 InputCommand => m_InputCommand;

        private InputManager m_Input;

        private void Start() {
            m_Input = InputManager.Instance;
        }

        private void Update() {
            m_InputCommand = Vector2.zero;

            if (m_Input.GetHudButton(0))
                m_InputCommand.x--;
            if (m_Input.GetHudButton(1))
                m_InputCommand.x++;
            if (m_Input.GetHudButton(2))
                m_InputCommand.y--;
        }
    }
}