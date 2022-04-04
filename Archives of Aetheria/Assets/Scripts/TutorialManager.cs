using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    public int popUpindex = 0;

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpindex)
                popUps[popUpindex].SetActive(true);

            else
                popUps[i].SetActive(false);
        }

        if (popUpindex == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                popUpindex++;
        }

        else if (popUpindex == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                popUpindex++;

        }
        else if (popUpindex == 2)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
                popUpindex++;
        }
        else if (popUpindex == 3)
        {
            if (Input.GetMouseButton(0))
                popUpindex++;
        }
        


    }
}
