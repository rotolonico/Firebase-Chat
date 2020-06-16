using System;
using Firebase.Auth;
using UnityEngine;

public class AuthAPI : MonoBehaviour
{
    private FirebaseAuth auth;
    private User user;

    private void Awake() => auth = FirebaseAuth.DefaultInstance;
    
    public void SignUpUser(string email, string password, Action callback, Action<AggregateException> fallback)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }

    public void SignInUser(string email, string password, Action callback, Action<AggregateException> fallback)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }

    public void SignOut() => auth.SignOut();

    public string GetUserId() => auth.CurrentUser.UserId;

    public User GetUser() => user;
    
    public void SetUser(User user) => this.user = user;
}
