using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class MessageList
{
    public List<Message> messages;
}

public class JsonReader : MonoBehaviour
{
    public TextAsset jsonFile;

    private MessageList messageList;

    void Start()
    {
        string jsonString = jsonFile.text;
        messageList = JsonUtility.FromJson<MessageList>(jsonString);
    }

    public Message getMessage(int index)
    {
        if (index < messageList.messages.Count - 1)
            return messageList.messages[index];
        return null;
    }
}
