[System.Serializable]
public class Message
{
    public int id;
    public string sender_name;
    public string in_response_to;
    public string content;
    public bool isContinuation = false;
}
