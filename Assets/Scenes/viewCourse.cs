using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class viewCourse : MonoBehaviour
{
    public Text StudentInformation;

    public Text ClassName;
    // Start is called before the first frame update
    void Start()
    {
        ClassName.text = DbManager.ClassName;
        string builder ="";
        string[] studentInfo = DbManager.StudentInformation.Split('/');
        foreach(string student in studentInfo)
        {
            builder +=(student +"\n");
        }
        StudentInformation.text = builder;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     public void GoBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(9);
    }
}
