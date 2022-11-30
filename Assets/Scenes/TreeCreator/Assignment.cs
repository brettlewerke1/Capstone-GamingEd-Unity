using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class Assignment : MonoBehaviour
{

    public InputField nameOfAssignment;
    public InputField startDate;
    public InputField dueDate;
    public TMP_InputField description;
    public InputField points;

    public InputField coins;

    public InputField attempts;
    public TMP_Dropdown dropdown;
    public Button saveButton;
    public Button newQuestion;
    readonly string postURL = "http://localhost/UnityApp/assignment.php";
    public Button submitButton;

    string Description;

    
    public float x = 3324;
    public float y = 0;
    public float z = 5435;

    public void Start()
    {
        //DontDestroyOnLoad(this);
        LoadAssignmentFromJson();
    }
    public void addNewAnswer()
    {
    }

    public void changeTag(int val)
    {
        if(val == 0)
        {
           
        }
        else if(val==1)
        {
            
        }
        else 
        {

        }

    }

    // IEnumerator NewAnswer()
    // {
    //     List<IMultipartFormSection> www = new List<IMultipartFormSection>();
    //     yield return www.SendWebRequest();

    // }
// mm/dd/yyyy

    public void GoBack()
    {
        SceneManager.UnloadSceneAsync("EditAssignmentIcon");
        Vector3 newCameraPosition = new Vector3( Camera.main.transform.position.x, 1095, 4965);
        Camera.main.transform.position = newCameraPosition;
    }

    public void SaveAssignment(Button button)
    {
        SaveDataHandler save = new SaveDataHandler();
        //-------------------------FIRST TIME FILE-----------------------//
        //
        //
        //
        //Debug.Log(button.transform.position);
        // find the nearest text input box
        //--------------------start saving stuff-----------------------//
        //save course name
        save._Course.CourseName = PlayerPrefs.GetString("ClassTag");
        //save the type
        save._Course.Type = "Assignment";
        //save course module
        save._Course.MODULE.moduleNumber = PlayerPrefs.GetInt("LevelNumber");
        //save assignment name
        save._Course.MODULE.ASSIGNMENT.AssignmentName = nameOfAssignment.text; 
        //save due date
        save._Course.MODULE.ASSIGNMENT.DueDate = dueDate.text;
        //save the description
        if(description.text != "")
        {
        save._Course.MODULE.ASSIGNMENT.Description = description.text;

        }
        //save the points
        save._Course.MODULE.ASSIGNMENT.Points = points.text;
        //save coins
        save._Course.MODULE.ASSIGNMENT.Coins = coins.text;
        //save 
        if(attempts.text != "")
        {
        save._Course.MODULE.ASSIGNMENT.UnlockCriteriaValue = attempts.text;
        }

        // //?? maybe using these
        // int index = dropdown.value;
        // string selectedOption = dropdown.options[index].text;
        
        save.SaveAssignmentIntoJson();
    }

    public void LoadAssignmentFromJson()
    {
        string filePath = Application.persistentDataPath + "/" + PlayerPrefs.GetString("ClassTag");
        string[] files = Directory.GetFiles(filePath, PlayerPrefs.GetString("ObjectToBeEditedLocation") + ".json", SearchOption.AllDirectories);
        string correctFile = "";
        if(files.Length != 0)
        {
            correctFile = files[0];
            Debug.Log(correctFile);
            Vector3 newPosition = new Vector3(0, 0, 0);
            Button newButton;
            using (var fs = File.Open(correctFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    var json = sr.ReadToEnd();
                    var JsonObject = JsonUtility.FromJson<SaveDataHandler.CourseData>(json);

                    nameOfAssignment.text = JsonObject.MODULE.ASSIGNMENT.AssignmentName; 
                    dueDate.text = JsonObject.MODULE.ASSIGNMENT.DueDate;
                    description.text = JsonObject.MODULE.ASSIGNMENT.Description;
                    points.text = JsonObject.MODULE.ASSIGNMENT.Points; 
                    coins.text = JsonObject.MODULE.ASSIGNMENT.Coins;
                    attempts.text = JsonObject.MODULE.ASSIGNMENT.UnlockCriteriaValue; 

                }
            }
        }
    }
// public void LoadFromJson()
//     {


//         //since we are looking up the direct path it will be first in string array
//         // TODO check if the file is empty

//             using (var fs = File.Open(correctFile, FileMode.Open, FileAccess.Read, FileShare.Read))
//             {
//                 using (var sr = new StreamReader(fs))
//                 {
//                     var json = sr.ReadToEnd();
//                     var JsonObject = JsonUtility.FromJson<SaveDataHandler.CourseData>(json);
//                     // loop through questions
//                     //Debug.Log(JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[1].QuestionNumber);
//                     string questionType = JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[PlayerPrefs.GetInt("QuestionNumber") -1].QuestionType;
//                     PlayerPrefs.SetString("QuestionType", questionType);
//                     QuestionName.text = JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[PlayerPrefs.GetInt("QuestionNumber") -1].QuestionName;
//                     Debug.Log(questionType);
                    
//                     int index = 0;
//                     LoadCorrectAnswerType(questionType, JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[PlayerPrefs.GetInt("QuestionNumber") -1]);
//                     // assign the assignment name and due date
//                     GameObject nameInputField = GameObject.Find("Name");
//                     nameInputField.GetComponent<TMP_InputField>().text = JsonObject.MODULE.ASSESSMENTS.AssessmentName;

//                     GameObject DueDate = GameObject.Find("DueDateInput");
//                     DueDate.GetComponent<TMP_InputField>().text = JsonObject.MODULE.ASSESSMENTS.DueDate;

//                 }
//             }
//     }
    

    
}
