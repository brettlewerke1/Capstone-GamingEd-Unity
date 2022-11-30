using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CourseCreation : MonoBehaviour
{

    public TMP_InputField CourseName;

    public TMP_InputField CourseTag;

    public TMP_InputField JoinKey;

    public Toggle marketplace;

    public Button createCourseButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateCourse()
    {
        if(marketplace.isOn == true)
        {
            PlayerPrefs.SetString("Marketplace", "true");
        }
        else if(marketplace.isOn==false)
        {
            PlayerPrefs.SetString("Marketplace", "false");
        }
        PlayerPrefs.SetString("ClassTag", CourseTag.text);
        PlayerPrefs.SetString("ClassName", CourseName.text);
        PlayerPrefs.SetString("JoinKey", JoinKey.text);
       
        string root =  Application.persistentDataPath +"/" + PlayerPrefs.GetString("ClassTag");
        // If directory does not exist, create it.  
        if (!Directory.Exists(root))  
        {  
            Directory.CreateDirectory(root);  
        }
        //Debug.Log(PlayerPrefs.GetString("ClassTag"));
        PlayerPrefs.SetString("LoggedIn", "false");
        SceneManager.LoadScene("MainCourseCreator");

    }

    public void VerifyInputs()
    {
        if(CourseName.text.Length != 0 && JoinKey.text.Length != 0 && CourseTag.text.Length !=0)
        {
            createCourseButton.interactable = true;

        }
        else
        {
            createCourseButton.interactable = false;
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("mainMenu");
    }
}
