using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

class GUIManager : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI gameEndText;
    [SerializeField]
    public GameObject GameEndPanel;
    [SerializeField]
    public Button button;

    private void Start()
    {
        GameEndPanel.SetActive(false);
        gameEndText.text = "";
        button.gameObject.SetActive(false);
    }
    
    public void DisplayGameEndMessage(int winner)
    {
        GameEndPanel.SetActive(true);
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
        GameManager.Instance._shouldLoadFromSavedLocation = true;
        int lastScene = GameManager.Instance.LoadLastVisitedScene();
        Debug.Log("Loading last scene: " + lastScene.ToString());
        GameEndPanel.SetActive(false);
        SceneManagers.StaticLoad(lastScene);
    }
}