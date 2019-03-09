using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerGridInventory;

public class EquipUnequipTimer : MonoBehaviour
{
    public float equipLockTime;
    float equipLockTimer;

    public float unequipLockTime;
    float unequipLockTimer;

    public PGISlot _slot;

    void Start()
    {
        PGISlotItem slotItem = gameObject.GetComponent<PGISlotItem>();
        if (slotItem != null)
        {
            slotItem.OnEquip.AddListener(OnEquip);
            slotItem.OnUnequip.AddListener(OnUnequip);
        }
    }

    private void FixedUpdate()
    {
        if (_slot != null)
        {
            if (equipLockTimer > 0 && _slot.Blocked)
            {
                equipLockTimer -= Time.deltaTime;

                if (equipLockTimer <= 0)
                {
                    _slot.Blocked = false;
                    _slot.UpdateSlot();
                }
            }

            if (unequipLockTimer > 0 && _slot.Blocked)
            {
                unequipLockTimer -= Time.deltaTime;

                if (unequipLockTimer <= 0)
                {
                    _slot.Blocked = false;
                    _slot.UpdateSlot();
                    _slot = null;
                }
            }
        }
    }

    public void OnEquip(PGISlotItem item, PGIModel inv, PGISlot slot)
    {
        if (!this.enabled || (slot.gameObject.transform.parent != null && slot.gameObject.transform.parent.gameObject.GetComponent<CloseContainerWindow>()))
            return;

        _slot = slot;

        _slot.Blocked = true;

        _slot.UpdateSlot();

        equipLockTimer = equipLockTime;
    }

    public void OnUnequip(PGISlotItem item, PGIModel inv, PGISlot slot)
    {
        if (!this.enabled || (slot.gameObject.transform.parent != null && slot.gameObject.transform.parent.gameObject.GetComponent<CloseContainerWindow>()))
            return;

        _slot = slot;

        _slot.Blocked = true;

        _slot.UpdateSlot();
        
        unequipLockTimer = unequipLockTime;
    }
}
