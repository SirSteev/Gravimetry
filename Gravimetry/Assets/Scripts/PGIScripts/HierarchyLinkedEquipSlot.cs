using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerGridInventory;
using PowerGridInventory.Extensions.ItemFilter;

public class HierarchyLinkedEquipSlot : MonoBehaviour
{
    [Tooltip("A list of equipment slots that will become blocked when this slot is equipped with an item.")]
    public PGISlot[] LowerLinkedSlots;
    public bool toggleAll = false;

    void Start()
    {
        PGISlot slot = GetComponent<PGISlot>();
        if (slot != null)
        {
            slot.OnEquipItem.AddListener(OnEquip);
            slot.OnUnequipItem.AddListener(OnUnequip);
        }
    }

    public void OnEquip(PGISlotItem item, PGIModel inv, PGISlot slot)
    {
        if (!this.enabled) return;

        //Check to see if the equipped item has an ItemType component
        ItemType type = item.GetComponent<ItemType>();
        if (type != null)//type.TypeName.Equals("Two-handed Weapon"))
        {
            //It does. So we need to block
            if (LowerLinkedSlots != null)
            {
                foreach (PGISlot linked in LowerLinkedSlots)
                {
                    linked.Blocked = true;

                    //HACK ALERT:
                    //This is a work-around for a bug introduced with the advent of 3D mesh icons.
                    //This simply ensures the linked slot's default icon is restored as it should be.
                    linked.gameObject.SetActive(false);
                    linked.gameObject.SetActive(true);
                }
            }
        }
    }

    public void OnUnequip(PGISlotItem item, PGIModel inv, PGISlot slot)
    {
        if (!this.enabled) return;

        if (LowerLinkedSlots != null)
        {
            foreach (PGISlot linked in LowerLinkedSlots)
            {
                //Warning, we are making the assumption that nothing else
                //had previously blocked this slot.

                //HACK ALERT: We need to check for Blocked stat before changing it here
                //due to the changes made for the 3D icon system and the highlight colors
                //used by items when equipped to slots.
                if (linked.Blocked) linked.Blocked = false;

                if (linked.Item != null && !toggleAll) break;
            }
        }
    }
}
