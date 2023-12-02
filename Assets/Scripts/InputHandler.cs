using System.Collections;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public bool allowedInput;
    public GameObject dialogueController;
    public GameObject restarter;
    public float doubleTapTimeThreshold = 0.1f;

    private bool tapCoroutineStarted = false;

    void Update()
    {
        if (allowedInput && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)))
        {
            if (tapCoroutineStarted)
            {
                StopCoroutine(nameof(TapCoroutine));
                HandleDoubleTap();
                tapCoroutineStarted = false;
            }
            else
            {
                tapCoroutineStarted = true;
                StartCoroutine(nameof(TapCoroutine));
            }
        }
    }

    IEnumerator TapCoroutine()
    {
        yield return new WaitForSeconds(doubleTapTimeThreshold);
        if (tapCoroutineStarted)
        {
            tapCoroutineStarted = false;
            HandleOneTap();
        }
    }

    public void setInputState(bool allowed)
    {
        allowedInput = allowed;
    }

    void HandleDoubleTap()
    {
        DialogueController dialogueControl = dialogueController.GetComponent<DialogueController>();
        dialogueControl.addMessage(true);
    }

    void HandleOneTap()
    {
        DialogueController dialogueControl = dialogueController.GetComponent<DialogueController>();
        if (dialogueControl.storyEnded)
            restarter.GetComponent<Restarter>().Restart();
        dialogueControl.addMessage(false);
    }

    Vector2 GetTouchPosition()
    {
        if (Input.touchCount > 0)
        {
            return Camera.main.ScreenToWorldPoint(Input.touches[0].position);
        }
        else
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
