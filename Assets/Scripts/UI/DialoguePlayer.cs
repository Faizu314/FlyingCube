using System.Collections;
using UnityEngine;
using TMPro;

namespace Phezu.Derek {

    public class DialoguePlayer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI m_DialogueText;
        [SerializeField] private RectTransform m_DialogueContainer;

        [Space]
        [Header("Configuration")]
        [SerializeField] private bool m_DontFade = false;
        [SerializeField] private float m_FadeOutDuration = 0.5f;
        [SerializeField] private EasingFunction.Ease m_FadeOutEase = EasingFunction.Ease.Linear;
        [SerializeField] private float m_FadeInDuration = 0.5f;
        [SerializeField] private EasingFunction.Ease m_FadeInEase = EasingFunction.Ease.Linear;

        private Coroutine m_FadeCo = null;
        private DialogueData m_DialogueData;
        private int m_DialogueIndex;

        private void Start() {
            m_DialogueContainer.localScale = Vector3.zero;
        }

        public void StartDialogue(DialogueData dialogueData) {
            m_DialogueData = dialogueData;
            m_DialogueIndex = 0;

            m_DialogueText.text = m_DialogueData.Dialogues[m_DialogueIndex].Text;

            if (m_FadeCo == null)
                m_FadeCo = StartCoroutine(nameof(FadeIn_Co));
        }

        public void NextDialogue() {
            if (m_DialogueIndex >= m_DialogueData.Dialogues.Length - 1)
                return;

            if (m_FadeCo == null) {
                m_DialogueIndex++;
                m_FadeCo = StartCoroutine(nameof(NextDialogue_Co));
            }
            else
                SkipFade();
        }

        private void SkipFade() {
            if (m_FadeCo != null) {
                StopAllCoroutines();
                m_FadeCo = null;
            }

            m_DialogueText.text = m_DialogueData.Dialogues[m_DialogueIndex].Text;
            m_DialogueContainer.localScale = Vector3.one;
        }

        private IEnumerator NextDialogue_Co() {
            m_FadeCo = StartCoroutine(nameof(FadeOut_Co));

            yield return m_FadeCo;

            m_DialogueText.text = m_DialogueData.Dialogues[m_DialogueIndex].Text;

            m_FadeCo = StartCoroutine(nameof(FadeIn_Co));

            yield return m_FadeCo;

            m_FadeCo = null;
        }

        private IEnumerator FadeIn_Co() {
            var func = EasingFunction.GetEasingFunction(m_FadeInEase);

            float t = 0f;

            while (t < m_FadeInDuration) {
                func.Invoke(0f, m_FadeInDuration, t);
                t += Time.deltaTime;

                m_DialogueContainer.localScale = Vector3.one * t / m_FadeInDuration;

                yield return null;
            }

            m_DialogueContainer.localScale = Vector3.one;

            m_FadeCo = null;
        }

        private IEnumerator FadeOut_Co() {
            var func = EasingFunction.GetEasingFunction(m_FadeOutEase);

            float t = 0f;

            while (t < m_FadeOutDuration) {
                func.Invoke(0f, m_FadeOutDuration, t);
                t += Time.deltaTime;

                m_DialogueContainer.localScale = Vector3.one * (1f - (t / m_FadeOutDuration));

                yield return null;
            }

            m_DialogueContainer.localScale = Vector3.zero;

            m_FadeCo = null;
        }
    }
}