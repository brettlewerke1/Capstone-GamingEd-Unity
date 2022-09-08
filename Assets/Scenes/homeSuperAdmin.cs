using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class homeSuperAdmin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RegisterTeachers()
    {
        SceneManager.LoadScene(1);

    }

    public void getTeachers()
    {
        SceneManager.LoadScene(5);
    }

    public void LogoutButton()
    {
        DbManager.logOut();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    }
}
