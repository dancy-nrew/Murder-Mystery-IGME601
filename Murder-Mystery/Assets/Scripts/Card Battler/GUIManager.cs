using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class GUIManager : MonoBehaviour
{
    public TextMeshProUGUI gameEndText;
    public Button button;

    private void Start()
    {
        gameEndText = GameObject.Find("GameEndMessage").GetComponent<TextMeshProUGUI>();
        gameEndText.text = "";
        button = GameObject.Find("ExitButton").GetComponent<Button>();
        button.gameObject.SetActive(false);
    }

    public void DisplayGameEndMessage(int winner)
    {
        if (winner == 0)
        {
            gameEndText.text = "It's a draw!";
        } else if (winner == ConstantParameters.PLAYER_1)
        {
            gameEndText.text = "Yes! We caught them!";
        } else
        {
            gameEndText.text = "Oh no! We couldn't catch them!";
        }
    }

    public void ShowButton()
    {
        button.gameObject.SetActive(true);
    }

    public void ExitCardBattler()
    {
        // How do we return to the specific scene we were just in?
        // Currently sending the user back to the hub which is not great
        SceneManager.LoadScene("3DIso_Movement");
    }
}