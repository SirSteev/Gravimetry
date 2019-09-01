using System.Collections.Generic;
using UnityEngine;
using PowerGridInventory;



public class ContainerHandeler : MonoBehaviour
{
    [HideInInspector]
    public List<PGIModel> models = new List<PGIModel>();

    public List<Vector2> modelStartPositions = new List<Vector2>();
    public List<Vector2> modelSizes = new List<Vector2>();

    [HideInInspector]
    public CloseContainerWindow closeContainerWindow;

    private void Start()
    {
        closeContainerWindow = null;

        if (modelSizes.Count < modelStartPositions.Count) Debug.Log("Model Sizes count on " + gameObject.name + " is less than Model Start Positions count, using last model size for remainder.");
        
        if (modelStartPositions.Count >= modelSizes.Count)
        {
            for (int i = 0; i < modelStartPositions.Count; i++)
            {
                models.Add(gameObject.AddComponent<PGIModel>());

                if (i < modelSizes.Count)
                {
                    models[i].GridCellsX = (int)modelSizes[i].x;
                    models[i].GridCellsY = (int)modelSizes[i].y;
                }
                else
                {
                    models[i].GridCellsX = (int)modelSizes[modelSizes.Count - 1].x;
                    models[i].GridCellsY = (int)modelSizes[modelSizes.Count - 1].y;
                }
            }
        }
        else
        {
            Debug.LogError("Model Start Positions is less than Model Sizes on " + gameObject.name + " please fix.");
        }
    }

    private void Update()
    {
        if (transform.parent != null)
        {
            if (closeContainerWindow != null && transform.parent.gameObject.GetComponent<ContainerHandeler>() != null)
            {
                closeContainerWindow.CloseContainer();
                closeContainerWindow = null;
            }
        }
    }
}
