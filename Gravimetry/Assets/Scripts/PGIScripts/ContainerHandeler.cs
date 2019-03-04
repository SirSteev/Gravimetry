using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerGridInventory;
public class ContainerHandeler : MonoBehaviour
{
    public List<PGIModel> models = new List<PGIModel>();
    public List<Vector2> modelStartPositions = new List<Vector2>();

    public float GetWidth(float _slotScale, float _slotSpacing)
    {
        float width = 0;

        float largestX = -1;

        for (int ndx = 0; ndx < modelStartPositions.Count; ndx++)
        {
            if (modelStartPositions[ndx].x > largestX)
                largestX = modelStartPositions[ndx].x;
        }

        width = largestX * _slotScale + models[(int)largestX].GridCellsX * _slotScale + (largestX - 1) * _slotSpacing;

        return width;
    }

    public float GetHeight(float _slotScale, float _slotSpacing)
    {
        float height = 0;

        float largestY = -1;
        float largestHeight = -1;

        for (int ndx = 0; ndx < modelStartPositions.Count; ndx++)
        {
            if (modelStartPositions[ndx].y > largestY)
                largestY = modelStartPositions[ndx].y;
        }

        for (int ndx = 0; ndx < modelStartPositions.Count; ndx++)
        {
            if (modelStartPositions[ndx].y == largestY && largestHeight < models[ndx].GridCellsY * _slotScale)
                largestHeight = models[ndx].GridCellsY * _slotScale;
        }

        height = largestY * _slotScale + models[(int)largestY].GridCellsY * _slotScale + (largestY - 1) * _slotSpacing;

        return height;
    }
}
