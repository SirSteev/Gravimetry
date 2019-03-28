using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PowerGridInventory;

public class ItemContainer : MonoBehaviour
{
    public GameObject containerGridFramesObject;
    public PGISlot containerSlot;
    public ContainerHandeler containerHandeler;
    
    public GameObject gridFramePrefab;

    List<GameObject> gridFrames = new List<GameObject>();
    List<RectTransform> gridFrameRectTransforms = new List<RectTransform>();
    List<AutoSquareSlots> autoSquareSlots = new List<AutoSquareSlots>();
    List<PGIView> gridPGIViews = new List<PGIView>();

    public float slotScale = 25f;
    public float slotSpacing = 0.5f;
    

    public Vector2 sizeDelta = Vector2.zero;
    //bool sizeDeltaSet = false;
    

    public void OnEquip()
    {
        //Debug.Log("TRIGGERED");

        containerHandeler = containerSlot.Item.gameObject.GetComponent<ContainerHandeler>();

        if (containerHandeler == null)
        {
            Debug.Log("NULL Container.... its not one");
            return;
        }

        //containerGridFramesObject.GetComponent<RectTransform>().sizeDelta = new Vector2(containerHandeler.GetWidth(slotScale, slotSpacing), containerHandeler.GetHeight(slotScale, slotSpacing));

        for (int ndx = 0; ndx < gridFrames.Count; ndx++)
        {
            gridFrames[ndx].SetActive(false);
        }

        for (int ndx = 0; ndx < containerHandeler.models.Count; ndx++)
        {
            if (ndx >= gridFrames.Count)
            { 
                gridFrames.Add(Instantiate(gridFramePrefab, containerGridFramesObject.transform));
                gridFrameRectTransforms.Add(gridFrames[ndx].GetComponent<RectTransform>());
                autoSquareSlots.Add(gridFrames[ndx].GetComponent<AutoSquareSlots>());
                gridPGIViews.Add(autoSquareSlots[ndx].View);
            }

            gridFrames[ndx].SetActive(true);
            gridPGIViews[ndx].Model = containerHandeler.models[ndx];
            
            gridFrameRectTransforms[ndx].sizeDelta = new Vector2(containerHandeler.models[ndx].GridCellsX * slotScale, containerHandeler.models[ndx].GridCellsY * slotScale);
            gridFrameRectTransforms[ndx].anchoredPosition = new Vector2((containerHandeler.modelStartPositions[ndx].x * (slotScale + slotSpacing)) + gridFrameRectTransforms[ndx].sizeDelta.x / 2,
                                                                       -((containerHandeler.modelStartPositions[ndx].y * (slotScale + slotSpacing)) + gridFrameRectTransforms[ndx].sizeDelta.y / 2));

            Vector2 temp = new Vector2(containerHandeler.models[ndx].GridCellsX, containerHandeler.models[ndx].GridCellsY) * slotScale + containerHandeler.modelStartPositions[ndx] * slotScale;

            if (temp.x > sizeDelta.x) sizeDelta.x = temp.x;
            if (temp.y > sizeDelta.y) sizeDelta.y = temp.y;

            autoSquareSlots[ndx].UpdateView();
            containerSlot.UpdateSlot();
        }
        sizeDelta -= new Vector2(0, 50);

        if (sizeDelta.y < 0) sizeDelta.y = 0;
    }

    public void OnUnEquip()
    {
        if (containerHandeler == null)
        {
            Debug.Log("NULL Container.... its not one");
            return;
        }

        containerGridFramesObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        for (int ndx = 0; ndx < containerHandeler.models.Count; ndx++)
        {
            gridFrameRectTransforms[ndx].sizeDelta = Vector2.zero;
            gridFrameRectTransforms[ndx].anchoredPosition = Vector2.zero;

            gridPGIViews[ndx].Model = null;
        }
        containerHandeler = null;
        
        containerSlot.UpdateSlot();
    }
}
