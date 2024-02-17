/*
 * AppController.cs
 * By: William McCarty
 * Handles Running of main app flow 
 * */
using UnityEngine;

public class AppController : MonoBehaviour
{
    //Singleton AppController
    private static AppController _Instance;
    public static AppController Instance
    {
        get
        {
            //If we don't have a instance at the time of access, find it in the scene
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<AppController>();
            }
            return _Instance;
        }
    }
    
    //User that is currently running the Chat application
    User _ActiveUser;
    public User ActiveUser
    {
        get
        {
            return _ActiveUser;
        }
    }

    //Main float Start
    private void Awake()
    {
        _Instance = this;
        DontDestroyOnLoad(this.gameObject);
        //Create the user that will have this data - Simulated internal login
        _ActiveUser = new User("SomeUN", "SomePWTheUserEntered");
        //Transistion to starting screen
        UIManager.Instance.OnUISelect(UIManager.UIType.ConversationsList);
    }
}
