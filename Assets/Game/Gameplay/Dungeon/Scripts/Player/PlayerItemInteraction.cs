using System;

using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{
    private Animator anim = null;
    private Equipment equipment = null;
    private PlayerInputController inputController = null;

    private Action<bool> onToggleDefense = null;
    private Action<int> onConsumeLife = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(Equipment equipment, PlayerInputController inputController, Action<bool> onToggleDefense, Action<int> onConsumeLife)
    {
        this.equipment = equipment;
        this.inputController = inputController;

        this.onToggleDefense = onToggleDefense;
        this.onConsumeLife = onConsumeLife;
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
                        ToggleOnInteractionInput();
                        break;
                    case WeaponType.Wand:
                        anim.SetTrigger("AttackWand");
                        ToggleOnInteractionInput();
                        break;
                    case WeaponType.Bow:
                        break;
                }
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
                }
            }
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
                ToggleOnInteractionInput();
            }
            else if (item is Consumible consumible)
            {
                anim.SetTrigger("ConsumePotion");
                onConsumeLife?.Invoke(consumible.amount);
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

    public void SpawnProjectile()
    {

    }

    public void BackToCurrentInput()
    {
        inputController.UpdateInputFSM(inputController.CurrentInputState);
    }

    private void ToggleOnInteractionInput()
    {
        //inputController.UpdateInputFSM(FSM_INPUT.INTERACTING, false);
    }

    private Item GetItemByEquipmentIndex(int index)
    {
        int itemId = equipment.GetID(index);
        return ItemManager.Instance.GetItemFromID(itemId);
    }
}
