using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("References")] [SerializeField] private GameObject previewTank;
    [SerializeField] private InputField nameInputBox;

    public void ExitGame() //When called will quit the game
    {
        Application.Quit();
    }

    public void CheckIfReadyAllowed(Button readyButton) //If the player name and tank colour have been selected then allow the next button
    {
        //Only run the check if the parent is enabled
        if (readyButton.transform.parent.gameObject.activeSelf)
        {
            //Decide whether the cutomization button 
            if (nameInputBox.text.Length > 0) { readyButton.interactable = true; }
            else { readyButton.interactable = false; }
        }
    }

    public void ChangeTankPreviewColour(Image myImage) //Change the tank preview for when customizing your tank
    {
        //Set the colour of the preview tank
        foreach (Transform child in previewTank.transform)
        {
            child.GetComponent<Image>().color = myImage.color;
        }
    }

    public void P1Ready() //Save player 1's customization settings
    {
        //Save the player colour and name
        PlayerManager.singleton.SetPlayerInfo(1, nameInputBox.text, previewTank.GetComponentInChildren<Image>().color);
        //Clear the name input box and preview box ready for player 2
        nameInputBox.text = "";
        //Set the colour of the preview tank to white
        foreach (Transform child in previewTank.transform)
        {
            child.GetComponent<Image>().color = Color.white;
        }
    }

    public void P2Ready() //Save player 2's customization settings
    {
        //Save the player colour and name
        PlayerManager.singleton.SetPlayerInfo(2, nameInputBox.text, previewTank.GetComponentInChildren<Image>().color);
        //Start the game
        SceneManager.LoadScene(1);
    }
}
