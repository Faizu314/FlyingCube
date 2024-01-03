using System.Collections.Generic;
using UnityEngine;
using Phezu.Util;

namespace Phezu.Derek {

    public class InputManager : Singleton<InputManager> {
        [SerializeField] private string[] m_HudButtons;

        public string[] HudButtons {
            get {
                string[] result = new string[m_HudButtons.Length];
                for (int i = 0; i < result.Length; i++)
                    result[i] = m_HudButtons[i];
                return result;
            }
        }

        private bool[] m_HudButtonsState;

        private void Awake() {
            if (m_HudButtons == null)
                return;
            m_HudButtonsState = new bool[m_HudButtons.Length];
        }

#if UNITY_EDITOR

        [SerializeField] private List<bool> m_DebugInputs;

        private void Start() {
            for (int i = 0; i < m_HudButtons.Length; i++)
                m_DebugInputs.Add(false);
        }
        private void Update() {
            for (int i = 0; i < m_DebugInputs.Count; i++)
                m_DebugInputs[i] = m_HudButtonsState[i];
        }

#endif

        public void SetHudButton(int hudButtonIndex, bool state) {
            m_HudButtonsState[hudButtonIndex] = state;
        }

        public bool GetHudButton(int hudButtonIndex) {
            return m_HudButtonsState[hudButtonIndex];
        }

        public bool[] GetHudButtons() {
            bool[] buttons = new bool[m_HudButtonsState.Length];

            for (int i = 0; i < buttons.Length; i++) {
                buttons[i] = m_HudButtonsState[i];
            }

            return buttons;
        }
    }
}