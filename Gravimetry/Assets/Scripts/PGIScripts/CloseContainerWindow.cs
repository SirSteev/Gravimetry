using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerGridInventory;
using UnityEngine.UI;

public class CloseContainerWindow : MonoBehaviour
{
    public OpenContainerNewWindow creator;
    public PGISlot slot;
    public RectTransform panelRect;
    public RectTransform windowRect;
    public Vector2 defaultPanelRectSizeDelta;
    public Vector2 defaultWindowRectSizeDelta;
    public Image icon;

    private void Awake()
    {
        defaultPanelRectSizeDelta = panelRect.sizeDelta;
        defaultWindowRectSizeDelta = windowRect.sizeDelta;
    }

    public void CloseContainer()
    {
        Debug.Log("Closeing");
        gameObject.SetActive(false);


        DefaultWindowSizes();
    }

    public void DefaultWindowSizes()
    {
        windowRect.sizeDelta = defaultWindowRectSizeDelta;
        panelRect.sizeDelta = defaultPanelRectSizeDelta;
    }
}
