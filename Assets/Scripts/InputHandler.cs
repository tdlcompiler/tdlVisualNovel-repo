using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public bool allowedInput;
    public GameObject dialogueController;
    public GameObject restarter;

    void Update()
    {
        if (allowedInput && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)))
        {
            Vector2 touchPos = GetTouchPosition();
            if (TouchOnBackground(touchPos))
                HandleTouch();
        }
    }

    public void setInputState(bool allowed)
    {
        allowedInput = allowed;
    }

    void HandleTouch()
    {
        DialogueController dialogueControl = dialogueController.GetComponent<DialogueController>();
        if (dialogueControl.storyEnded)
            restarter.GetComponent<Restarter>().Restart();
        dialogueControl.addMessage(dialogueControl.currentMessage);
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

    bool TouchOnBackground(Vector2 touchPosition)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);

        if (hitCollider != null && hitCollider.gameObject == gameObject)
        {
            return true;
        }
        return false;
    }
}
