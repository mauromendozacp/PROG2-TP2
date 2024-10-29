using System;

using UnityEngine;
using UnityEngine.InputSystem;

public enum FSM_INPUT
{
    ENABLE_ALL,
    MOVEMENT,
    ATTACK,
    ONLY_UI,
    DISABLE_INTERACTIONS,
    DISABLE_ALL
}

public class PlayerInputController : MonoBehaviour
{
    private DungeonInput inputAction = null;
    private FSM_INPUT currentInputState = default;

    private Action onFire = null;
    private Action onRoll = null;
    private Action onPause = null;

    public Vector2 Move { get => GetMoveValue(); }
    public FSM_INPUT CurrentInputState { get => currentInputState; }

    public void Init(Action onFire, Action onRoll, Action onPause)
    {
        this.onFire = onFire;
        this.onRoll = onRoll;
        this.onPause = onPause;

        inputAction = new DungeonInput();

        inputAction.Player.Fire.performed += OnFire;

        UpdateInputFSM(FSM_INPUT.ENABLE_ALL);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        onFire?.Invoke();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        onRoll?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        onPause?.Invoke();
    }

    public void UpdateInputFSM(FSM_INPUT fsm)
    {
        switch (fsm)
        {
            case FSM_INPUT.ENABLE_ALL:
                inputAction.Player.Enable();
                inputAction.UI.Enable();
                break;
            case FSM_INPUT.MOVEMENT:
                inputAction.Player.Fire.Disable();
                inputAction.Player.Move.Enable();
                break;
            case FSM_INPUT.ATTACK:
                inputAction.Player.Fire.Enable();
                inputAction.Player.Move.Disable();
                break;
            case FSM_INPUT.ONLY_UI:
                inputAction.Player.Disable();
                inputAction.UI.Enable();
                break;
            case FSM_INPUT.DISABLE_INTERACTIONS:
                inputAction.Player.Fire.Disable();
                inputAction.Player.Move.Disable();
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