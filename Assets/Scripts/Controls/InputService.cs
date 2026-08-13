using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CyberEvolution.Controls
{
    [DefaultExecutionOrder(-10)]
    public class InputService : MonoBehaviour
    {
        public static InputService Instance {get; private set;}
        public Vector2 Direction => _moveAction?.ReadValue<Vector2>() ?? Vector2.zero;
        public Vector2 Scroll => _scrollAction?.ReadValue<Vector2>() ?? Vector2.zero;

        public event Action OnJump;

        [SerializeField] private InputActionAsset _inputActions;
        
        private const string MoveActionPath = "Input/Move";
        private const string ScrollActionPath = "Input/Scroll";
        private const string JumpActionPath = "Input/Jump";

        private InputAction _moveAction;
        private InputAction _scrollAction;
        private InputAction _jumpAction;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;

            InitializeActions();
            SubscribeToEvents();
        }

        private void InitializeActions()
        {
            _moveAction = _inputActions.FindAction(MoveActionPath);
            _scrollAction = _inputActions.FindAction(ScrollActionPath);
            _jumpAction = _inputActions.FindAction(JumpActionPath);
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            _jumpAction.performed += OnJumpActionPerformed;
        }

        private void UnsubscribeFromEvents()
        {
            _jumpAction.performed -= OnJumpActionPerformed;
        }

        private void OnJumpActionPerformed(InputAction.CallbackContext obj)
        {
            OnJump?.Invoke();
        }
    }
}