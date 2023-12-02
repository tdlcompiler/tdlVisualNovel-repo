using UnityEngine;

public class ContainerController : MonoBehaviour
{
    public void allowInput()
    {
        GetComponentInParent<InputHandler>().setInputState(true);
    }

    public void rejectInput()
    {
        GetComponentInParent<InputHandler>().setInputState(false);
    }

    public void destroyContainer()
    {
        Destroy(gameObject);
    }
}
