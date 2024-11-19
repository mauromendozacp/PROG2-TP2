using System;

using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{
    [SerializeField] private ProjectilePoolController projectilePoolController = null;
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private LayerMask floorLayer = default;
    [SerializeField] private AttackItem leftHandItem = null;
    [SerializeField] private AttackItem rightHandItem = null;
    [SerializeField] private AudioEvent swordSound = null;
    [SerializeField] private AudioEvent potionSound = null;
    [SerializeField] private AudioEvent arrowSound = null;

    private Animator anim = null;
    private PlayerInputController inputController = null;
    private PlayerInventoryController inventoryController = null;

    private Weapon currentWeapon = null;
    private Projectile currentProjectile = null;
    private Vector3 targetPosition = Vector3.zero;


    private Action<bool> onToggleDefense = null;
    private Action<int> onConsumeLife = null;
    private Action<Vector3> onLookMousePosition = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(PlayerInputController inputController, PlayerInventoryController inventoryController, Action<bool> onToggleDefense, Action<int> onConsumeLife, Action<Vector3> onLookMousePosition)
    {
        this.inputController = inputController;
        this.inventoryController = inventoryController;

        this.onToggleDefense = onToggleDefense;
        this.onConsumeLife = onConsumeLife;
        this.onLookMousePosition = onLookMousePosition;
    }

    public void PressAction1()
    {
        Item item = GetItemByEquipmentIndex(1);
        if (item != null)
        {
            if (item is Weapon weapon)
            {
                switch (weapon.type)
                {
                    case WeaponType.Sword:
                        anim.SetTrigger("AttackSword");
                        rightHandItem.SetDamage(weapon.damage);
                        GameManager.Instance.AudioManager.PlayAudio(swordSound);
                        ToggleOnInteractionInput();
                        break;
                    case WeaponType.Wand:
                        anim.SetTrigger("AttackWand");
                        targetPosition = GetMouseWorldPosition();

                        ToggleOnInteractionInput();
                        break;
                    case WeaponType.Bow:
                        break;
                }

                currentWeapon = weapon;
            }
            else if (item is Projectile projectile)
            {
                Item item2 = GetItemByEquipmentIndex(0);
                if (item2 is Weapon weapon2)
                {
                    switch (weapon2.type)
                    {
                        case WeaponType.Bow:
                            anim.SetTrigger("AttackBow");
                            ToggleOnInteractionInput();
                            break;
                    }

                    currentWeapon = weapon2;
                }

                currentProjectile = projectile;
            }

            onLookMousePosition?.Invoke(GetMouseWorldPosition());
        }
    }

    public void PressAction2()
    {
        Item item = GetItemByEquipmentIndex(0);
        if (item != null)
        {
            if (item is Shield shield)
            {
                onToggleDefense?.Invoke(true);
                anim.SetBool("Defense", true);
            }
            else if (item is Consumible consumible)
            {
                anim.SetTrigger("ConsumePotion");
                onConsumeLife?.Invoke(consumible.amount);
                inventoryController.ConsumeEquipmentItem(0);
                GameManager.Instance.AudioManager.PlayAudio(potionSound);
                ToggleOnInteractionInput();
            }
        }
    }

    public void CancelAction1()
    {

    }

    public void CancelAction2()
    {
        Item item = GetItemByEquipmentIndex(0);
        if (item != null)
        {
            if (item is Shield shield)
            {
                onToggleDefense?.Invoke(false);
                anim.SetBool("Defense", false);
            }
        }
    }

    public void AttackSword()
    {
        rightHandItem.ToggleCollider(true);
    }

    public void SpawnProjectile()
    {
        switch (currentWeapon.type)
        {
            case WeaponType.Wand:
                MeteorsProjectile meteors = projectilePoolController.GetProjectileItem(PROJECTILE_TYPE.METEORS) as MeteorsProjectile;
                meteors.transform.position = targetPosition;
                meteors.SetDamage(currentWeapon.damage);

                break;
            case WeaponType.Bow:
                ArrowProjectile arrow = projectilePoolController.GetProjectileItem(PROJECTILE_TYPE.ARROW) as ArrowProjectile;
                arrow.transform.position = rightHandItem.transform.position;
                arrow.transform.rotation = transform.rotation;
                arrow.SetDamage(currentWeapon.damage);
                arrow.FireArrow(currentWeapon.speed, transform.forward);
                arrow.SetMesh(currentProjectile.mesh);
                GameManager.Instance.AudioManager.PlayAudio(arrowSound);

                inventoryController.ConsumeEquipmentItem(1);

                break;
        }
    }

    public void BackToCurrentInput()
    {
        inputController.UpdateInputFSM(inputController.CurrentInputState);

        leftHandItem.ToggleCollider(false);
        rightHandItem.ToggleCollider(false);
    }

    public void ToggleOnInteractionInput()
    {
        inputController.UpdateInputFSM(FSM_INPUT.INTERACTING, false);
    }

    private Item GetItemByEquipmentIndex(int index)
    {
        int itemId = inventoryController.Equipment.GetID(index);
        return ItemManager.Instance.GetItemFromID(itemId);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorLayer))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}
