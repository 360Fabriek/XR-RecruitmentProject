using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadAScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
