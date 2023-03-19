using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public bool IsCurrentSceneLast => SceneManager.sceneCount - 1 == SceneManager.GetActiveScene().buildIndex;

    public void SwitchToNextScene()
    {
        if (IsCurrentSceneLast)
        {
            Debug.LogError("Current scene is last!");
            return;
        }

        SwitchToScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SwitchToScene(int sceneBuildIndex)
    {
        if (sceneBuildIndex < 0 || SceneManager.sceneCountInBuildSettings <= sceneBuildIndex)
        {
            Debug.LogError("Scene index out of range!");
            return;
        }

        SceneManager.LoadScene(sceneBuildIndex);
    }
}
