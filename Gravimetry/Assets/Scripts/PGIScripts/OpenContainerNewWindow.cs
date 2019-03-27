using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using PowerGridInventory;

public class OpenContainerNewWindow : MonoBehaviour
{
    int clicks;
    float timer;
    float maxTimer = 0.5f;

    public GameObject openContainerPreFab;
    public GameObject playerInventoryObject;
    
    RectTransform panelRect;
    RectTransform canvasRect;

    public int windowCount = 2;
    ContainerHandeler[] containerHandelers;
    ItemContainer[] itemContainers;
    GameObject[] openContainers;
    GameObject[] openContainerWindows;
    int containerNdx = 0;
    PGISlot _slot;

    private void Start()
    {
        openContainers = new GameObject[windowCount];
        openContainerWindows = new GameObject[windowCount];
        containerHandelers = new ContainerHandeler[windowCount];
        itemContainers = new ItemContainer[windowCount];

        GameObject canvas = gameObject;
        while (canvas.name != "Canvas")
        {
            canvas = canvas.transform.parent.gameObject;
        }

        canvasRect = canvas.GetComponent<RectTransform>();
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
        if (eventData.button != PointerEventData.InputButton.Left) return;
        
        if (slot.Item != null && slot.Item.gameObject.transform.parent != null && slot.Item.gameObject.transform.parent.gameObject.GetComponent<ContainerHandeler>() != null)
        {
            Debug.Log("IM IN A BOX NO CAN OPEN");
            return;
        }

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

                    openContainerWindows[ndx].transform.SetSiblingIndex(openContainerWindows[ndx].transform.parent.transform.childCount - 1);

                    if (!openContainerWindows[ndx].activeInHierarchy)
                    {
                        Debug.Log("CONTAINER WINDOW WAS CLOSED");

                        CloseContainerWindow closeContainerWindow = openContainerWindows[ndx].GetComponent<CloseContainerWindow>();

                        // resize window
                        closeContainerWindow.DefaultWindowSizes();

                        RectTransform containerRect = openContainerWindows[ndx].GetComponent<RectTransform>();
                        containerRect.sizeDelta += itemContainers[ndx].sizeDelta;

                        panelRect = closeContainerWindow.panelRect;
                        panelRect.sizeDelta += new Vector2(itemContainers[ndx].sizeDelta.x, 0);

                        panelRect.gameObject.GetComponent<DragPanel>().constraintsTransform = canvasRect;

                        containerHandelers[ndx].closeContainerWindow = closeContainerWindow;
                        
                        openContainerWindows[ndx].SetActive(true);
                        closeContainerWindow.icon.sprite = null;
                        closeContainerWindow.icon.sprite = slot.Item.Icon;
                    }
                }
                else
                {
                    Debug.Log("WORKS");

                    if (openContainerWindows[containerNdx] != null)
                    {
                        for (int i = 0; i < openContainerWindows.Length; i++)
                        {
                            if (!openContainerWindows[i].activeInHierarchy)
                            {
                                containerNdx = i;
                                break;
                            }
                        }
                    }

                    _slot = slot;
                    
                    openContainers[containerNdx] = _slot.Item.gameObject;

                    if (openContainerWindows[containerNdx] == null)
                        openContainerWindows[containerNdx] = Instantiate(openContainerPreFab, playerInventoryObject.transform.parent.transform);
                    
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
        if (containerNdx == windowCount) containerNdx = 0;
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

        containerHandelers[containerNdx] = _slot.Item.gameObject.GetComponent<ContainerHandeler>();
        itemContainers[containerNdx] = closeContainerWindow.slot.gameObject.GetComponent<ItemContainer>();

        containerHandelers[containerNdx].closeContainerWindow = closeContainerWindow;

        // resize window
        closeContainerWindow.DefaultWindowSizes();
        
        closeContainerWindow.windowRect.sizeDelta += itemContainers[containerNdx].sizeDelta;
        //closeContainerWindow.windowRect.sizeDelta -= new Vector2(0, 50);
        
        panelRect = openContainerWindows[containerNdx].GetComponent<CloseContainerWindow>().panelRect;
        panelRect.sizeDelta += new Vector2(itemContainers[containerNdx].sizeDelta.x, 0);

        panelRect.gameObject.GetComponent<DragPanel>().constraintsTransform = canvasRect;

        if (!openContainerWindows[containerNdx].activeInHierarchy)
            openContainerWindows[containerNdx].SetActive(true);

        closeContainerWindow.icon.sprite = null;
        closeContainerWindow.icon.sprite = _slot.Item.Icon;

        openContainerWindows[containerNdx].transform.SetSiblingIndex(0);
        openContainerWindows[containerNdx].transform.SetSiblingIndex(openContainerWindows[containerNdx].transform.parent.transform.childCount - 1);

        IncrementNdx();
    }
}
