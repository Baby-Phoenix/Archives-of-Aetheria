using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Player player;
    public LayerMask whatisTutorial, whatisBridge, whatisDeath, whatisTemple;
    public GameObject[] popUps;
    public GameObject[] respawnAnchors;
    public GameObject currentRespawnAnchors;
    public int popUpindex = 0, previousIndex = 0;
    public bool isPlayerInTutorialGrounds;
    public bool isPlayerInNextPart, isPlayerDead;

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
        isPlayerDead = Physics.Raycast(player.transform.position, -transform.up, 2f, whatisDeath);

        if (isPlayerDead)
        {
            //do vignette
            player.gameObject.transform.position = respawnAnchors[previousIndex].transform.position;
            player.healthBar.SetMaxValue(10f);
        }


        if (popUpindex == 0)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                popUpindex++;

            //this else makes sure that players dont see a pop up until they are in tutorial grounds
            else if (!isPlayerInTutorialGrounds)
            {
                previousIndex = popUpindex;
                currentRespawnAnchors = respawnAnchors[previousIndex];
                popUpindex = -1;
            }
        }

        else if (popUpindex == 1)
        {
            if (!isPlayerInTutorialGrounds && isPlayerInNextPart)
            {
                popUpindex++;
                previousIndex = popUpindex;
                currentRespawnAnchors = respawnAnchors[previousIndex];
                popUpindex = -1;
            }
        }

        else if (popUpindex == 2)
        {

            if (!isPlayerInTutorialGrounds && isPlayerInNextPart)
            {
                popUpindex++;
                previousIndex = popUpindex;
                currentRespawnAnchors = respawnAnchors[previousIndex];
                popUpindex = -1;
            }
        }
        else if (popUpindex == 3)
        {
            if (!isPlayerInNextPart)
            {
                popUpindex++;
                previousIndex = popUpindex;
                currentRespawnAnchors = respawnAnchors[previousIndex];
                popUpindex = -1;
            }

        }

        else if (popUpindex == 4)
        {
            if (!isPlayerInTutorialGrounds && !isPlayerInNextPart)
            {
                popUpindex++;
                previousIndex = popUpindex;
                currentRespawnAnchors = respawnAnchors[previousIndex];
                popUpindex = -1;
            }
        }

        else if (popUpindex == 5)
        {

            bool isTemple = Physics.Raycast(player.transform.position, -transform.up, 2f, whatisTemple);
            if (isTemple)
            {
                popUpindex++;
                previousIndex = popUpindex;
                currentRespawnAnchors = respawnAnchors[previousIndex];
                popUpindex = -1;
            }
        }

        else if (popUpindex == 6)
        {
            //if player wins
            //   popUpindex++;
        }

        //if the player is back on tutorial grounds set the popUpindex back
        else if (popUpindex == -1 && isPlayerInTutorialGrounds)
        {
            popUpindex = previousIndex;
            currentRespawnAnchors = respawnAnchors[previousIndex];
            previousIndex = 0;
        }

    }
}
 