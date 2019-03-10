using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerGridInventory;

public class DisableSlotIfEmpty : MonoBehaviour
{
    public List<PGISlot> disableSlots = new List<PGISlot>();
    PGISlot _slot;

    void Start()
    {
        _slot = gameObject.GetComponent<PGISlot>();

        if (_slot == null)
        {
            Debug.Log(gameObject.name + " missing PGISlot Component.");
            return;
        }
        else
        {
            _slot.OnEquipItem.AddListener(OnEquip);
            _slot.OnUnequipItem.AddListener(OnUnequip);
        }

        if (_slot.Item == null)
        {
            foreach (var item in disableSlots)
            {
                item.Blocked = true;
            }
        }
    }

    public void OnEquip(PGISlotItem item, PGIModel inv, PGISlot slot)
    {
        foreach (var dis in disableSlots)
        {
            dis.Blocked = false;
        }
    }

    public void OnUnequip(PGISlotItem item, PGIModel inv, PGISlot slot)
    {
        foreach (var dis in disableSlots)
        {
            dis.Blocked = true;
        }
    }
   
}
