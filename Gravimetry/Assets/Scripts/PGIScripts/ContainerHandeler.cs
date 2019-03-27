using System.Collections.Generic;
using UnityEngine;
using PowerGridInventory;



public class ContainerHandeler : MonoBehaviour
{
    public List<PGIModel> models = new List<PGIModel>();
    public List<Vector2> modelStartPositions = new List<Vector2>();
    [HideInInspector]
    public CloseContainerWindow closeContainerWindow;

    private void Start()
    {
        closeContainerWindow = null;
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
