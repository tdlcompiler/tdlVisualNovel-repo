using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MessageList
{
    public List<Message> messages;
}

public class JsonReader : MonoBehaviour
{
    public TextAsset jsonFile;

    public static MessageList messageList;

    private int startMessagesCount;

    void Start()
    {
        string jsonString = jsonFile.text;
        messageList = JsonUtility.FromJson<MessageList>(jsonString);
        startMessagesCount = messageList.messages.Count;
        analysisMessages();
    }

    private void analysisMessages()
    {
        while (findAndSplitMessages() > 0)
            findAndSplitMessages();
    }

    private int findAndSplitMessages()
    {
        int count = 0;
        foreach (Message message in messageList.messages.ToArray())
        {
            if (message.content.Length > 125)
            {
                splitMessage(message);
                count++;
            }
        }
        return count;
    }

    private void splitMessage(Message message)
    {
        Message messageContinuation = new() { content = message.content[122..], id = ++startMessagesCount, in_response_to = message.in_response_to, sender_name = message.sender_name, isContinuation = true };
        message.content = message.content[..122];
        message.content += "...";
        messageList.messages.Insert(messageList.messages.IndexOf(message) + 1, messageContinuation);
    }

    public Message getMessage(int index)
    {
        if (index < messageList.messages.Count - 1)
            return messageList.messages[index < 0 ? 0 : index];
        return null;
    }

    public Message getMessageByID(int id)
    {
        foreach (Message message in messageList.messages)
        {
            if (message.id == id)
                return message;
        }
        return null;
    }
}
