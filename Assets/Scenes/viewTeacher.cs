using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class viewTeacher : MonoBehaviour
{
    public Text TeacherInformation;
    public Text TeacherName;

    // dependent on the amount of spaces on echoed at the end of getOneTeacher.php
    int numberOfSpacesToDisplayUsername = 1;

    // Start is called before the first frame update
    void Start()
    {
        TeacherInformation.text = DbManager.AdminInformation;
        string[] adminInfo = DbManager.AdminInformation.Split(' ');
        TeacherName.text = adminInfo[numberOfSpacesToDisplayUsername];
    }

     public void GoBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }

    public void Delete()
    {
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(8);
    }

   


}
