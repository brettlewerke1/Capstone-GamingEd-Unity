using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button loginButton;

    public void Start()
    {

    }
    public void GoToLogIn()
    {
        SceneManager.LoadScene(2);
    }

    public void GoToCourseCreationPage()
    {
        SceneManager.LoadScene("CourseCreation");
    }
}


// test comment

// another test comment

