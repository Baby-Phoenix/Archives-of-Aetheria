using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Player player;
    public LayerMask whatisTutorial, whatisBridge;
    public GameObject[] popUps;
    public GameObject[] checkPoints;
    public int popUpindex = 0, previousIndex = 0;
    public bool isPlayerInTutorialGrounds;
    public bool isPlayerInNextPart;

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

        isPlayerInTutorialGrounds = Physics.Raycast(player.transform.position, -transform.up, 2f, whatisTutorial);
        isPlayerInNextPart = Physics.Raycast(player.transform.position, -transform.up, 2f, whatisBridge);

        if (popUpindex == 0)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                popUpindex++;

            //this else makes sure that players dont see a pop up until they are in tutorial grounds
            else if (!isPlayerInTutorialGrounds)
            {
                previousIndex = popUpindex;
                popUpindex = -1;
            }
        }

        else if (popUpindex == 1)
        {
            if (!isPlayerInTutorialGrounds && isPlayerInNextPart)
            {
                popUpindex++;
                previousIndex = popUpindex;
                popUpindex = -1;
            }
        }

        else if (popUpindex == 2)
        {
            if (!isPlayerInTutorialGrounds && isPlayerInNextPart)
            {
                popUpindex++;
                previousIndex = popUpindex;
                popUpindex = -1;
            }
        }
        else if (popUpindex == 3)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) ;
            //popUpindex++;

            else if (!isPlayerInTutorialGrounds)
            {
                previousIndex = popUpindex;
                popUpindex = -1;
            }
        }

        //if the player is back on tutorial grounds set the popUpindex back
        else if (popUpindex == -1 && isPlayerInTutorialGrounds)
        {
            popUpindex = previousIndex;
            previousIndex = 0;
        }

    }
}
