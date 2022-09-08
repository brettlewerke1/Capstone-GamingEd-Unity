using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class viewCourses : MonoBehaviour
{
    readonly string postURL = "http://localhost/UnityApp/getAllCourses.php";

    readonly string otherURL = "http://localhost/UnityApp/getOneCourse.php";
    public GameObject buttonPrefab;
    public GameObject buttonParent;
    public Text ClassInformation;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ViewTeacherCourses());
    }

    IEnumerator ViewTeacherCourses()
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("Username", DbManager.username));
        UnityWebRequest www = UnityWebRequest.Post(postURL, wwwForm);
        yield return www.SendWebRequest();
            string text = www.downloadHandler.text;
            if(text == "none")
            {
                ClassInformation.text = "You have no courses created, lets make one!";

            }
            else
            {
                ClassInformation.text = "List of courses you have created";
                string[] classes= text.Split(',');
                // format the string
                foreach(string classNum in classes)
                {
                    if(classNum != "")
                    {
                    GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
                    newButton.GetComponent<studentButton>().className.text = classNum;
                    newButton.GetComponent<Button>().onClick.AddListener(() =>StartCoroutine(GetClass(classNum)));
                    }
                }

            }
            Debug.Log(text);
    }
     public IEnumerator GetClass(string classNum)
    {
        string text;
        DbManager.ClassName = classNum;

        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("ClassNumber", classNum));
        using (UnityWebRequest www = UnityWebRequest.Post(otherURL, wwwForm))
        {
           yield return www.SendWebRequest();
            text = www.downloadHandler.text;
            Debug.Log(text);
            DbManager.StudentInformation = text;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(10);  

        


    }

    public void goBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);   
    }
}
