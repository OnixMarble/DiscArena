using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/Input", order = 1)]
public class InputReader : ScriptableObject
{
    public event UnityAction<Vector2> OnTouchScreenEvent = null;
    public event UnityAction<Vector2> OnTouchEndEvent = null;
    public event UnityAction OnTouchEndUIEvent = null;
    private GameInputActions m_GameInputActions = null;

    private void OnEnable()
    {
        if (m_GameInputActions == null)
        {
            m_GameInputActions = new GameInputActions();
        }

        m_GameInputActions.Gameplay.Disable();
        m_GameInputActions.UI.Enable();

        m_GameInputActions.UI.TouchPress.canceled += OnUITouchUp;
        m_GameInputActions.Gameplay.Touch.performed += OnGameplayTouchScreen;
        m_GameInputActions.Gameplay.TouchPress.canceled += OnGameplayTouchUp;
    }

    private void OnDisable()
    {
        m_GameInputActions.Disable();

        m_GameInputActions.UI.TouchPress.canceled -= OnUITouchUp;
        m_GameInputActions.Gameplay.Touch.performed -= OnGameplayTouchScreen;
        m_GameInputActions.Gameplay.TouchPress.canceled -= OnGameplayTouchUp;
    }

    private void OnUITouchUp(InputAction.CallbackContext context)
    {
        OnTouchEndUIEvent?.Invoke();
        ActivateGameplayActionMap();
    }

    private void OnGameplayTouchScreen(InputAction.CallbackContext context)
    {
        OnTouchScreenEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnGameplayTouchUp(InputAction.CallbackContext context)
    {
        OnTouchEndEvent?.Invoke(m_GameInputActions.Gameplay.Touch.ReadValue<Vector2>());

        ActivateUIActionMap();
    }

    private void ActivateUIActionMap()
    {
        m_GameInputActions.UI.Enable();
        m_GameInputActions.Gameplay.Disable();
    }

    private void ActivateGameplayActionMap()
    {
        m_GameInputActions.UI.Disable();
        m_GameInputActions.Gameplay.Enable();
    }
}
