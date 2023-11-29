using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Button button;
    private Vector3 originalScale;
    private Coroutine scaleDownCoroutine;
    private Coroutine scaleUpCoroutine;

    [SerializeField]
    private float scaleDuration = 0.2f;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button != null)
        {
            if (scaleDownCoroutine != null)
            {
                StopCoroutine(scaleDownCoroutine);
            }

            scaleDownCoroutine = StartCoroutine(ScaleDown());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button != null)
        {
            if (scaleDownCoroutine != null)
            {
                StopCoroutine(scaleDownCoroutine);
            }

            if (scaleUpCoroutine != null)
            {
                StopCoroutine(scaleUpCoroutine);
            }

            scaleUpCoroutine = StartCoroutine(ScaleUp());
        }
    }

    IEnumerator ScaleDown()
    {
        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, originalScale * 0.9f, elapsedTime / scaleDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale * 0.9f;
    }

    IEnumerator ScaleUp()
    {
        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale * 0.9f, originalScale, elapsedTime / scaleDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }

    void TaskOnClick()
    {
        //
    }
}
