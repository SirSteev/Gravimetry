using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public float lockTimerMax;
    public float lockTimer;

    public Vector2 sizeDelta;
    bool sizeDeltaSet = false;

    public void FixedUpdate()
    {
        if (containerSlot.Blocked)
        {
            lockTimer -= Time.deltaTime;

            if (lockTimer <= 0)
            {
                containerSlot.Blocked = false;
                containerSlot.UpdateSlot();
            }
        }
    }

    

    public void OnEquip()
    {
        Debug.Log("TRIGGERED");

        containerHandeler = containerSlot.Item.gameObject.GetComponent<ContainerHandeler>();

        //containerGridFramesObject.GetComponent<RectTransform>().sizeDelta = new Vector2(containerHandeler.GetWidth(slotScale, slotSpacing), containerHandeler.GetHeight(slotScale, slotSpacing));

        for (int ndx = 0; ndx < containerHandeler.models.Count; ndx++)
        {
            if (ndx >= gridFrames.Count)
            { 
                gridFrames.Add(Instantiate(gridFramePrefab, containerGridFramesObject.transform));
                gridFrameRectTransforms.Add(gridFrames[ndx].GetComponent<RectTransform>());
                autoSquareSlots.Add(gridFrames[ndx].GetComponent<AutoSquareSlots>());
                gridPGIViews.Add(autoSquareSlots[ndx].View);
            }

            gridPGIViews[ndx].Model = containerHandeler.models[ndx];

            Vector2 tempDelta;
            //------
            gridFrameRectTransforms[ndx].sizeDelta = tempDelta = new Vector2(containerHandeler.models[ndx].GridCellsX * slotScale, containerHandeler.models[ndx].GridCellsY * slotScale);
            gridFrameRectTransforms[ndx].anchoredPosition = new Vector2((containerHandeler.modelStartPositions[ndx].x * (slotScale + slotSpacing)) + gridFrameRectTransforms[ndx].sizeDelta.x / 2,
                                                                       -((containerHandeler.modelStartPositions[ndx].y * (slotScale + slotSpacing)) + gridFrameRectTransforms[ndx].sizeDelta.y / 2));
            if (!sizeDeltaSet) sizeDelta += tempDelta;
            //------
            
            autoSquareSlots[ndx].UpdateView();

            gridPGIViews[ndx].DisableRendering = true;
            gridPGIViews[ndx].DisableRendering = false;
        }
        sizeDeltaSet = true;
    }

    public void OnUnEquip()
    {
        containerGridFramesObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        for (int ndx = 0; ndx < containerHandeler.models.Count; ndx++)
        {
            gridFrameRectTransforms[ndx].sizeDelta = Vector2.zero;
            gridFrameRectTransforms[ndx].anchoredPosition = Vector2.zero;

            gridPGIViews[ndx].Model = null;
        }
        containerHandeler = null;

        containerSlot.Blocked = true;
        lockTimer = lockTimerMax;
    }
}
