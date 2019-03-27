using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public List<GameObject> buttons = new List<GameObject>();

    public List<GameObject> options = new List<GameObject>();
    public List<GameObject> optionalOptions = new List<GameObject>();

    public bool bringButtonToFront;
    
    public void ButtonPress(GameObject button)
    {
        //Debug.Log(button.name + " was pressed");

        if (buttons.Contains(button))
        {
            int ndx = buttons.IndexOf(button);

            if (options[ndx].activeInHierarchy) return;

            foreach (var option in options)
            {
                option.SetActive(false);
            }

            foreach (var frame in optionalOptions)
            {
                frame.SetActive(false);
            }

            options[ndx].SetActive(true);
            if (ndx < optionalOptions.Count)
                optionalOptions[ndx].SetActive(true);

            if (bringButtonToFront) buttons[ndx].transform.SetSiblingIndex(buttons.Count - 1);
        }
        else
        {
            Debug.Log("BUTTON MISSING FROM LIST");
        }
    }
}
