using System;

using UnityEngine;
using UnityEngine.InputSystem;

public enum FSM_INPUT
{
    ENABLE_ALL,
    INTERACTING,
    INVENTORY,
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
    private Action onChangeWeapons = null;
    private Action onPressAction1 = null;
    private Action onPressAction2 = null;
    private Action onCancelAction1 = null;
    private Action onCancelAction2 = null;

    public Vector2 Move { get => GetMoveValue(); }
    public FSM_INPUT CurrentInputState { get => currentInputState; }

    public void Init(Action onPause, Action onInvetory, Action onPick, Action<bool> onRun, Action onChangeWeapons,
        Action onPressAction1, Action onPressAction2, Action onCancelAction1, Action onCancelAction2)
    {
        this.onPause = onPause;
        this.onInvetory = onInvetory;
        this.onPick = onPick;
        this.onRun = onRun;
        this.onChangeWeapons = onChangeWeapons;
        this.onPressAction1 = onPressAction1;
        this.onPressAction2 = onPressAction2;
        this.onCancelAction1 = onCancelAction1;
        this.onCancelAction2 = onCancelAction2;

        inputAction = new PlayerInput();
        inputAction.Player.PauseGame.performed += OnPause;
        inputAction.Player.Inventory.performed += OnInvetory;
        inputAction.Player.PickItem.performed += OnPick;
        inputAction.Player.Run.performed += OnStartRun;
        inputAction.Player.Run.canceled += OnEndRun;
        inputAction.Player.ChangeWeapon.performed += OnChangeWeapons;
        inputAction.Player.Action1.performed += OnPressAction1;
        inputAction.Player.Action2.performed += OnPressAction2;
        inputAction.Player.Action1.canceled += OnCancelAction1;
        inputAction.Player.Action2.canceled += OnCancelAction2;

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

    public void OnChangeWeapons(InputAction.CallbackContext context)
    {
        onChangeWeapons?.Invoke();
    }

    public void OnPressAction1(InputAction.CallbackContext context)
    {
        onPressAction1?.Invoke();
    }

    public void OnPressAction2(InputAction.CallbackContext context)
    {
        onPressAction2?.Invoke();
    }

    public void OnCancelAction1(InputAction.CallbackContext context)
    {
        onCancelAction1?.Invoke();
    }

    public void OnCancelAction2(InputAction.CallbackContext context)
    {
        onCancelAction2?.Invoke();
    }

    public void UpdateInputFSM(FSM_INPUT fsm, bool setCurrentState = true)
    {
        switch (fsm)
        {
            case FSM_INPUT.ENABLE_ALL:
                inputAction.Player.Enable();
                inputAction.UI.Enable();
                break;
            case FSM_INPUT.INTERACTING:
                inputAction.Player.Disable();
                inputAction.Player.PauseGame.Enable();
                inputAction.UI.Enable();
                break;
            case FSM_INPUT.INVENTORY:
                inputAction.Player.Disable();
                inputAction.Player.Inventory.Enable();
                inputAction.Player.PauseGame.Enable();
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

        if (setCurrentState)
        {
            currentInputState = fsm;
        }
    }

    private Vector2 GetMoveValue()
    {
        if (inputAction == null) return Vector2.zero;

        return inputAction.Player.Move.ReadValue<Vector2>();
    }
}