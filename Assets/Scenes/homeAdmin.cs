using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class homeAdmin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CourseButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(9);
    }

    public void LogoutButton()
    {
        DbManager.logOut();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    }
}
