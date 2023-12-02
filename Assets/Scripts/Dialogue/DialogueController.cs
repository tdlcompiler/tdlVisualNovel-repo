using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    private static readonly string ME_IS_AUTHOR = "ß";
    private static readonly string ME_IS_SENDER_ATTRIBUTE = "me";
    private static readonly string LEFT_FIRST_MESSAGE_ANIM = "FirstMessageAnimation";
    private static readonly string RIGHT_FIRST_MESSAGE_ANIM = "RightFirstMessageAnimation";
    private static readonly string SHOW_COMPANION_MESSAGE_CONTAINER_ANIM = "LeftMessageContainerAnimation";
    private static readonly string SHOW_MY_MESSAGE_CONTAINER_ANIM = "RightMessageContainerAnimation";
    private static readonly string HIDE_MY_MESSAGE_CONTAINER_ANIM = "HideRightMessageAnimation";
    private static readonly string HIDE_COMPANION_MESSAGE_CONTAINER_ANIM = "HideMessageAnimation";
    private static readonly string RETURN_COMPANION_MESSAGE_CONTAINER_ANIM = "ReturnMessageAnimation";
    private static readonly string RETURN_PREVIOUS_COMPANION_MESSAGE_CONTAINER_ANIM = "ReturnPrevMessageAnimation";
    private static readonly string HIDE_FINISH_STORY_TEXT_ANIM = "HideFinishStoryTextAnimation";
    private static readonly string HIDE_HELP_TEXT_ANIM = "HideHelpTextAnimation";
    private static readonly string RESPONSE_CONTAINER_NAME = "ResponseContainer";

    public static readonly string NULL_STRING = "";

    public GameObject companionMessageContainer;
    public GameObject myMessageContainer;
    public GameObject startText;
    public GameObject helpText;
    public GameObject finishStoryText;
    public GameObject blockPrevMsgText;
    public Canvas canvas;
    public Sprite alternativeContainer;
    public bool storyEnded = false;

    private GameObject finishStoryTextCopy;
    private GameObject lastMessage;
    private Message currentMessageObj;
    private bool start = true;
    private List<GameObject> messageList = new List<GameObject>();

    public void addMessage(bool prevMessage)
    {
        Message message;
        int currentMessageListIndex = JsonReader.messageList.messages.IndexOf(currentMessageObj);
        if (storyEnded && prevMessage)
            currentMessageListIndex = JsonReader.messageList.messages.Count - 1;
        else if (currentMessageListIndex < 0)
        {
            currentMessageListIndex = 0;
            start = true;
        }
        message = GetComponent<JsonReader>().getMessage(prevMessage ? currentMessageListIndex - 1 : start || storyEnded ? currentMessageListIndex : currentMessageListIndex + 1);
        currentMessageObj = message;
        if (prevMessage && currentMessageListIndex == 0)
        {
            playFirstMessageAnimation();
            return;
        }
        if (storyEnded && prevMessage)
        {
            if (finishStoryTextCopy.TryGetComponent<Animator>(out var animator))
            {
                animator.Play(HIDE_FINISH_STORY_TEXT_ANIM);
            }
            storyEnded = false;
        }
        if (message != null)
        {
            if (lastMessage != null)
            {
                hideMessage(lastMessage, prevMessage);
            }
            if (message.in_response_to == NULL_STRING)
                ShowNewMessage(message.content, message.sender_name, prevMessage);
            else
            {
                Message listenerMessage = GetComponent<JsonReader>().getMessageByID(int.Parse(message.in_response_to));
                if (listenerMessage != null)
                    ShowNewMessage(message.content, message.sender_name, prevMessage, true, listenerMessage.sender_name, listenerMessage.content);
            }
        }
        else
        {
            if (lastMessage != null)
            {
                hideMessage(lastMessage, prevMessage);
            }
            storyEnded = true;
            finishStoryTextCopy = Instantiate(finishStoryText, canvas.transform);
        }
        start = false;
    }

    public void playFirstMessageAnimation()
    {
        if (lastMessage != null && lastMessage.TryGetComponent<Animator>(out var animator))
        {
            if (lastMessage.GetComponentsInChildren<TextMeshProUGUI>()[1].text != ME_IS_AUTHOR)
                animator.Play(LEFT_FIRST_MESSAGE_ANIM, 0, 0);
            else
                animator.Play(RIGHT_FIRST_MESSAGE_ANIM, 0, 0);
            Instantiate(blockPrevMsgText, canvas.transform);
        }
    }

    private void ShowNewMessage(string text, string sender, bool prevMessage, bool isResponse = false, string listenerName = null, string listenerMessageText = null)
    {
        GameObject newMessage;
        TextMeshProUGUI textComponent;
        TextMeshProUGUI authorTextComponent;
        if (sender == ME_IS_SENDER_ATTRIBUTE)
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
            responseAuthorTextComponent.SetText(listenerName == ME_IS_SENDER_ATTRIBUTE ? ME_IS_AUTHOR : listenerName);
        }
        if (startText != null)
        {
            if (helpText.TryGetComponent<Animator>(out var helpTextAnimator))
            {
                helpTextAnimator.Play(HIDE_HELP_TEXT_ANIM);
            }
            //startText.SetActive(false);
            GameObject.Destroy(startText);
            startText = null;
        }
        if (JsonReader.messageList.messages.IndexOf(currentMessageObj) == 0 && authorTextComponent.text != ME_IS_AUTHOR)
            newMessage.GetComponent<Image>().sprite = alternativeContainer;
        if (newMessage.TryGetComponent<Animator>(out var animator))
        {
            if (!prevMessage)
            {
                if (authorTextComponent.text != ME_IS_AUTHOR)
                    animator.Play(SHOW_COMPANION_MESSAGE_CONTAINER_ANIM);
                else
                    animator.Play(SHOW_MY_MESSAGE_CONTAINER_ANIM);
            }
            else
            {
                if (authorTextComponent.text != ME_IS_AUTHOR)
                    animator.Play(RETURN_PREVIOUS_COMPANION_MESSAGE_CONTAINER_ANIM);
                else
                    animator.Play(SHOW_MY_MESSAGE_CONTAINER_ANIM);
            }
        }
        messageList.Add(newMessage);
        lastMessage = newMessage;
    }

    private void hideMessage(GameObject message, bool prevMessage)
    {
        if (!message.TryGetComponent<Animator>(out var animator))
            return;
        if (!prevMessage)
        {
            if (message.GetComponentsInChildren<TextMeshProUGUI>()[1].text == ME_IS_AUTHOR)
                animator.Play(HIDE_MY_MESSAGE_CONTAINER_ANIM);
            else
                animator.Play(HIDE_COMPANION_MESSAGE_CONTAINER_ANIM);
        }
        else
        {
            if (message.GetComponentsInChildren<TextMeshProUGUI>()[1].text == ME_IS_AUTHOR)
                animator.Play(HIDE_MY_MESSAGE_CONTAINER_ANIM);
            else
                animator.Play(RETURN_COMPANION_MESSAGE_CONTAINER_ANIM);
        }
    }
}
