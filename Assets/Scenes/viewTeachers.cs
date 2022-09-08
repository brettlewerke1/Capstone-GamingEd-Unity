using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class viewTeachers : MonoBehaviour
{
    readonly string getTeachersURL = "http://localhost/UnityApp/getAllTeachers.php";
    readonly string getTeacherURL = "http://localhost/UnityApp/getOneTeacher.php";
    public GameObject buttonPrefab;
    public GameObject buttonParent;
    public Text TeacherInformation;
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(GetAllTeachers());
        
    }
    public void GoBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }


    IEnumerator GetAllTeachers()
    {
        UnityWebRequest www = UnityWebRequest.Get(getTeachersURL);
            yield return www.SendWebRequest();
            string text = www.downloadHandler.text;
            //Debug.Log(text);
            string[] response = text.Split(' ');
            // format the string
            foreach(string username in response)
            {
                if(username != "")
                {
                GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
                newButton.GetComponent<teacherButton>().teacherName.text = username;
                newButton.GetComponent<Button>().onClick.AddListener(() =>StartCoroutine(GetTeacher(username)));
                }
            }


    }
    public IEnumerator GetTeacher(string username)
    {
        string text;

        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("Username", username));
        using (UnityWebRequest www = UnityWebRequest.Post(getTeacherURL, wwwForm))
        {
           yield return www.SendWebRequest();
            text = www.downloadHandler.text;
            Debug.Log(text);
            DbManager.AdminInformation = text;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(7);

        


    }
}
