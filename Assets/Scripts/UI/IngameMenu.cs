using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class IngameMenu : MonoBehaviour
{
    [Header("Required")] [SerializeField] private Text winnerLabel;
    [SerializeField] private Text p1NameLabel, p1VictoriesLabel, p2NameLabel, p2VictoriesLabel;

    private void Update()
    {
        //If escape has been pressed, then return to the main menu
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }

    public void SetResults(int winner) //When called, the results screen variables will be updated
    {
        //Display the winner
        switch (winner)
        {
            case 1:
                winnerLabel.text = "" + PlayerManager.singleton.GetPlayerName(1) + " wins!";
                break;
            case 2:
                winnerLabel.text = "" + PlayerManager.singleton.GetPlayerName(2) + " wins!";
                break;
        }
        //Update the amount of victories text
        p1VictoriesLabel.text = "" + PlayerManager.singleton.GetPlayerVictories(1);
        p1NameLabel.text = PlayerManager.singleton.GetPlayerName(1);
        p2VictoriesLabel.text = "" + PlayerManager.singleton.GetPlayerVictories(2);
        p2NameLabel.text = PlayerManager.singleton.GetPlayerName(2);
    }

    public void ReplayLevel()
    {
        //Get the current scene and load it again
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        //Switch between the two maps
        if (currentScene == 1) { currentScene++; }
        else { currentScene--; }
        //Load the new map
        SceneManager.LoadScene(currentScene);
    }

    public void ReturnToMainMenu()
    {
        //Destroy the player manager
        Destroy(PlayerManager.singleton.gameObject);
        //Return to main menu
        SceneManager.LoadScene(0);
    }
}
