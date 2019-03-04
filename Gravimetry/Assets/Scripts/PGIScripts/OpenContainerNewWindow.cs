using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using PowerGridInventory;

public class OpenContainerNewWindow : MonoBehaviour
{
    public int clicks;
    public float timer;
    public float maxTimer = 0.5f;

    public GameObject openContainerPreFab;
    public GameObject playerInventoryObject;

    ContainerHandeler containerHandeler;
    ItemContainer itemContainer;
    RectTransform panelRect;

    const int CONTAINERCOUNT = 2;
    public GameObject[] openContainers;
    public GameObject[] openContainerWindows;
    int containerNdx = 0;
    PGISlot _slot;

    private void Start()
    {
        openContainers = new GameObject[CONTAINERCOUNT];
        openContainerWindows = new GameObject[CONTAINERCOUNT];
    }

    private void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        else if (clicks != 0)
        {
            clicks = 0;
        }
    }
    
    public void OpenContainer(PointerEventData eventData, PGISlot slot)
    {
        Debug.Log("CLICK");
        timer = maxTimer;

        if (eventData.button == PointerEventData.InputButton.Left) clicks++;

        if (clicks >= 2)
        {
            if (slot.Item.gameObject.GetComponent<ContainerHandeler>() != null)
            {
                int ndx = -1;
                for (int i = 0; i < openContainerWindows.Length; i++)
                {
                    if (openContainers[i] == slot.Item.gameObject) ndx = i;
                }

                if (ndx != -1)
                {
                    Debug.Log("CONTAINER ALLREADY OPEN");

                    if (!openContainerWindows[ndx].activeInHierarchy)
                    {
                        Debug.Log("CONTAINER WINDOW WAS CLOSED");

                        // resize window
                        RectTransform containerRect = openContainerWindows[ndx].GetComponent<RectTransform>();
                        containerRect.sizeDelta += itemContainer.sizeDelta;
                        containerRect.sizeDelta -= new Vector2(0, 50);

                        panelRect = openContainerWindows[ndx].GetComponent<CloseContainerWindow>().panelRect;
                        panelRect.sizeDelta += new Vector2(itemContainer.sizeDelta.x, 0);

                        openContainerWindows[ndx].SetActive(true);
                        openContainerWindows[ndx].GetComponent<CloseContainerWindow>().icon.sprite = null;
                        openContainerWindows[ndx].GetComponent<CloseContainerWindow>().icon.sprite = slot.Item.Icon;
                    }
                }
                else
                {
                    Debug.Log("WORKS");
                    _slot = slot;
                    
                    openContainers[containerNdx] = _slot.Item.gameObject;
                    if (openContainerWindows[containerNdx] == null)
                        openContainerWindows[containerNdx] = Instantiate(openContainerPreFab, playerInventoryObject.transform);

                    
                    Invoke("EquipItem", 0.1f);

                    CloseContainerWindow thing = openContainerWindows[containerNdx].GetComponent<CloseContainerWindow>();
                    if (thing == null)
                    {
                        Debug.Log("CloseContainerWindow missing!!");
                    }
                    else
                    {
                        thing.creator = this;
                    }
                }
            }
            else
            {
                Debug.Log("NOT A CONTAINER");
            }
        }
    }

    void IncrementNdx()
    {
        containerNdx++;
        if (containerNdx == CONTAINERCOUNT) containerNdx = 0;
    }

    void EquipItem()
    {
        Debug.Log(_slot.Item.gameObject.name);

        CloseContainerWindow closeContainerWindow = openContainerWindows[containerNdx].GetComponent<CloseContainerWindow>();

        PGISlot windowSlot = closeContainerWindow.slot;
        while (windowSlot.Item != _slot.Item)
        {
            windowSlot.AssignItem(_slot.Item);
        }

        windowSlot.Item.TriggerEquipEvents(_slot.Model, windowSlot);

        containerHandeler = _slot.Item.gameObject.GetComponent<ContainerHandeler>();
        itemContainer = closeContainerWindow.slot.gameObject.GetComponent<ItemContainer>();

        // resize window
        closeContainerWindow.DefaultWindowSizes();
        
        closeContainerWindow.windowRect.sizeDelta += itemContainer.sizeDelta;
        closeContainerWindow.windowRect.sizeDelta -= new Vector2(0, 50);
        
        panelRect = openContainerWindows[containerNdx].GetComponent<CloseContainerWindow>().panelRect;
        panelRect.sizeDelta += new Vector2(itemContainer.sizeDelta.x, 0);

        closeContainerWindow.icon.sprite = null;
        closeContainerWindow.icon.sprite = _slot.Item.Icon;

        IncrementNdx();
    }
}
