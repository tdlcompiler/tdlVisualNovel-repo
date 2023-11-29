using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int newScene;

    void loadScene()
    {
        SceneManager.LoadScene(newScene);
    }

    void finishLoading()
    {
        gameObject.SetActive(false);
    }
}
