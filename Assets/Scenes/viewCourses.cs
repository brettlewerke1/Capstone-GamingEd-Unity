using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class viewCourses : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject buttonParent;
    public Text ClassInformation;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ViewTeacherCourses());
        getMultipleCourses();
    }
     public void GetClass(string classNum)
    {
        string text;
        PlayerPrefs.SetString("ClassTag", classNum);
        UnityEngine.SceneManagement.SceneManager.LoadScene(10);  

        


    }

    public void goBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);   
    }
    

    //this function gets a list of classes in your local folder
    public void getMultipleCourses()
    {
        ClassInformation.text = "List of courses you have created";
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);
        DirectoryInfo[] classes = di.GetDirectories();
        // format the string
        foreach (DirectoryInfo classNum in classes)
        {

            GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
            newButton.GetComponent<studentButton>().className.text = classNum.Name;
            newButton.GetComponent<Button>().onClick.AddListener(() => GetClass(classNum.Name));

        }
    }

}
