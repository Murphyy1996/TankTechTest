using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager singleton;

    //Inspector variables
    [SerializeField] [Header("Required")] private GameObject tankPrefab;
    [SerializeField] [Header("Player Information")] private string player1Name = "player 1";
    [SerializeField] private string player2Name = "player 2";
    [SerializeField] private Color player1Colour = Color.white, player2Colour = Color.white;
    [SerializeField] [Header("Runtime player references")] private GameObject p1Tank;
    [SerializeField] private GameObject p2Tank;
    [SerializeField] private int p1Victories = 0, p2Victories = 0;

    private void Awake() //On awake, make this a singleton accesible from anywhere
    {
        singleton = this;
        DontDestroyOnLoad(this);
    }

    private void OnLevelWasLoaded(int level) //Trigger any methods when a level is loaded
    {
        //If not on the main menu
        if (level > 0)
        {
            //Spawn both players
            SpawnPlayers();
        }
    }

    private void SpawnPlayers()
    {
        //Find the spawn points in the scene
        Transform spawns = GameObject.Find("Spawnpoints").transform;
        //Spawn both players
        int pID = 1;
        foreach (Transform spawn in spawns)
        {
            if (pID == 1)
            {
                p1Tank = Instantiate(tankPrefab, spawn.position, tankPrefab.transform.rotation) as GameObject;
                ConfigureTank(1);
            }
            else
            {
                p2Tank = Instantiate(tankPrefab, spawn.position, tankPrefab.transform.rotation) as GameObject;
                ConfigureTank(2);
            }
            pID++;
        }
        //Give the tank variables to the turn manager begin the game
        TurnManager.singleton.SetUpTurnManager(p1Tank, p2Tank);
    }

    private void ConfigureTank(int pID)
    {
        //Declare the movement / gun scripts as these are only going to be used once and thats in the config
        Movement movementScript;
        Gun gunScript;
        //Declare empty variables that will be used when configuring players
        SpriteRenderer[] sprites = null;
        Text nameLabel = null;
        //Assign each player variables depending on the player
        switch (pID)
        {
            case 1:
                //Get the required scripts and set the name of the object
                p1Tank.name = "Player 1";
                movementScript = p1Tank.GetComponent<Movement>();
                gunScript = p1Tank.GetComponent<Gun>();
                //Set the colour of the object
                sprites = p1Tank.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer renderer in sprites)
                {
                    //Make sure the muzzle flash colour is not changed
                    if (renderer.gameObject.name != "MuzzleFlash")
                    {
                        renderer.color = player1Colour;
                    }
                }
                //Set the name tag and colour of it
                nameLabel = p1Tank.GetComponentInChildren<Text>();
                nameLabel.text = player1Name;
                nameLabel.color = player1Colour;
                //Turn off movement and gun script as the player will be activated when needed via the turn system
                movementScript.enabled = false;
                gunScript.enabled = false;
                break;
            case 2:
                //Get the required scripts and set the name of the object
                p2Tank.name = "Player 2";
                movementScript = p2Tank.GetComponent<Movement>();
                gunScript = p2Tank.GetComponent<Gun>();
                //Set the colour of the object
                sprites = p2Tank.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer renderer in sprites)
                {
                    //Make sure the muzzle flash colour is not changed
                    if (renderer.gameObject.name != "MuzzleFlash")
                    {
                        renderer.color = player2Colour;
                    }
                }
                //Set the name tag and colour of it
                nameLabel = p2Tank.GetComponentInChildren<Text>();
                nameLabel.text = player2Name;
                nameLabel.color = player2Colour;
                //Turn off movement and gun script as the player will be activated when needed via the turn system
                movementScript.enabled = false;
                gunScript.enabled = false;
                break;
        }
    }

    //Methods for setting / Giving variables from and to other scripts

    public void SetPlayerInfo(int playerNumber, string name, Color colour)
    {
        switch (playerNumber)
        {
            case 1:
                player1Name = name;
                player1Colour = colour;
                break;
            case 2:
                player2Name = name;
                player2Colour = colour;
                break;
        }
    }
    public string GetPlayerName(int playerNumber) //Return the requested player name
    {
        switch (playerNumber)
        {
            case 1:
                return player1Name;
            case 2:
                return player2Name;
        }
        return "";
    }

    public void IncrementVictoryCounter(int pID) //Increment the victory counter
    {
        switch (pID)
        {
            case 1:
                p1Victories++;
                break;
            case 2:
                p2Victories++;
                break;
        }
    }

    public int GetPlayerVictories(int pID)
    {
        switch (pID)
        {
            case 1:
                return p1Victories;
            case 2:
                return p2Victories;
        }
        return 0;
    }
}
