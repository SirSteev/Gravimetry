using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerGridInventory;

public class TimerItem
{
    public float unequipLockTimer;
    public float equipLockTimer;

    public PGISlot slot;

    EquipUnequipTimer observer;

    public TimerItem(EquipUnequipTimer _observer, PGISlot _slot)
    {
        slot = _slot;
        observer = _observer;
    }

    public void UpdateEquipTimer(float _time)
    {
        equipLockTimer = _time;
    }

    public void UpdateUnequipTimer(float _time)
    {
        unequipLockTimer = _time;
    }

    public void FixedUpdate(float _elapsedTime)
    {
        if (slot != null)
        {
            if (equipLockTimer > 0 && slot.Blocked)
            {
                equipLockTimer -= _elapsedTime;

                if (equipLockTimer <= 0)
                {
                    slot.Blocked = false;
                    slot.UpdateSlot();
                }
            }

            if (unequipLockTimer > 0 && slot.Blocked)
            {
                unequipLockTimer -= _elapsedTime;

                if (unequipLockTimer <= 0)
                {
                    slot.Blocked = false;
                    slot.UpdateSlot();

                    observer.SubjectComplete(this);
                }
            }
        }
    }
}
    
public class EquipUnequipTimer : MonoBehaviour
{
    public float equipLockTime;
    
    public float unequipLockTime;
    
    List<TimerItem> timers = new List<TimerItem>();
    
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
        for (int ndx = 0; ndx < timers.Count; ndx++)
        {
            timers[ndx].FixedUpdate(Time.deltaTime);
        }
    }

    public void SubjectComplete(TimerItem _self)
    {
        timers.Remove(_self);
    }

    public void OnEquip(PGISlotItem item, PGIModel inv, PGISlot slot)
    {
        if (!this.enabled || (slot.gameObject.transform.parent != null && slot.gameObject.transform.parent.gameObject.GetComponent<CloseContainerWindow>()))
            return;

        TimerItem timerItem = new TimerItem(this, slot);

        timers.Add(timerItem);

        slot.Blocked = true;

        slot.UpdateSlot();

        timerItem.UpdateEquipTimer(equipLockTime);
    }

    public void OnUnequip(PGISlotItem item, PGIModel inv, PGISlot slot)
    {
        if (!this.enabled || (slot.gameObject.transform.parent != null && slot.gameObject.transform.parent.gameObject.GetComponent<CloseContainerWindow>()))
            return;
        
        foreach (var timer in timers)
        {
            if (timer.slot == slot)
            {
                slot.Blocked = true;
                slot.UpdateSlot();

                timer.UpdateUnequipTimer(unequipLockTime);

                return;
            }
        }

        Debug.Log(slot.gameObject.name + " is missing from list on " + gameObject.name);
    }
}
