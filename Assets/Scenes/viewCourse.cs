using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class viewCourse : MonoBehaviour
{
    public Text StudentInformation;

    readonly string GetOneCourseURL = "http://localhost/UnityApp/getOneCourse.php";

    public Text ClassName;
    // Start is called before the first frame update
    void Start()
    {
        ClassName.text = PlayerPrefs.GetString("ClassTag");
        StartCoroutine(getClassInformation());

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator getClassInformation()
    {
        string text;
        Debug.Log("I AM RAN");

        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("ClassNumber", PlayerPrefs.GetString("ClassTag")));
        using (UnityWebRequest www = UnityWebRequest.Post(GetOneCourseURL, wwwForm))
        {
           yield return www.SendWebRequest();
            text = www.downloadHandler.text;
            if(text.Length == 0)
            {
                Debug.Log("No students found for course: " + PlayerPrefs.GetString("ClassTag"));
            }
            else
            {
                CreateTextObjects(text);


            }


            //DbManager.AdminInformation = text;
        }
    }
     public void GoBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(9);
    }


    //this function takes in the response from the DB and string splits it
    // it then spawns in an object that holds text and a button to view the specific student
    public void CreateTextObjects(string dbResponse)
    {
        Vector3 position;
        GameObject newObj;
        GameObject parent;
        string[] studentInfo;
        string[] listOfStudents = convertToList(dbResponse);
        // get the first object(it should exist already)
        GameObject firstObj = GameObject.Find("Row");
        //assign the parent after we capture row
        parent = GameObject.Find("TableRows");
        //split the first string in the array
        studentInfo =  listOfStudents[0].Split(new string[] { "%/%/%/%/%/%/%/%/%/%/*&^" }, StringSplitOptions.None);
        firstObj.transform.GetChild(0).gameObject.GetComponent<Text>().text =studentInfo[0].ToString();
        firstObj.transform.GetChild(1).gameObject.GetComponent<Text>().text =studentInfo[1].ToString();
        firstObj.transform.GetChild(2).gameObject.GetComponent<Text>().text =studentInfo[2].ToString();
        firstObj.transform.GetChild(3).gameObject.GetComponent<Text>().text =studentInfo[3].ToString();

        //---------------Now spawn in objects underneath it--------------//

        for(int i = 1; i < listOfStudents.Length -1; i++)
        {
            studentInfo =  listOfStudents[i].Split(new string[] { "%/%/%/%/%/%/%/%/%/%/*&^" }, StringSplitOptions.None);
            Debug.Log(studentInfo[0]);
            Debug.Log(studentInfo[1]);
            Debug.Log(studentInfo[2]);
            Debug.Log(studentInfo[3]);
            position = findLowestRow(firstObj);
            position.y -=25;

            newObj = Instantiate(firstObj, position, Quaternion.identity);
            newObj.transform.SetParent(parent.transform);
            newObj.tag = "Rows";
            newObj.transform.GetChild(0).gameObject.GetComponent<Text>().text =studentInfo[0].ToString();
            newObj.transform.GetChild(1).gameObject.GetComponent<Text>().text =studentInfo[1].ToString();
            newObj.transform.GetChild(2).gameObject.GetComponent<Text>().text =studentInfo[2].ToString();
            newObj.transform.GetChild(3).gameObject.GetComponent<Text>().text =studentInfo[3].ToString();

        }

    }

    public string[] convertToList(string dbResponse)
    {
        string[] split = dbResponse.Split(new string[] { "////////////////////%%%%%" }, StringSplitOptions.None);
        return split;
    }

    public Vector3 findLowestRow(GameObject obj)
    {
        GameObject lowestObject = obj;
        foreach (GameObject go in  GameObject.FindGameObjectsWithTag("Rows"))
        {
          if(go.transform.position.y < lowestObject.transform.position.y)
          {
            lowestObject = go;
        
          }
        }
        return lowestObject.transform.position;
    }

    public void GoToStudent(Button button)
    {
        GameObject parent = button.transform.parent.gameObject;
        //set player prefs to student name associated with the button we clicked
        string studentName =parent.transform.GetChild(0).gameObject.GetComponent<Text>().text;
        PlayerPrefs.SetString("StudentName", studentName);
        UnityEngine.SceneManagement.SceneManager.LoadScene("viewStudent");  

    }

    public void GoToCourseCreationPage()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainCourseCreator");  

    }
}
