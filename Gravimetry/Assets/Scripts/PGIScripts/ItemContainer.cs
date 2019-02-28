using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerGridInventory;

public class ItemContainer : MonoBehaviour
{
    public PGIView containerGridView;
    public PGISlot containerSlot;

    public void OnEquip()
    {
        PGIModel model = containerSlot.Item.gameObject.GetComponent<PGIModel>();
        containerGridView.Model = model;
    }

    public void OnUnEquip()
    {
        containerGridView.Model = null;
    }
}
