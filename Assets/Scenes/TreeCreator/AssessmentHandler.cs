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

public class AssessmentHandler : MonoBehaviour
{
    public List<int> questionList = new List<int>{0};

    public string questionName;

    public TMP_InputField NameOfAssessment;

    public TMP_InputField DueDate;

    public TMP_InputField UnlockCriteriaAttemptsValue;

    public TMP_InputField UnlockCriteriaScoreValue;

    public Toggle DisplayAnswers;

    public Button New_Question_Button;

    public string courseName;




 



    // Start is called before the first frame update
    void Start()
    {
        New_Question_Button.onClick.AddListener(NewQuestionButton);
        LoadFromJson();

        //courseName = PlayerPrefs.GetString("CourseName");
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void NewQuestionButton()
    {
        Vector3 newButtonPosition =  FindLowestButton();
        newButtonPosition.y -=50;
        int number = addToQuestionList(questionList);
        Button newButton = Instantiate(New_Question_Button, newButtonPosition, Quaternion.identity);
        newButton.name = questionName;
        newButton.GetComponentInChildren<Text>().text = questionName;
        newButton.GetComponent<QuestionAttributes>().QuestionNumber = number;
        newButton.onClick.AddListener(delegate{ LoadCorrectScene(number); });
        setParent(newButton, GameObject.Find("Main"));


    }

    public Vector3 FindLowestButton()
    {
        Vector3 position = New_Question_Button.transform.position;

        GameObject lowestObject = GameObject.Find("NewQuestion");
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Button") as GameObject[])
        {

            if(lowestObject.transform.position.y > go.transform.position.y)
            {
                lowestObject = go;
            }
            

        }
        return lowestObject.transform.position;
    }
    public void setParent(Button childObject, GameObject parentObject)
    {
        // set parent
        childObject.transform.SetParent(parentObject.transform);

    }

    public int addToQuestionList(List<int> questionList)
    {
        int index = 0;
        while(index < questionList.Count)
        {
            Debug.Log(questionList[index]);
            index++;
        }
        questionList.Add(index);
        questionName = "Question " + index.ToString();
        return index;


    }

    public void LoadCorrectScene(int number)
    {
        SceneManager.LoadScene("EditAssessmentIcon", LoadSceneMode.Additive);
        PlayerPrefs.SetInt("QuestionNumber", number);
        Debug.Log("set number as: " +PlayerPrefs.GetInt("QuestionNumber"));

    }

    public void GoBack()
    {
        SceneManager.UnloadSceneAsync("AssessmentPage");
        Vector3 newCameraPosition = new Vector3( Camera.main.transform.position.x, 1095, 4965);
        Camera.main.transform.position = newCameraPosition;
    }

    public void LoadHelpScene()
    {
        SceneManager.LoadScene("ValueUnlockHelpScene", LoadSceneMode.Additive);
    }

    public void PlayerPref()
    {
        PlayerPrefs.SetString("NameOfAssessment", NameOfAssessment.text );
        PlayerPrefs.SetString("DueDate", DueDate.text);
        PlayerPrefs.SetString("UnlockCriteriaAttempts", "Attempts");
        PlayerPrefs.SetString("UnlockCriteriaAttemptsValue", UnlockCriteriaAttemptsValue.text);
        PlayerPrefs.SetString("UnlockCriteriaScoreValue", UnlockCriteriaScoreValue.text);
        //player prefs does not support bool, change it to string
        if(DisplayAnswers.isOn == true)
        {
            PlayerPrefs.SetString("DisplayAnswers", "true");
        }
        else if(DisplayAnswers.isOn == false)
        {
            PlayerPrefs.SetString("DisplayAnswers", "false");
        }

        Debug.Log("IM CALLED");
    }

    public void LoadFromJson()
    {
        // find the correct file from the folder
        Debug.Log(PlayerPrefs.GetString("ObjectToBeEditedLocation"));
        string filePath = Application.persistentDataPath + "/" + PlayerPrefs.GetString("ClassTag");
        string[] files = Directory.GetFiles(filePath, PlayerPrefs.GetString("ObjectToBeEditedLocation") + ".json", SearchOption.AllDirectories);
        //since we are looking up the direct path it will be first in string array
        // TODO check if the file is empty
        string correctFile = "";
        if(files.Length != 0)
        {
            correctFile = files[0];
            Vector3 newPosition = new Vector3(0, 0, 0);
            Button newButton;
            using (var fs = File.Open(correctFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    var json = sr.ReadToEnd();
                    var JsonObject = JsonUtility.FromJson<SaveDataHandler.CourseData>(json);
                    // loop through questions
                    //Debug.Log(JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[1].QuestionNumber);
                    int index = 0;
                    foreach (SaveDataHandler.Question question in JsonObject.MODULE.ASSESSMENTS.QUESTION.questions)
                    {
                        if (JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[index].QuestionNumber != 0)
                        {
                            
                            newPosition = FindLowestButton();
                            newPosition.y -= 50;
                            newButton = Instantiate(New_Question_Button, newPosition, Quaternion.identity);
                            newButton.name = "Question" + JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[index].QuestionNumber;
                            newButton.GetComponentInChildren<Text>().text = "Question " + JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[index].QuestionNumber;
                            int number = addToQuestionList(questionList);
                            newButton.GetComponent<QuestionAttributes>().QuestionNumber = number;
                            newButton.onClick.AddListener(() => { LoadCorrectScene(number); });
                            setParent(newButton, GameObject.Find("Main"));
                            index++;
                        }
                    }
                    // assign the assignment name and due date
                    GameObject nameInputField = GameObject.Find("Name");
                    nameInputField.GetComponent<TMP_InputField>().text = JsonObject.MODULE.ASSESSMENTS.AssessmentName;

                    GameObject DueDate = GameObject.Find("DueDateInput");
                    DueDate.GetComponent<TMP_InputField>().text = JsonObject.MODULE.ASSESSMENTS.DueDate;

                    // GameObject UnlockCriteriaType = GameObject.Find("CriteriaUnlockInput");
                    // UnlockCriteriaType.GetComponent<TMP_InputField>().text = JsonObject.MODULE.ASSESSMENTS.UnlockCriteriaType;

                    // GameObject UnlockCriteriaValue = GameObject.Find("UnlockCriteriaValueInput");
                    // UnlockCriteriaValue.GetComponent<TMP_InputField>().text = JsonObject.MODULE.ASSESSMENTS.UnlockCriteriaValue;

                    GameObject DisplayAnswersObj = GameObject.Find("DisplayAnswers");
                    DisplayAnswers.isOn = JsonObject.MODULE.ASSESSMENTS.DisplayAnswers;

                }
            }
        }
    }

}
