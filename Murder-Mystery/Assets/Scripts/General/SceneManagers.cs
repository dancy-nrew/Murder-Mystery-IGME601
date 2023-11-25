using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public static void StaticLoad(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public static int GetCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        return scene.buildIndex;
    }
}

