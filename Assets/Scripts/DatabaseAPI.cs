using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class DatabaseAPI : MonoBehaviour
{
    private DatabaseReference reference;
    private EventHandler<ChildChangedEventArgs> newMessageListener;
    private EventHandler<ChildChangedEventArgs> editedMessageListener;
    private EventHandler<ChildChangedEventArgs> deletedMessageListener;

    private void Awake()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://tutorial-9be3d.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void PostMessage(Message message, Action callback, Action<AggregateException> fallback)
    {
        var messageJSON = StringSerializationAPI.Serialize(typeof(Message), message);
        reference.Child("messages").Push().SetRawJsonValueAsync(messageJSON).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }

    public void EditMessage(string messageId, string newText, Action callback, Action<AggregateException> fallback)
    {
        var messageJSON = StringSerializationAPI.Serialize(typeof(string), newText);
        reference.Child($"messages/{messageId}/text").SetRawJsonValueAsync(messageJSON).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }

    public void DeleteMessage(string messageId, Action callback, Action<AggregateException> fallback)
    {
        reference.Child($"messages/{messageId}").SetRawJsonValueAsync(null).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }

    public void ListenForNewMessages(Action<Message, string> callback, Action<AggregateException> fallback)
    {
        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null) fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
            else
                callback(
                    StringSerializationAPI.Deserialize(typeof(Message), args.Snapshot.GetRawJsonValue()) as Message,
                    args.Snapshot.Key);
        }

        newMessageListener = CurrentListener;

        reference.Child("messages").ChildAdded += newMessageListener;
    }

    public void ListenForEditedMessages(Action<string, string> callback, Action<AggregateException> fallback)
    {
        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null) fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
            else
                callback(args.Snapshot.Key,
                    StringSerializationAPI.Deserialize(typeof(string), args.Snapshot.Child("text").GetRawJsonValue()) as
                        string);
        }

        editedMessageListener = CurrentListener;

        reference.Child("messages").ChildChanged += editedMessageListener;
    }

    public void ListenForDeletedMessages(Action<string> callback, Action<AggregateException> fallback)
    {
        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null) fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
            else callback(args.Snapshot.Key);
        }

        deletedMessageListener = CurrentListener;

        reference.Child("messages").ChildRemoved += deletedMessageListener;
    }

    public void StopListeningForMessages()
    {
        reference.Child("messages").ChildAdded -= newMessageListener;
        reference.Child("messages").ChildChanged -= editedMessageListener;
        reference.Child("messages").ChildRemoved -= deletedMessageListener;
    }

    public void PostUser(User user, Action callback, Action<AggregateException> fallback)
    {
        var messageJSON = StringSerializationAPI.Serialize(typeof(User), user);
        reference.Child($"users/{APIHandler.Instance.authAPI.GetUserId()}").SetRawJsonValueAsync(messageJSON)
            .ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
                else callback();
            });
    }

    public void GetUser(Action<User> callback, Action<AggregateException> fallback) =>
        reference.Child($"users/{APIHandler.Instance.authAPI.GetUserId()}").GetValueAsync().ContinueWith(
            task =>
            {
                if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
                else callback(StringSerializationAPI.Deserialize(typeof(User), task.Result.GetRawJsonValue()) as User);
            });
}