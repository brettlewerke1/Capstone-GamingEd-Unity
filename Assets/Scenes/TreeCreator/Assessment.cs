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


public class Assessment : MonoBehaviour
{

    //these is the parent of the input objects
    public GameObject MultipleChoiceObject;

    public GameObject MatchingObject;

    public GameObject Fill_in_BlankObject;

    //these are the children
    public GameObject MultipleChoiceInputObject;

    public GameObject MatchingInputObject;

    public GameObject Fill_in_BlankInputObject;

    public TMP_InputField QuestionName;

    public Button newQuestion;

    public Button newMultipleChoiceAnswer;

    public TMP_Dropdown dropdown;





    // Start is called before the first frame update
    void Start()
    {
        LoadFromJson();

    }

    //public UGUI TextComponent;

    public void onNewAnswerPoolButton()
    {
        
        //check which type of question we are doing so we can assign correct parent
        if(PlayerPrefs.GetString("QuestionType") == "Multiple Choice")
        {
            Vector3 position = FindLowestInputField().transform.localPosition;
            position.y -=100;
            //textinput.transform.SetParent(parent.transform);
            //moveNewQuestionObject(position, "down");
            GameObject textinput = Instantiate(MultipleChoiceInputObject, MultipleChoiceObject.transform, false);
            textinput.transform.localPosition = position;
            //move it under the parent
            // wipe date inside of object
            textinput.transform.GetChild(0).GetComponent<TMP_InputField>().text = "";
            textinput.transform.GetChild(0).GetComponent<TMP_InputField>().tag = "InputField";
            textinput.transform.GetChild(1).GetComponent<Toggle>().isOn = false;
            textinput.GetComponent<AnswerAttributes>().number = countGameObjectsWithTag(textinput) -1;
            //addToAnswerPool();
        }
        else if(PlayerPrefs.GetString("QuestionType") == "Matching")
        {
            Vector3 position = FindLowestInputField().transform.localPosition;
            position.y -=100;
            //moveNewQuestionObject(position, "down");
            GameObject textinput = Instantiate(MatchingInputObject, MatchingObject.transform,false);
            textinput.transform.localPosition = position;
            //move it under the parent
            //GameObject parent = GameObject.Find("MainAssessment");
            //textinput.transform.SetParent(parent.transform);
            // wipe date inside of object
            textinput.transform.GetChild(0).GetComponent<TMP_InputField>().text = "";
            textinput.transform.GetChild(2).GetComponent<TMP_InputField>().text = "";
            textinput.GetComponent<AnswerAttributes>().number = countGameObjectsWithTag(textinput) -1;

            //addToAnswerPool();
        }
    }

    public void onLessAnswerPoolButton(GameObject objectToBeDeleted)
    {
        if(objectToBeDeleted.GetComponent<AnswerAttributes>().number !=0)
        {
            Destroy(FindLowestInputField());
            // // find the correct file from the folder
            // Debug.Log(PlayerPrefs.GetString("ObjectToBeEditedLocation"));
            // string filePath = Application.persistentDataPath + "/" + PlayerPrefs.GetString("ClassTag");
            // string[] files = Directory.GetFiles(filePath, PlayerPrefs.GetString("ObjectToBeEditedLocation") + ".json", SearchOption.AllDirectories);
            // //since we are looking up the direct path it will be first in string array
            // // TODO check if the file is empty
            // string correctFile = files[0];
            // if (correctFile.Length != 0)
            // {
            //     string JsonString;
            //     using (var fs = File.Open(correctFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            //     {
            //         using (var sr = new StreamReader(fs))
            //         {
            //             var json = sr.ReadToEnd();
            //             var JsonObject = JsonUtility.FromJson<SaveDataHandler.CourseData>(json);
            //             // loop through questions
            //             //Debug.Log(JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[1].QuestionNumber);

            //             int questionNumber = int.Parse(PlayerPrefs.GetString("QuestionNumber")) - 1;
            //             JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[questionNumber].deleteMultipleChoiceFromList(objectToBeDeleted.GetComponent<AnswerAttributes>().number);
            //             // assign the assignment name and due date
            //             JsonString = JsonUtility.ToJson(JsonObject);

            //         }
            //     }
            //     System.IO.File.WriteAllText(PlayerPrefs.GetString("FilePath"), JsonString);
            // }
        }
    }

    public void onNewQuestionButton()
    {
        GameObject questionObject = GameObject.Find("Question");
        // lets spawn new question under the new question button
        GameObject questionButton = GameObject.Find("NewQuestion");
        // get location of question button and spawn underneath it
        Vector3 location = new Vector3(325
                                        ,questionButton.transform.position.y-200
                                        ,questionButton.transform.position.z );
        GameObject newQuestionObject = Instantiate(questionObject, location, Quaternion.identity);
        GameObject parent = GameObject.Find("MainAssessment");
        newQuestionObject.transform.SetParent(parent.transform);
        //moveNewQuestionObject(FindLowestInputField().transform.position, "down");
    }

    public void moveNewQuestionObject(Vector3 position, string direction)
    {
        Vector3 newPosition = FindLowestInputField().transform.position;
        if(direction == "down")
        {
            newPosition = new Vector3(position.x+100, position.y-200, position.z);
        }
        else if(direction == "up")
        {
            newPosition = new Vector3(position.x-100, position.y-200, position.z);         
        }
        GameObject newQuestionButton = GameObject.Find("NewQuestion");
        newQuestionButton.transform.position = newPosition;
    }

    public GameObject FindLowestInputField()
    {
        // need a reference point so find positon of header
        GameObject lowestObject = GameObject.Find("DropdownText");
        // we have the playerprefs alredy saved so no need for an if statement for each type
        // just string contatinate
        string questionType = PlayerPrefs.GetString("QuestionType");
        questionType = questionType.Replace(" ", "");
        Debug.Log(questionType);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(questionType + "Input") as GameObject[])
        {
            if(lowestObject.transform.position.y > go.transform.position.y)
            {
                lowestObject = go;
            }
            

        }
        return lowestObject;

    }

    public int countGameObjectsWithTag(GameObject gameObject)
    {
        int count = 0;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(gameObject.tag) as GameObject[])
        {

            count++;
            

        }
        return count;
    }

    public void GoBack()
    {
        SceneManager.UnloadSceneAsync("EditAssessmentIcon");
    }

    public void SaveAssessment()
    {
        SaveDataHandler save = new SaveDataHandler();
        //Debug.Log("Im called right now");
        //-------------------------FIRST TIME FILE-----------------------//
        //
        //
        //
        if(PlayerPrefs.GetInt("QuestionNumber") == 1)
        {
        // //Debug.Log(button.transform.position);
        // // find the nearest text input box
        // GameObject parent = button.transform.parent.gameObject;
        // GameObject grandfather = parent.transform.parent.gameObject;
        // GameObject inputfield = parent.transform.GetChild(0).gameObject;

        //--------------------start saving stuff-----------------------//
        //save course name
        save._Course.CourseName = PlayerPrefs.GetString("ClassTag");
        //save the typr of icon
        save._Course.Type = "Assessment";
        //save course module
        save._Course.MODULE.moduleNumber = PlayerPrefs.GetInt("LevelNumber");
        //save assessment name
        save._Course.MODULE.ASSESSMENTS.AssessmentName = PlayerPrefs.GetString("NameOfAssessment");
        //save due date
        save._Course.MODULE.ASSESSMENTS.DueDate = PlayerPrefs.GetString("DueDate");

        //save._Course.MODULE.ASSESSMENTS.UnlockCriteriaType = PlayerPrefs.GetString("UnlockCriteria");

        //if the user did not imput anything for this, it will default
        if(PlayerPrefs.GetString("UnlockCriteriaAttemptsValue") != "")
        {
            save._Course.MODULE.ASSESSMENTS.UnlockCriteriaAttemptsValue = PlayerPrefs.GetString("UnlockCriteriaAttemptsValue");
        }
        //if the user did not imput anything for this, it will default
        if(PlayerPrefs.GetString("UnlockCriteriaScoreValue") != "")
        {
            save._Course.MODULE.ASSESSMENTS.UnlockCriteriaScoreValue = PlayerPrefs.GetString("UnlockCriteriaScoreValue");
        }

        save._Course.MODULE.ASSESSMENTS.DisplayAnswers = PlayerPrefs.GetString("DisplayAnswers") == "true";
        Debug.Log(save._Course.MODULE.ASSESSMENTS.DisplayAnswers);
        //save question type
        save._Course.MODULE.ASSESSMENTS.QUESTION.newQuestionList(PlayerPrefs.GetInt("QuestionNumber"), PlayerPrefs.GetString("QuestionName"), PlayerPrefs.GetString("QuestionType"));
        if(PlayerPrefs.GetString("QuestionType") == "Multiple Choice")
        {
           foreach (GameObject go in GameObject.FindGameObjectsWithTag("MultipleChoiceInput") as GameObject[])
            {
                // loop through answers and save them
                save._Course.MODULE.ASSESSMENTS.QUESTION.
                questions[0].
                updateMultipleChoicesList(go.GetComponent<AnswerAttributes>().number,
                                  go.transform.GetChild(0).gameObject.GetComponent<TMP_InputField>().text,
                                  go.transform.GetChild(1).gameObject.GetComponent<Toggle>().isOn,
                                  int.Parse(go.transform.GetChild(5).GetComponent<TMP_InputField>().text),
                                  int.Parse(go.transform.GetChild(6).GetComponent<TMP_InputField>().text));
            
            

            } 
        }
        else if(PlayerPrefs.GetString("QuestionType") == "Matching")
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("MatchingInput") as GameObject[])
            {
                // loop through answers and save them
                save._Course.MODULE.ASSESSMENTS.QUESTION.
                questions[0].
                updateMatchingChoicesList(go.GetComponent<AnswerAttributes>().number,
                                          go.transform.GetChild(0).gameObject.GetComponent<TMP_InputField>().text,
                                          go.transform.GetChild(2).gameObject.GetComponent<TMP_InputField>().text,
                                          int.Parse(go.transform.GetChild(6).gameObject.GetComponent<TMP_InputField>().text),
                                          int.Parse(go.transform.GetChild(5).gameObject.GetComponent<TMP_InputField>().text));
            
            

            }
            

        }
        else if(PlayerPrefs.GetString("QuestionType") == "Fill_in_Blank")
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Fill_in_Blank") as GameObject[])
            {
                // loop through answers and save them
                save._Course.MODULE.ASSESSMENTS.QUESTION.
                questions[0].fillBlankChoices.QuestionAnswer = go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_InputField>().text;

                save._Course.MODULE.ASSESSMENTS.QUESTION.
                questions[0].fillBlankChoices.Coins = int.Parse(go.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);

                save._Course.MODULE.ASSESSMENTS.QUESTION.
                questions[0].fillBlankChoices.Points = int.Parse(go.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TMP_InputField>().text);
            
            

            }
            

        }
        //save answer pool
        save.SaveAssessmentIntoJson();
        }
        //-----------------------UPDATE EXISTING JSON FILE--------------------------//
        else 
        {
        save.UpdateAssessmentJson(PlayerPrefs.GetString("FilePath"));
        }
    }

    public void UpdatePlayerPrefs()
    {
        PlayerPrefs.SetString("QuestionName", QuestionName.text);
    }

    public void LoadFromJson()
    {

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
                    string questionType = JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[PlayerPrefs.GetInt("QuestionNumber") -1].QuestionType;
                    PlayerPrefs.SetString("QuestionType", questionType);
                    QuestionName.text = JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[PlayerPrefs.GetInt("QuestionNumber") -1].QuestionName;
                    Debug.Log(questionType);
                    
                    int index = 0;
                    LoadCorrectAnswerType(questionType, JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[PlayerPrefs.GetInt("QuestionNumber") -1]);
                    // assign the assignment name and due date
                    GameObject nameInputField = GameObject.Find("Name");
                    nameInputField.GetComponent<TMP_InputField>().text = JsonObject.MODULE.ASSESSMENTS.AssessmentName;

                    GameObject DueDate = GameObject.Find("DueDateInput");
                    DueDate.GetComponent<TMP_InputField>().text = JsonObject.MODULE.ASSESSMENTS.DueDate;

                }
            }
        }
    }

    public void LoadCorrectAnswerType(string type, SaveDataHandler.Question question)
    {
        int dropdownIndex;
        // spawn in multiple choice
        if(type == "Multiple Choice")
        {

            dropdownIndex = 0;
            TMP_Dropdown dropdown = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
            dropdown.value = dropdownIndex;
            foreach (SaveDataHandler.MultipleChoice answer in question.multipleChoices)
            {
                if(answer.AnswerNumber == 0)
                {
                    GameObject mainAssessmentObj = GameObject.Find("MainAssessment");
                    setParent(MultipleChoiceObject, mainAssessmentObj);
                    Vector3 tempLocation = mainAssessmentObj.transform.localPosition;
                    tempLocation.y -= 300;
                    MultipleChoiceObject.transform.localPosition = tempLocation;
                    MultipleChoiceInputObject.transform.GetChild(0).gameObject.GetComponent<TMP_InputField>().text = answer.AnswerName;
                    MultipleChoiceInputObject.transform.GetChild(1).gameObject.GetComponent<Toggle>().isOn = answer.CorrectAnswer;
                    MultipleChoiceInputObject.transform.GetChild(5).GetComponent<TMP_InputField>().text = answer.Points.ToString();
                    MultipleChoiceInputObject.transform.GetChild(6).GetComponent<TMP_InputField>().text = answer.Coins.ToString();


                }
                else 
                {
                    Vector3 position = FindLowestInputField().transform.localPosition;
                    position.y -=100;
                    //textinput.transform.SetParent(parent.transform);
                    //moveNewQuestionObject(position, "down");
                    GameObject textinput = Instantiate(MultipleChoiceInputObject, MultipleChoiceObject.transform, false);
                    textinput.transform.localPosition = position;
                    textinput.transform.GetChild(0).gameObject.GetComponent<TMP_InputField>().text = answer.AnswerName;
                    textinput.transform.GetChild(1).gameObject.GetComponent<Toggle>().isOn = answer.CorrectAnswer;
                    textinput.transform.GetChild(5).GetComponent<TMP_InputField>().text = answer.Points.ToString();
                    textinput.transform.GetChild(6).GetComponent<TMP_InputField>().text = answer.Coins.ToString();
                    textinput.GetComponent<AnswerAttributes>().number = countGameObjectsWithTag(textinput) -1;
                    
                }

                
            }
        }
        else if(type == "Matching")
        {
            dropdownIndex = 2;
            TMP_Dropdown dropdown = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
            dropdown.value = dropdownIndex;
            foreach (SaveDataHandler.Matching answer in question.matchingChoices)
            {
                if(answer.AnswerNumber == 0)
                {
                    GameObject mainAssessmentObj = GameObject.Find("MainAssessment");
                    setParent(MatchingObject, mainAssessmentObj);
                    Vector3 tempLocation = mainAssessmentObj.transform.localPosition;
                    tempLocation.y -= 100;
                    MatchingInputObject.transform.localPosition = tempLocation;
                    MatchingInputObject.transform.GetChild(0).gameObject.GetComponent<TMP_InputField>().text = answer.leftTwin;
                    MatchingInputObject.transform.GetChild(2).gameObject.GetComponent<TMP_InputField>().text = answer.rightTwin;
                    MatchingInputObject.transform.GetChild(5).gameObject.GetComponent<TMP_InputField>().text = answer.Coins.ToString();
                    MatchingInputObject.transform.GetChild(6).gameObject.GetComponent<TMP_InputField>().text = answer.Points.ToString();


                }
                else 
                {
                    Vector3 position = FindLowestInputField().transform.localPosition;
                    position.y -=100;
                    //textinput.transform.SetParent(parent.transform);
                    //moveNewQuestionObject(position, "down");
                    GameObject textinput = Instantiate(MatchingInputObject, MatchingObject.transform, false);
                    textinput.transform.localPosition = position;
                    MatchingInputObject.transform.GetChild(0).gameObject.GetComponent<TMP_InputField>().text = answer.leftTwin;
                    MatchingInputObject.transform.GetChild(2).gameObject.GetComponent<TMP_InputField>().text = answer.rightTwin;
                    MatchingInputObject.transform.GetChild(5).gameObject.GetComponent<TMP_InputField>().text = answer.Coins.ToString();
                    MatchingInputObject.transform.GetChild(6).gameObject.GetComponent<TMP_InputField>().text = answer.Points.ToString();
                    textinput.GetComponent<AnswerAttributes>().number = countGameObjectsWithTag(textinput) -1;
                    
                }

                
            }
        }
        else if(type == "Fill_in_Blank")
        {
            dropdownIndex = 1;
            TMP_Dropdown dropdown = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
            dropdown.value = dropdownIndex;
            GameObject mainAssessmentObj = GameObject.Find("MainAssessment");
            setParent(Fill_in_BlankObject, mainAssessmentObj);
            Vector3 tempLocation = mainAssessmentObj.transform.localPosition;
            tempLocation.y -= 100;
            Fill_in_BlankInputObject.transform.localPosition = tempLocation;
            Fill_in_BlankInputObject.transform.GetChild(0).GetComponent<TMP_InputField>().text = question.fillBlankChoices.QuestionAnswer;
            Fill_in_BlankInputObject.transform.GetChild(1).GetComponent<TMP_InputField>().text = question.fillBlankChoices.Coins.ToString();
            Fill_in_BlankInputObject.transform.GetChild(2).GetComponent<TMP_InputField>().text = question.fillBlankChoices.Points.ToString();


        }
    }

    public void setParent(GameObject childObject, GameObject parentObject)
    {
        // set parent
        childObject.transform.SetParent(parentObject.transform);

    }

    // public void addToAnswerPool()
    // {
    //     int index = 0;
    //     while(index < AnswerPoolList.Count)
    //     {
    //         Debug.Log(AnswerPoolList[index].number);
    //         index++;
    //     }
    //     AnswerPool pool = new AnswerPool();
    //     pool.number = index;
    //     Debug.Log("Index: " + index + " added");
    //     AnswerPoolList.Add(pool);


    // }



    // public GameObject findNearestInputBox(Vector3 position)
    // {
    //     GameObject[] inputFields = GameObject.FindGameObjectsWithTag("InputField");

    //     foreach (GameObject go in GameObject.FindGameObjectsWithTag("InputField") as GameObject[])
    //     {
          
    //         if(go.GetComponent<AnswerAttributes>() != null)
    //         {
                
    //         }
            

    //     } 
    //     return nearestBox;
    // }

}
