using UnityEngine;
using UnityEngine.UI;

public class PlayButtonHandler : MonoBehaviour
{
    public Button button;

    void Start()
    {
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        
    }
}
