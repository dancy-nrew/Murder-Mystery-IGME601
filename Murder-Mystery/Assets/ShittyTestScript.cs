using UnityEngine;

public class ShittyTestScript : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.ToggleMusic();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AudioManager.Instance.PlaySFX("SFXAudioSource0");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AudioManager.Instance.PlaySFX("SFXAudioSource1");
        }
    }
}
