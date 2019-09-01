using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerGridInventory;
using UnityEngine.EventSystems;

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
                    if (observer.SubjectComplete(this)) slot.Blocked = false;
                    slot.UpdateSlot();
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

    public int thing;

    PGISlotItem slotItem;
    PGISlot hoverSlot;
    PGIModel _model;

    void Start()
    {
        slotItem = gameObject.GetComponent<PGISlotItem>();

        if (slotItem != null)
        {
            slotItem.OnEquip.AddListener(OnEquip);
            slotItem.OnUnequip.AddListener(OnUnequip);
            slotItem.OnCanEquip.AddListener(OnCanEquip);
        }
    }

    private void FixedUpdate()
    {
        for (int ndx = timers.Count - 1; ndx >= 0; ndx--)
        {
            timers[ndx].FixedUpdate(Time.deltaTime);
        }
        thing = timers.Count;
    }

    public bool SubjectComplete(TimerItem _self)
    {
        timers.Remove(_self);
        
        slotItem.OnCanEquip.Invoke(slotItem, _model, hoverSlot);
        hoverSlot.OnCanEquipItem.Invoke(slotItem, _model, hoverSlot);
        if (slotItem.Equipped < 0)
            hoverSlot.View.TimerHighlightHack(slotItem, hoverSlot);

        foreach (var timer in timers)
        {
            if (timer.slot == _self.slot)
            {
                return false;
            }
        }
        return true;
    }

    public void OnEquip(PGISlotItem item, PGIModel inv, PGISlot slot)
    {
        if (!this.enabled || (slot.gameObject.transform.parent != null && slot.gameObject.transform.parent.gameObject.GetComponent<CloseContainerWindow>()))
            return;

        TimerItem timerItem = new TimerItem(this, slot);

        timers.Add(timerItem);
        timerItem.UpdateEquipTimer(equipLockTime);

        slot.Blocked = true;
        slot.UpdateSlot();
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

    public void OnCanEquip(PGISlotItem slotItem, PGIModel model, PGISlot slot)
    {
        //Debug.Log(gameObject.name + ", " + slot.gameObject.name);
        hoverSlot = slot;
        _model = model;

        if (timers.Count > 0)
        {
            model.CanPerformAction = false;
        }
    }
}
