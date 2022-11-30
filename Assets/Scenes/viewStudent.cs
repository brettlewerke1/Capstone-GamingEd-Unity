using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class viewStudent : MonoBehaviour
{
    public TMP_Text StudentUsername;

    public TMP_Text StudentIGN;

    public TMP_Text StudentPassword;

    public TMP_Text StudentCoins;

    public TMP_Text AssignmentTodaysDate;

    public TMP_Text Assignment3WeeksAgo;

    public TMP_Text AssessmentTodaysDate;

    public TMP_Text Assessment3WeeksAgo;

    public string getOneStudentURL = "http://localhost/UnityApp/getOneStudent.php";

    public string getAllAssignmentsForCourseURL = "http://localhost/UnityApp/getAllAssignmentsForCourse.php";
    // Start is called before the first frame update
    void Start()
    {
        //asign student name we are looking at to username;
        StudentUsername.text = PlayerPrefs.GetString("StudentName");
        StartCoroutine(getStudentInformation());
        StartCoroutine(getRecentCompletedAssignments());

    }

    // Update is called once per frame
    void Update()
    {
        
 
 
    }

    public IEnumerator getStudentInformation()
    {
        string text;

        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("StudentUsername", StudentUsername.text));
        using (UnityWebRequest www = UnityWebRequest.Post(getOneStudentURL, wwwForm))
        {
           yield return www.SendWebRequest();
            text = www.downloadHandler.text;
            //Debug.Log(text);
            if(text.Length == 0)
            {
                
            }
            else
            {
               UpdateStudentInfoTableObj(text);

            }


            //DbManager.AdminInformation = text;
        }
    }

    public IEnumerator getRecentCompletedAssignments()
    {
        string text;

        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("StudentUsername", StudentUsername.text));
        using (UnityWebRequest www = UnityWebRequest.Post(getAllAssignmentsForCourseURL, wwwForm))
        {
            yield return www.SendWebRequest();
            text = www.downloadHandler.text;
            Debug.Log(text);
            if (text.Length == 0)
            {

            }
            else
            {
                UpdateAssignmentRowObjects(text);

            }


            //DbManager.AdminInformation = text;
        }

    }

    public void GoBack()
    {
        SceneManager.LoadScene("viewCourse");
    }

    public void UpdateStudentInfoTableObj(string dbResponse)
    {
        string[] studentInfo;
        string[] splitString = convertToList(dbResponse);
        studentInfo =  splitString[0].Split(new string[] { "%/%/%/%/%/%/%/%/%/%/*&^" }, StringSplitOptions.None);
        StudentIGN.text = studentInfo[1];
        StudentPassword.text = studentInfo[2];
        StudentCoins.text = studentInfo[3];

    }
    public void UpdateAssignmentRowObjects(string dbResponse)
    {
        Vector3 position;
        GameObject newObj;
        GameObject parent;
        string[] AssignmentInfo;
        string[] listOfAssignments = convertToList(dbResponse);
        // get the first object(it should exist already)
        GameObject firstObj = GameObject.Find("AssignmentRow");
        Debug.Log(firstObj.name);
        //assign the parent after we capture row
        parent = GameObject.Find("AssignmentTableRows");
        //split the first string in the array
        AssignmentInfo =  listOfAssignments[0].Split(new string[] { "%/%/%/%/%/%/%/%/%/%/*&^" }, StringSplitOptions.None);
        firstObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text =AssignmentInfo[0].ToString();
        firstObj.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text =AssignmentInfo[1].ToString();
        firstObj.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text =AssignmentInfo[2].ToString();
        

        //---------------Now spawn in objects underneath it--------------//

        for(int i = 1; i < listOfAssignments.Length -1; i++)
        {
            AssignmentInfo =  listOfAssignments[i].Split(new string[] { "%/%/%/%/%/%/%/%/%/%/*&^" }, StringSplitOptions.None);
            Debug.Log(AssignmentInfo[0]);
            Debug.Log(AssignmentInfo[1]);
            Debug.Log(AssignmentInfo[2]);
            position = findLowestRow(firstObj, "AssignmentRow");
            position.y -=150;

            newObj = Instantiate(firstObj, position, Quaternion.identity);
            newObj.transform.SetParent(parent.transform);
            newObj.tag = "Rows";
            newObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text =AssignmentInfo[0].ToString();
            newObj.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text =AssignmentInfo[1].ToString();
            newObj.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text =AssignmentInfo[2].ToString();
            

        }

    }

    public string[] convertToList(string dbResponse)
    {
        string[] split = dbResponse.Split(new string[] { "////////////////////%%%%%" }, StringSplitOptions.None);
        return split;
    }

    public Vector3 findLowestRow(GameObject obj, string ObjName)
    {
        GameObject lowestObject = obj;
        foreach (GameObject go in  GameObject.FindGameObjectsWithTag(ObjName))
        {
          if(go.transform.position.y < lowestObject.transform.position.y)
          {
            lowestObject = go;
        
          }
        }
        return lowestObject.transform.position;
    }
}
