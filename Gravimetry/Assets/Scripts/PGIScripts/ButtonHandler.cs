using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public List<GameObject> options = new List<GameObject>();
    public List<GameObject> buttons = new List<GameObject>();

    public void ButtonPress(GameObject button)
    {
        if (buttons.Contains(button))
        {
            int ndx = buttons.IndexOf(button);

            foreach (var frame in options)
            {
                frame.SetActive(false);
            }

            options[ndx].SetActive(true);

            buttons[ndx].transform.SetSiblingIndex(buttons.Count - 1);
        }
        else
        {
            Debug.Log("BUTTON MISSING FROM LIST");
        }
    }
}
