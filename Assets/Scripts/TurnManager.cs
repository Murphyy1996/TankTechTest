using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static TurnManager singleton;
    [SerializeField] [Header("Required")] private Text currentTurnText;
    [SerializeField] private GameObject resultsScreen;
    [SerializeField] [Header("Turn Information")] private int currentTurn = 0;
    [SerializeField] [Header("Runtime References")] private Movement p1Movement;
    [SerializeField] private Movement p2Movement;
    [SerializeField] private Gun p1Gun, p2Gun;
    [SerializeField] private Animator textAnimator;

    private void Awake() //Initialise this singleton for easy access
    {
        singleton = this;
    }

    private void SetPlayerActive(int pID, bool isActive) //This will set whether the specified player is active or not
    {
        switch (pID)
        {
            case 1:
                p1Movement.enabled = isActive;
                p1Gun.enabled = isActive;
                break;
            case 2:
                p2Movement.enabled = isActive;
                p2Gun.enabled = isActive;
                break;
        }
    }

    public void SetUpTurnManager(GameObject player1, GameObject player2)
    {
        //Get the required scripts
        p1Movement = player1.GetComponent<Movement>();
        p2Movement = player2.GetComponent<Movement>();
        p1Gun = player1.GetComponent<Gun>();
        p2Gun = player2.GetComponent<Gun>();
        textAnimator = currentTurnText.gameObject.GetComponent<Animator>();
        //Start the intiial turn
        InitialTurn();

    } //Will be called when this script needs to be set up

    private void InitialTurn() //Run the code for the initial turn (Randomising which player goes first
    {
        //Randomly decide the first player turn
        Random.InitState((int)Time.time);
        currentTurn = 1;
        int randomNum = Random.Range(0, 10);
        if (randomNum >= 5) { currentTurn = 2; }
        //Display the correct turn text
        DisplayTurnText();
        //Activate the respective players tank
        SetPlayerActive(currentTurn, true);
    }

    private void DisplayTurnText() //Display the turn text for the correct player
    {
        //Scale the text
        textAnimator.SetTrigger("Scale");
        //Show which players turn it is
        currentTurnText.text = "" + PlayerManager.singleton.GetPlayerName(currentTurn) + "'s turn!";
    }

    public void NextTurn() //When called this will start the next turn and switch to the next player
    {
        //Deactivate the old player
        SetPlayerActive(currentTurn, false);
        //Switch the turn counter and configure the other player
        switch (currentTurn)
        {
            case 1:
                //Switch to player 2
                currentTurn = 2;
                p2Gun.Reload();
                break;
            case 2:
                //Switch to player 1
                currentTurn = 1;
                p1Gun.Reload();
                break;
        }
        //Show the new player turn counter
        DisplayTurnText();
        //Activate the new player
        SetPlayerActive(currentTurn, true);
    }

    public void EndGame(int pIDWinner) //This will end the game and show which player has won
    {
        //Disable movement of both players
        SetPlayerActive(1, false);
        SetPlayerActive(2, false);
        //Increment the victory counter
        PlayerManager.singleton.IncrementVictoryCounter(pIDWinner);
        //Configure the results screen on what to show
        resultsScreen.GetComponentInParent<IngameMenu>().SetResults(pIDWinner);
        //Display the results screen
        resultsScreen.SetActive(true);
    }

    public int GetCurrentTurn()
    {
        return currentTurn;
    }
}
