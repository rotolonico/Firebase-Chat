using System;

[Serializable]
public class Message
{
    public string senderNickname;
    public string senderUserId;
    public string text;
    
    public Message(string senderNickname, string senderUserId, string text)
    {
        this.senderNickname = senderNickname;
        this.senderUserId = senderUserId;
        this.text = text;
    }
}
