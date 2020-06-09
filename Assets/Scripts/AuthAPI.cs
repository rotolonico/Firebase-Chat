using System;
using Firebase.Auth;
using UnityEngine;

public class AuthAPI : MonoBehaviour
{
    private FirebaseAuth auth;
    
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

    public bool IsSignedIn() => auth.CurrentUser != null;
}
