using TMPro;
using UnityEngine;

public class ChatHandler : MonoBehaviour
{
    public DatabaseAPI database;

    public TMP_InputField senderIF;
    public TMP_InputField textIF;

    public GameObject messagePrefab;
    public Transform messagesContainer;

    private void Start()
    {
        database.ListenForMessages(InstantiateMessage, Debug.Log);
    }

    public void SendMessage() => database.PostMessage(new Message(senderIF.text, textIF.text),
        () => Debug.Log("Message was sent!"), Debug.Log);

    private void InstantiateMessage(Message message)
    {
        var newMessage = Instantiate(messagePrefab, transform.position, Quaternion.identity);
        newMessage.transform.SetParent(messagesContainer, false);
        newMessage.GetComponent<TextMeshProUGUI>().text = $"{message.sender}: {message.text}";
    }

    public void SignOut()
    {
        
    }
}