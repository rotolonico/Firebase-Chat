using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneHandler : MonoBehaviour
{
    public TMP_InputField textIF;

    public GameObject messagePrefab;
    public Transform messagesContainer;

    private void Start() => APIHandler.Instance.databaseAPI.ListenForMessages(InstantiateMessage, Debug.Log);

    public void SendMessage()
    {
        APIHandler.Instance.databaseAPI.PostMessage(
            new Message(APIHandler.Instance.authAPI.GetUser().nickname, textIF.text),
            () => Debug.Log("Message was sent!"), Debug.Log);
    }

    private void InstantiateMessage(Message message)
    {
        var newMessage = Instantiate(messagePrefab, transform.position, Quaternion.identity);
        newMessage.transform.SetParent(messagesContainer, false);
        newMessage.GetComponent<TextMeshProUGUI>().text = $"{message.sender}: {message.text}";
    }

    public void SignOut()
    {
        APIHandler.Instance.databaseAPI.StopListeningForMessages();
        APIHandler.Instance.authAPI.SignOut();
        SceneManager.LoadScene(1);
    }
}