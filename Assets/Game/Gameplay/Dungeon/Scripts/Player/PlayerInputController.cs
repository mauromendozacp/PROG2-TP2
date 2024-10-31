using System;

using UnityEngine;
using UnityEngine.InputSystem;

public enum FSM_INPUT
{
    ENABLE_ALL,
    ONLY_UI,
    DISABLE_ALL
}

public class PlayerInputController : MonoBehaviour
{
    private PlayerInput inputAction = null;
    private FSM_INPUT currentInputState = default;

    private Action onPause = null;
    private Action onInvetory = null;
    private Action onPick = null;
    private Action<bool> onRun = null;

    public Vector2 Move { get => GetMoveValue(); }
    public FSM_INPUT CurrentInputState { get => currentInputState; }

    public void Init(Action onPause, Action onInvetory, Action onPick, Action<bool> onRun)
    {
        this.onPause = onPause;
        this.onInvetory = onInvetory;
        this.onPick = onPick;
        this.onRun = onRun;

        inputAction = new PlayerInput();

        inputAction.Player.Pause.performed += OnPause;
        inputAction.Player.Inventory.performed += OnInvetory;
        inputAction.Player.Pick.performed += OnPick;
        inputAction.Player.Run.performed += OnStartRun;
        inputAction.Player.Run.canceled += OnEndRun;

        UpdateInputFSM(FSM_INPUT.ENABLE_ALL);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        onPause?.Invoke();
    }

    public void OnInvetory(InputAction.CallbackContext context)
    {
        onInvetory?.Invoke();
    }

    public void OnPick(InputAction.CallbackContext context)
    {
        onPick?.Invoke();
    }

    public void OnStartRun(InputAction.CallbackContext context)
    {
        onRun?.Invoke(true);
    }

    public void OnEndRun(InputAction.CallbackContext context)
    {
        onRun?.Invoke(false);
    }

    public void UpdateInputFSM(FSM_INPUT fsm)
    {
        switch (fsm)
        {
            case FSM_INPUT.ENABLE_ALL:
                inputAction.Player.Enable();
                inputAction.UI.Enable();
                break;
            case FSM_INPUT.ONLY_UI:
                inputAction.Player.Disable();
                inputAction.UI.Enable();
                break;
            case FSM_INPUT.DISABLE_ALL:
                inputAction.Player.Disable();
                inputAction.UI.Disable();
                break;
        }

        currentInputState = fsm;
    }

    private Vector2 GetMoveValue()
    {
        if (inputAction == null) return Vector2.zero;

        return inputAction.Player.Move.ReadValue<Vector2>();
    }
}