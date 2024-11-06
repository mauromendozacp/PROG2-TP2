using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private Transform bodyTransform = null;
    [SerializeField] private float walkSpeed = 0f;
    [SerializeField] private float runSpeed = 0f;
    [SerializeField] private float turnSmoothVelocity = 0f;
    [SerializeField] private PlayerInventoryController inventoryController = null;
    [SerializeField] private PickItem pickItem = null;

    private PlayerInputController inputController = null;

    private CharacterController character = null;
    private Animator anim = null;

    private Vector3 direction = Vector3.zero;
    private float turnSmoothTime = 0f;
    private float velocityY = 0f;

    private float currentSpeed = 0f;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        inputController = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        currentSpeed = walkSpeed;

        inputController.Init(null, inventoryController.ToggleInventory, PickItem, ToggleRun);
        inventoryController.Init();
    }

    private void Update()
    {
        ApplyGravity();
        Movement();

        UpdateAnimation();
    }

    public void ResetPlayer(Vector3 resetPosition)
    {
        character.enabled = false;

        bodyTransform.SetPositionAndRotation(resetPosition, Quaternion.identity);

        character.enabled = true;
    }

    private void ApplyGravity()
    {
        if (!character.isGrounded)
        {
            velocityY = -Physics.gravity.magnitude * Time.deltaTime;
        }
        else
        {
            velocityY = 0f;
        }
    }

    private void Movement()
    {
        direction = new Vector3(inputController.Move.x, velocityY, inputController.Move.y).normalized;

        if (direction.magnitude > Mathf.Epsilon)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float characterAngle = Mathf.SmoothDampAngle(bodyTransform.eulerAngles.y, targetAngle, ref turnSmoothTime, turnSmoothVelocity);

            bodyTransform.rotation = Quaternion.Euler(0f, characterAngle, 0f);

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

        return inputMove * currentSpeed / runSpeed;
    }

    private void ToggleRun(bool status)
    {
        currentSpeed = status ? runSpeed : walkSpeed;
    }

    private void PickItem()
    {
        ItemData item = pickItem.GetClosestItem();
        if (item != null)
        {
            anim.SetTrigger("PickUp");
            inventoryController.AddNewItem(item);
            pickItem.RemoveDestroyItem(item);
            Destroy(item.gameObject);
        }
    }
}
