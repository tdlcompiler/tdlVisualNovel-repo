using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int newScene;
    public GameObject startText;
    public GameObject dialogueController;

    void loadScene()
    {
        InputHandler handler = transform.parent.GetComponent<InputHandler>();

        if (handler != null)
            handler.allowedInput = false;
        SceneManager.LoadScene(newScene);
    }

    void finishLoading()
    {
        InputHandler handler = transform.parent.GetComponent<InputHandler>();

        if (startText != null)
            startText.SetActive(true);

        if (dialogueController != null)
            dialogueController.SetActive(true);

        if (handler != null)
            handler.allowedInput = true;

        GameObject.Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
