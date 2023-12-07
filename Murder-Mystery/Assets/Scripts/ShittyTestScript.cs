using UnityEngine;

public class ShittyTestScript : MonoBehaviour
{
    //This script is only for testing
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.ToggleMusic();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AudioManager.Instance.ChangeSong("mExploration");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AudioManager.Instance.ChangeSong("mCardBattle");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AudioManager.Instance.PlaySFX("aCardDeal");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AudioManager.Instance.PlaySFX("aCardFlip");
        }
    }
}
