using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    private static readonly string ME_IS_AUTHOR = "Я";
    private static readonly string ME_IS_SENDER = "me";
    private static readonly string HIDE_MY_MESSAGE_CONTAINER_ANIM = "HideRightMessageAnimation";
    private static readonly string HIDE_COMPANION_MESSAGE_CONTAINER_ANIM = "HideMessageAnimation";
    private static readonly string RESPONSE_CONTAINER_NAME = "ResponseContainer";
    private static readonly string NULL_STRING = "";

    public int currentMessage = 0;
    public GameObject companionMessageContainer;
    public GameObject myMessageContainer;
    public GameObject startText;
    public GameObject finishStoryText;
    public Canvas canvas;
    public Sprite alternativeContainer;
    public bool storyEnded = false;

    private GameObject lastMessage;
    private List<GameObject> messageList = new List<GameObject>();

    public void addMessage(int index)
    {
        if (currentMessage > 0) // удалить
            return;
        Message message = startText.GetComponent<JsonReader>().getMessage(index);
        if (message != null)
        {
            if (lastMessage != null)
            {
                hideMessage(lastMessage);
            }
            if (message.in_response_to == NULL_STRING)
                ShowNewMessage(message.content, message.sender_name);
            else
            {
                Message listenerMessage = startText.GetComponent<JsonReader>().getMessage(int.Parse(message.in_response_to));
                if (listenerMessage != null)
                    ShowNewMessage(message.content, message.sender_name, true, listenerMessage.sender_name, listenerMessage.content);
            }
            currentMessage++;
        }
        else
        {
            if (lastMessage != null)
            {
                hideMessage(lastMessage);
            }
            storyEnded = true;
            finishStoryText.SetActive(true);
        }
    }

    private void ShowNewMessage(string text, string sender, bool isResponse = false, string listenerName = null, string listenerMessageText = null)
    {
        GameObject newMessage;
        TextMeshProUGUI textComponent;
        TextMeshProUGUI authorTextComponent;
        if (sender == ME_IS_SENDER)
        {
            newMessage = Instantiate(myMessageContainer, canvas.transform);
            authorTextComponent = newMessage.GetComponentsInChildren<TextMeshProUGUI>()[1];
            textComponent = newMessage.GetComponentsInChildren<TextMeshProUGUI>()[0];
            authorTextComponent.SetText(ME_IS_AUTHOR);
        }
        else
        {
            newMessage = Instantiate(companionMessageContainer, canvas.transform);
            authorTextComponent = newMessage.GetComponentsInChildren<TextMeshProUGUI>()[1];
            textComponent = newMessage.GetComponentsInChildren<TextMeshProUGUI>()[0];
            authorTextComponent.SetText(sender);
        }
        textComponent.SetText(text);
        if (isResponse)
        {
            GameObject responseContainer = newMessage.transform.Find(RESPONSE_CONTAINER_NAME).gameObject;
            responseContainer.SetActive(true);
            TextMeshProUGUI responseTextComponent = responseContainer.GetComponentsInChildren<TextMeshProUGUI>()[0];
            TextMeshProUGUI responseAuthorTextComponent = responseContainer.GetComponentsInChildren<TextMeshProUGUI>()[1];
            responseTextComponent.SetText(listenerMessageText);
            responseAuthorTextComponent.SetText(listenerName);
        }
        if (startText.activeSelf)
        {
            startText.SetActive(false);
            newMessage.GetComponent<Image>().sprite = alternativeContainer;
        }
        messageList.Add(newMessage);
        lastMessage = newMessage;
    }

    private void hideMessage(GameObject message)
    {
        Animator animator = message.GetComponent<Animator>();
        if (animator != null)
        {
            if (message.GetComponentsInChildren<TextMeshProUGUI>()[1].text == ME_IS_AUTHOR)
                animator.Play(HIDE_MY_MESSAGE_CONTAINER_ANIM);
            else
                animator.Play(HIDE_COMPANION_MESSAGE_CONTAINER_ANIM);
        }
    }
}
