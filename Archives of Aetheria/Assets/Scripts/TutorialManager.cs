using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public PlayerMove player;
    public LayerMask whatisTutorial;
    public GameObject[] popUps;
    public GameObject[] checkPoints;
    public int popUpindex = 0, previousIndex = 0;
    public bool isPlayerInTutorialGrounds;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            player.healthBar.health--;
            player.healthBar.UpdateHealth();
        }

        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpindex)
                popUps[popUpindex].SetActive(true);

            else
                popUps[i].SetActive(false);
        }

        isPlayerInTutorialGrounds = Physics.Raycast(player.transform.position, -transform.up, 2f, whatisTutorial);

        if (popUpindex == 0)
        {
            if (Input.GetMouseButton(0))
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
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                popUpindex++;

            else if (!isPlayerInTutorialGrounds)
            {
                previousIndex = popUpindex;
                popUpindex = -1;
            }
        }

        else if (popUpindex == 2)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                popUpindex++;

            else if (!isPlayerInTutorialGrounds)
            {
                previousIndex = popUpindex;
                popUpindex = -1;
            }
        }
        else if (popUpindex == 3)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
                popUpindex++;

            else if (!isPlayerInTutorialGrounds)
            {
                previousIndex = popUpindex;
                popUpindex = -1;
            }
        }

        //if the player is back on tutorial grounds set the popUpindex back
        else if (popUpindex == -1 &&  isPlayerInTutorialGrounds) 
        {
            popUpindex = previousIndex;
            previousIndex = 0;
        }

    }
}
