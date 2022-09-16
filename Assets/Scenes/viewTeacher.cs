using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class viewTeacher : MonoBehaviour
{
    public Text TeacherInformation;
    public Text TeacherName;

    // Start is called before the first frame update
    void Start()
    {
        TeacherInformation.text = DbManager.AdminInformation;
        string[] adminInfo = DbManager.AdminInformation.Split(' ');
        TeacherName.text = adminInfo[1];
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
