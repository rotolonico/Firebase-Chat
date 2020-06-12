using UnityEngine;
using UnityEngine.SceneManagement;

public class APIHandler : MonoBehaviour
{
    public static APIHandler Instance;
    public DatabaseAPI databaseAPI;
    public AuthAPI authAPI;
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        databaseAPI = GetComponent<DatabaseAPI>();
        authAPI = GetComponent<AuthAPI>();
        SceneManager.LoadScene(1);
    }
}
