using System;
using UnityEngine;

namespace Phezu.Derek {

    [CreateAssetMenu(fileName ="DialogueData", menuName ="Phezu/DialogueData")]
    public class DialogueData : ScriptableObject
    {
        public enum CharacterType { None = 0, Left, Right };

        [Serializable]
        public struct Dialogue {
            public CharacterType Type;
            [TextArea]
            public string Text;
            public AudioClip VoiceOver;
        }

        [Header("Sprites")]
        public Sprite Background;
        public Sprite LeftCharacter;
        public Sprite RightCharacter;

        [Space]
        public Dialogue[] Dialogues;
    }
}
