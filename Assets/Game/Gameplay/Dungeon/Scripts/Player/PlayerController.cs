using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    [Header("General Settings")]
    [SerializeField] private Transform bodyTransform = null;
    [SerializeField] private int maxLife = 0;
    [SerializeField] private float walkSpeed = 0f;
    [SerializeField] private float runSpeed = 0f;
    [SerializeField] private float defenseSpeed = 0f;
    [SerializeField] private float turnSmoothVelocity = 0f;
    [SerializeField] private float aimRotationTime = 0f;
    [SerializeField] private PlayerInventoryController inventoryController = null;
    [SerializeField] private PlayerItemInteraction itemInteraction = null;
    [SerializeField] private PickItem pickItem = null;
    [SerializeField] private AudioEvent deathSound = null;
    [SerializeField] private AudioEvent recieveHitSound = null;
    [SerializeField] private AudioEvent pickUpSound = null;

    private PlayerInputController inputController = null;

    private CharacterController character = null;
    private Animator anim = null;
    private Coroutine aimRotationCoroutine = null;

    private Vector3 direction = Vector3.zero;
    private int currentLife = 0;
    private float turnSmoothTime = 0f;
    private float velocityY = 0f;
    private float currentSpeed = 0f;
    private bool isDefending = false;
    private bool isDead = false;

    private Action onOpenPausePanel = null;
    private Action<int, int> onUpdateLife = null;
    private Action onPlayerDeath = null;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        inputController = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        currentSpeed = walkSpeed;
        currentLife = maxLife;

        inputController.Init(ToggleOnPause, ToggleInventory, PickItem, ToggleRun, inventoryController.ChangeWeapons,
            itemInteraction.PressAction1, itemInteraction.PressAction2, itemInteraction.CancelAction1, itemInteraction.CancelAction2);
        inventoryController.Init();
        itemInteraction.Init(inputController, inventoryController, ToggleDefense, ConsumePotionLife, LookMousePosition);
    }

    private void Update()
    {
        ApplyGravity();
        Movement();

        UpdateAnimation();
    }

    public void Init(Action onOpenPausePanel, Action<int, int> onUpdateLife, Action onPlayerDeath)
    {
        this.onOpenPausePanel = onOpenPausePanel;
        this.onUpdateLife = onUpdateLife;
        this.onPlayerDeath = onPlayerDeath;
    }

    public void ResetPlayer(Vector3 resetPosition)
    {
        character.enabled = false;

        bodyTransform.SetPositionAndRotation(resetPosition, Quaternion.identity);

        character.enabled = true;
    }

    public void TogglePause(bool status)
    {
        inputController.UpdateInputFSM(status ? FSM_INPUT.ONLY_UI : inputController.CurrentInputState, false);
    }

    public void DisableInput()
    {
        inputController.UpdateInputFSM(FSM_INPUT.ONLY_UI);
    }

    public void PlayVictoryAnimation()
    {
        anim.Play("Victory");
    }

    private void ApplyGravity()
    {
        velocityY = !character.isGrounded ? -Physics.gravity.magnitude : 0f;
    }

    private void Movement()
    {
        direction = new Vector3(inputController.Move.x, 0f, inputController.Move.y).normalized;

        if (direction.magnitude > Mathf.Epsilon)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float characterAngle = Mathf.SmoothDampAngle(bodyTransform.eulerAngles.y, targetAngle, ref turnSmoothTime, turnSmoothVelocity);

            bodyTransform.rotation = Quaternion.Euler(0f, characterAngle, 0f);

            direction.y = velocityY;
            character.Move(currentSpeed * Time.deltaTime * direction);
        }
    }

    private void UpdateAnimation()
    {
        anim.SetFloat("Speed", GetMovementSpeed(), 0.05f, Time.deltaTime);
    }

    private float GetMovementSpeed()
    {
        float inputMove = Mathf.Clamp(Mathf.Abs(inputController.Move.x) + Mathf.Abs(inputController.Move.y), 0f, 1f);
        float maxSpeed = isDefending ? defenseSpeed : runSpeed;

        return inputMove * currentSpeed / maxSpeed;
    }

    private void ToggleRun(bool status)
    {
        currentSpeed = status ? runSpeed : walkSpeed;
    }

    private void ToggleDefense(bool status)
    {
        isDefending = status;
        currentSpeed = status ? defenseSpeed : walkSpeed;
    }

    private void PickItem()
    {
        ItemData item = pickItem.GetClosestItem();
        if (item != null)
        {
            anim.SetTrigger("PickUp");
            inventoryController.AddNewItem(item);
            pickItem.RemoveDestroyItem(item);
            GameManager.Instance.AudioManager.PlayAudio(pickUpSound);
            Destroy(item.gameObject);
        }
    }

    private void ToggleInventory()
    {
        inventoryController.ToggleInventory();

        inputController.UpdateInputFSM(inventoryController.IsOpenPanelInventory() ? FSM_INPUT.INVENTORY : FSM_INPUT.ENABLE_ALL);
    }

    private void ToggleOnPause()
    {
        TogglePause(true);
        onOpenPausePanel?.Invoke();
    }

    private void LookMousePosition(Vector3 mousePosition)
    {
        if (aimRotationCoroutine != null)
        {
            StopCoroutine(aimRotationCoroutine);
        }

        IEnumerator AimRotation(Quaternion targetRotation)
        {
            float timer = 0f;
            Quaternion currentRotation = bodyTransform.rotation;
            while (timer < aimRotationTime)
            {
                timer += Time.deltaTime;
                bodyTransform.rotation = Quaternion.Lerp(currentRotation, targetRotation, timer / aimRotationTime);

                yield return new WaitForEndOfFrame();
            }

            bodyTransform.rotation = targetRotation;
        }

        Vector3 dir = (mousePosition - bodyTransform.position).normalized;
        dir.y = 0f;

        aimRotationCoroutine = StartCoroutine(AimRotation(Quaternion.LookRotation(dir)));
    }

    private void ConsumePotionLife(int life)
    {
        currentLife = Mathf.Clamp(currentLife + life, 0, maxLife);
        onUpdateLife?.Invoke(currentLife, maxLife);
    }

    private void Death()
    {
        isDead = true;
        inputController.UpdateInputFSM(FSM_INPUT.ONLY_UI);
        anim.Play("Die");
        GameManager.Instance.AudioManager.PlayAudio(deathSound);

        onPlayerDeath?.Invoke();
    }

    public void Damage(int damageAmount)
    {
        currentLife -= damageAmount;
        if (currentLife <= 0)
        {
            currentLife = 0;
            if (!isDead)
            {
                Death();
            }
        }
        else
        {
            GameManager.Instance.AudioManager.PlayAudio(recieveHitSound);
        }

        onUpdateLife?.Invoke(currentLife, maxLife);
    }
}
