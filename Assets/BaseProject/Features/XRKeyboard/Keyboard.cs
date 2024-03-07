using UnityEngine;
using TMPro;

namespace VRUI
{
    public class Keyboard : MonoBehaviour
    {
        public delegate void KeyPressedDelegate(Keyboard keyboard, string keyPress);
        public event KeyPressedDelegate keyPressed;

        [SerializeField]
        private GameObject _letters;

        [SerializeField]
        private GameObject _numbers;

        [SerializeField]
        private KeyboardKey _layoutSwapKey;

        [SerializeField]
        private float _keyPressedDepth = 0.2f;
        public float KeyPressedDepth { get { return _keyPressedDepth; } }

        [SerializeField]
        private TextMeshPro headerText;
        public string HeaderText { set { headerText.text = value; } }

#if STEAM_VR
        private KeyboardMallet[] _mallets;
#endif
        private KeyboardKey[] _keys;

        private bool _shift = false;
        public bool shift { get { return _shift; } set { SetShift(value); } }

        public enum Layout
        {
            Letters,
            Numbers
        };

        private Layout _layout = Layout.Letters;
        public Layout layout { get { return _layout; } set { SetLayout(value); } }

        private KeyboardKey leftHandKey;        // Key that the left hand is currently pressing, if any
        private KeyboardKey rightHandKey;       // Key that the right hand is currently pressing, if any

        void Awake()
        {
#if STEAM_VR
            _mallets = GetComponentsInChildren<KeyboardMallet>(true);
#endif
            _keys = GetComponentsInChildren<KeyboardKey>(true);

#if STEAM_VR
            foreach (KeyboardMallet mallet in _mallets)
                mallet._keyboard = this;
#endif

            foreach (KeyboardKey key in _keys)
                key._keyboard = this;
        }

        private void OnDisable()
        {
            leftHandKey = rightHandKey = null;
        }

        // Internal
        public void _MalletStruckKeyboardKey(KeyboardKey key)
        {
            // Did we hit the key for another keyboard?
            if (key._keyboard != this)
                return;

            // Trigger key press animation
            key.KeyPressed();

            // Fire key press event
            if (keyPressed != null)
            {
                string keyPress = key.GetCharacter();

                bool shouldFireKeyPressEvent = true;

                if (keyPress == "\\s")
                {
                    // Shift
                    shift = !shift;
                    shouldFireKeyPressEvent = false;
                } else if (keyPress == "\\l")
                {
                    // Layout swap
                    if (layout == Layout.Letters)
                        layout = Layout.Numbers;
                    else if (layout == Layout.Numbers)
                        layout = Layout.Letters;

                    shouldFireKeyPressEvent = false;
                } else if (keyPress == "\\b")
                {
                    // Backspace
                    keyPress = "\b";
                } else
                {
                    // Turn off shift after typing a letter
                    if (shift && layout == Layout.Letters)
                        shift = false;
                }

                if (shouldFireKeyPressEvent)
                    keyPressed(this, keyPress);
            }
        }

        void SetShift(bool shift)
        {
            if (shift == _shift)
                return;

            foreach (KeyboardKey key in _keys)
                key.shift = shift;

            _shift = shift;
        }

        void SetLayout(Layout layout)
        {
            if (layout == _layout)
                return;

            shift = false;

            if (layout == Layout.Letters)
            {
                // Swap layouts
                _letters.SetActive(true);
                _numbers.SetActive(false);

                // Update layout swap key
                _layoutSwapKey.displayCharacter = "123";
                _layoutSwapKey.shiftDisplayCharacter = "123";
                _layoutSwapKey.RefreshDisplayCharacter();
            } else if (layout == Layout.Numbers)
            {
                // Swap layouts
                _letters.SetActive(false);
                _numbers.SetActive(true);

                // Update layout swap key
                _layoutSwapKey.displayCharacter = "abc";
                _layoutSwapKey.shiftDisplayCharacter = "abc";
                _layoutSwapKey.RefreshDisplayCharacter();
            }

            _layout = layout;
        }
        /*
        public void _FingerStruckKeyboardKey(KeyboardKey key, UIHand hand)
        {
            // Check to see if this hand is already pressing a key
            if (hand.IsLeftHand)
            {
                if (leftHandKey != null)
                    return;

                // Track that this hand is pressing this key
                leftHandKey = key;
            }
            else
            {
                if (rightHandKey != null)
                    return;

                // Track that this hand is pressing this key
                rightHandKey = key;
            }

            hand.TriggerHapticPulse(UISettings.KeyboardKeyHaptic);

            _MalletStruckKeyboardKey(key);
        }

        public void _FingerReleasedKeyboardKey(KeyboardKey key, UIHand hand)
        {
            // Check to see if this hand is actually recognized as pressing this key
            if (hand.IsLeftHand)
            {
                if (leftHandKey != key)
                    return;

                leftHandKey = null;
            }
            else
            {
                if (rightHandKey != key)
                    return;

                rightHandKey = null;
            }

            key.KeyReleased();
        }
        */
    }

}
