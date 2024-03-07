using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VRUI
{
    public class KeyboardDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _text;

        [SerializeField]
        private Keyboard _keyboard;
        public  Keyboard  keyboard { get { return _keyboard; } set { SetKeyboard(value); } }

        [SerializeField]
        private bool multiline = false;

        [SerializeField]
        private int maxChars = -1;

        public event System.Action OnTextChanged;
        public event System.Action OnSubmit;

        public string Text
        {
            get { return _text.text; }
            set
            {
                if (string.Compare(_text.text, value) != 0)
                {
                    _text.text = value;

                    if (OnTextChanged != null)
                    {
                        OnTextChanged();
                    }
                }
            }
        }

        void Awake()
        {
            if (_text == null)
                _text = GetComponentInChildren<TextMeshPro>(true);

            StartObservingKeyboard(_keyboard);
        }

        void OnDestroy()
        {
            StopObservingKeyboard(_keyboard);
        }

        public void SetMaxChars(int limit = -1)
        {
            maxChars = limit;
        }

        void SetKeyboard(Keyboard keyboard)
        {
            if (keyboard == _keyboard)
                return;

            StopObservingKeyboard(_keyboard);
            StartObservingKeyboard(keyboard);

            _keyboard = keyboard;
        }

        void StartObservingKeyboard(Keyboard keyboard)
        {
            if (keyboard == null)
                return;

            keyboard.keyPressed += KeyPressed;
        }

        void StopObservingKeyboard(Keyboard keyboard)
        {
            if (keyboard == null)
                return;

            keyboard.keyPressed -= KeyPressed;
        }

        void KeyPressed(Keyboard keyboard, string keyPress)
        {
            string text = Text;

            if (keyPress == "\b")
            {
                // Backspace
                if (text.Length > 0)
                    text = text.Remove(text.Length - 1);
            }
            else if (string.Compare(keyPress, "\\n") == 0)
            {
                if (!multiline)
                {
                    if (OnSubmit != null)
                        OnSubmit();
                    return;
                }
                else
                    text += System.Environment.NewLine;
            }
            else
            {
                // Don't exceed our max character count:
                if (maxChars > -1 && Text.Length >= maxChars)
                    return;

                // Regular key press
                text += keyPress;
            }

            Text = text;
        }
    }
}
