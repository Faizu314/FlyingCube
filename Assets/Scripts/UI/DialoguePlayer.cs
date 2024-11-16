using System.Collections;
using UnityEngine;
using TMPro;

namespace Phezu.Derek {

    public class DialoguePlayer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI m_DialogueText;
        [SerializeField] private RectTransform m_DialogueContainer;
        [SerializeField] private AudioSource m_DialogueAudioPlayer;

        [Space]
        [Header("Configuration")]
        [SerializeField] private float m_FadeOutDuration = 0.5f;
        [SerializeField] private EasingFunction.Ease m_FadeOutEase = EasingFunction.Ease.Linear;
        [SerializeField] private float m_FadeInDuration = 0.5f;
        [SerializeField] private EasingFunction.Ease m_FadeInEase = EasingFunction.Ease.Linear;

        private enum CoroutineStage { None = 0, FadingOutPrev, FadingInCurr };

        private Coroutine m_FadeCo = null;
        private DialogueData m_DialogueData;
        private int m_DialogueIndex;
        private CoroutineStage m_CoroutineStage;

        private void Start() {
            m_DialogueContainer.localScale = Vector3.zero;
        }

        public void StartDialogue(DialogueData dialogueData) {
            m_DialogueData = dialogueData;
            m_DialogueIndex = 0;

            ShowTextAndPlayAudio();

            if (m_FadeCo == null)
                m_FadeCo = StartCoroutine(nameof(FadeIn_Co));
        }

        public bool NextDialogue() {
            if (m_DialogueIndex >= m_DialogueData.Dialogues.Length - 1)
                return false;

            if (m_FadeCo == null) {
                m_DialogueIndex++;
                m_FadeCo = StartCoroutine(nameof(NextDialogue_Co));
            }
            else
                SkipFade();

            return true;
        }

        private void SkipFade() {
            StopAllCoroutines();
            m_FadeCo = null;

            if (m_CoroutineStage == CoroutineStage.FadingOutPrev) {
                ShowTextAndPlayAudio();
            }

            m_DialogueContainer.localScale = Vector3.one;
        }

        private IEnumerator NextDialogue_Co() {
            m_CoroutineStage = CoroutineStage.FadingOutPrev;

            m_FadeCo = StartCoroutine(nameof(FadeOut_Co));

            yield return m_FadeCo;

            m_CoroutineStage = CoroutineStage.FadingInCurr;

            ShowTextAndPlayAudio();

            m_FadeCo = StartCoroutine(nameof(FadeIn_Co));

            yield return m_FadeCo;

            m_CoroutineStage = CoroutineStage.None;
            m_FadeCo = null;
        }

        private IEnumerator FadeOut_Co() {
            if (m_DialogueAudioPlayer.isPlaying)
                m_DialogueAudioPlayer.Stop();

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

        private void ShowTextAndPlayAudio() {
            m_DialogueText.text = m_DialogueData.Dialogues[m_DialogueIndex].Text;

            if (m_DialogueData.Dialogues[m_DialogueIndex].VoiceOver != null) {
                m_DialogueAudioPlayer.clip = m_DialogueData.Dialogues[m_DialogueIndex].VoiceOver;
                m_DialogueAudioPlayer.Play();
            }
        }
    }
}