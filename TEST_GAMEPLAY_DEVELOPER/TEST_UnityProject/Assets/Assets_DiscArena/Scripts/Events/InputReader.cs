using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/Input", order = 1)]
public class InputReader : ScriptableObject
{
    public event UnityAction<Vector2> OnTouchScreenEvent = null;
    public event UnityAction<Vector2> OnShootEvent = null;
    private GameInputActions m_GameInputActions = null;

    private void OnEnable()
    {
        if (m_GameInputActions == null)
        {
            m_GameInputActions = new GameInputActions();
        }

        EnableInput();

        m_GameInputActions.Controls.Touch.performed += OnTouchScreen;
        m_GameInputActions.Controls.TouchPress.canceled += OnShoot;
    }

    private void OnDisable()
    {
        DisableInput();

        m_GameInputActions.Controls.Touch.performed -= OnTouchScreen;
        m_GameInputActions.Controls.TouchPress.canceled -= OnShoot;
    }

    private void OnTouchScreen(InputAction.CallbackContext context)
    {
        OnTouchScreenEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        OnShootEvent?.Invoke(m_GameInputActions.Controls.Touch.ReadValue<Vector2>());
    }

    private void EnableInput()
    {
        if (m_GameInputActions == null)
        {
            return;
        }

        m_GameInputActions.Enable();
    }

    private void DisableInput()
    {
        if (m_GameInputActions == null)
        {
            return;
        }

        m_GameInputActions.Disable();
    }
}
