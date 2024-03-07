using UnityEngine;
using TMPro;

namespace VRUI
{
//    [ExecuteInEditMode]
    //[SelectionBaseFixed]
    public class KeyboardKey : MonoBehaviour
    {
        private const float HYSTERESIS_PCT = 1.25f;     // Percentage that the original collider should be scaled up to cause the pressing collider to have to travel an additional distance away from the key compared to where it was upon collision in order to prevent rapid-fire triggering of the key when the pressing collider is right at the edge of the collider.

        public string character = "a";
        
        // These are overrides in case you need something different.
        public string displayCharacter      = null;
        public string shiftCharacter        = null;
        public string shiftDisplayCharacter = null;

        private bool _shift = false;
        public  bool  shift { get { return _shift; } set { SetShift(value); } }

        [SerializeField]
        private TextMeshPro _text;

        [SerializeField]
        private Transform _geometry;
        private float     _position       = 0.0f;
        private float     _targetPosition = 0.0f;

        [SerializeField]
        private AudioSource _audioSource;

        // Internal
        [HideInInspector]
        public Keyboard _keyboard;

        private Transform localTransform;
        private Collider keyCollider;
        private Vector3 origColliderScale;

        void Awake()
        {
            localTransform = transform;

            // Configure the rigidbody
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.useGravity  = false;
            rigidbody.isKinematic = true;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            keyCollider = GetComponentInChildren<Collider>();
            origColliderScale = keyCollider.transform.localScale;

            if (_text == null)
                _text = GetComponentInChildren<TextMeshPro>(true);

            RefreshDisplayCharacter();
        }

#if STEAM_VR
        public bool IsMalletHeadInFrontOfKey(KeyboardMallet mallet)
        {
            Vector3 localMalletHeadPosition = transform.InverseTransformPoint(mallet.malletHeadPosition);

            return localMalletHeadPosition.y >= 0.0f;
        }
#endif

        public void KeyPressed()
        {
            _position = -_keyboard.KeyPressedDepth;

            // Enlarge the collider to prevent rapid-fire triggering if the pressing collider is on the borderline
            keyCollider.transform.localScale = origColliderScale * HYSTERESIS_PCT;

            if (_audioSource != null)
            {
                if (_audioSource.isPlaying)
                    _audioSource.Stop();

                float scalePitch = 1.0f;///(_keyboard.transform.lossyScale.x + 0.2f);
                float pitchVariance = Random.Range(0.97f, 1.02f);
                _audioSource.pitch = scalePitch * pitchVariance;
                _audioSource.Play();
            }
        }

        public void KeyReleased()
        {
            // Reset the collider to original scale:
            keyCollider.transform.localScale = origColliderScale;
        }

        void SetShift(bool shift)
        {
            if (shift == _shift)
                return;

            _shift = shift;

            RefreshDisplayCharacter();
        }

        // Key animation
        void Update()
        {
            // Animate bounce
            _position = Mathf.Lerp(_position, _targetPosition, Time.unscaledDeltaTime * 20.0f);

            // Set position
            Vector3 localPosition = _geometry.localPosition;
            localPosition.y = _position;
            _geometry.localPosition = localPosition;
        }

        public void RefreshDisplayCharacter()
        {
            _text.text = GetDisplayCharacter();
        }

        // Helper functions
        string GetDisplayCharacter()
        {
            // Start with the character
            string dc = character;
            if (dc == null)
                dc = "";

            // If we've got a display character, swap for that.
            if (displayCharacter != null && displayCharacter != "")
                dc = displayCharacter;

            // If we're in shift mode, check our shift overrides.
            if (_shift) {
                if (shiftDisplayCharacter != null && shiftDisplayCharacter != "")
                    dc = shiftDisplayCharacter;
                else if (shiftCharacter != null && shiftCharacter != "")
                    dc = shiftCharacter;
                else
                    dc = dc.ToUpper();
            }

            return dc;
        }

        public string GetCharacter()
        {
            if (shift) {
                if (shiftCharacter != null && shiftCharacter != "")
                    return shiftCharacter;
                else
                    return character.ToUpper();
            }

            return character;
        }

        /*
        private void OnTriggerEnter(Collider other)
        {
            if (_keyboard == null)
                return;

            if (string.Compare(other.gameObject.tag, "Finger") != 0)
                return;

            // Only respond to collision from above:
            var localContactPt = localTransform.InverseTransformPoint(other.transform.position);
            if (localContactPt.y <= 0)
                return;

            var hand = other.GetComponentInParent<UIHand>();

            if (hand == null || !hand.enabled)
                return;

            _keyboard._FingerStruckKeyboardKey(this, hand);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_keyboard == null)
                return;

            if (string.Compare(other.gameObject.tag, "Finger") != 0)
                return;

            var hand = other.GetComponentInParent<UIHand>();

            if (hand == null || !hand.enabled)
                return;

            _keyboard._FingerReleasedKeyboardKey(this, hand);
        }
#if UNITY_EDITOR
        public void SimulateKeypress()
        {
            if (_keyboard == null)
                return;

            _keyboard._MalletStruckKeyboardKey(this);
        }
        
#endif
       */
    }

}
