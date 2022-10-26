using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class Assessment : MonoBehaviour
{

   
    public GameObject MultipleChoiceInputObject;

    public GameObject MatchingInputObject;

    public TMP_InputField QuestionName;

    public Button newQuestion;

    public Button newMultipleChoiceAnswer;

    public TMP_Dropdown dropdown;





    // Start is called before the first frame update
    void Start()
    {

    }

    //public UGUI TextComponent;

    public void onNewAnswerPoolButton()
    {
        
        //check which type of question we are doing so we can assign correct parent
        if(PlayerPrefs.GetString("TypeOfQuestion") == "MultipleChoice")
        {
            Vector3 position = FindLowestInputField().transform.position;

            position.x = 134;
            position.y -=25;
            //moveNewQuestionObject(position, "down");
            GameObject textinput = Instantiate(MultipleChoiceInputObject, position, Quaternion.identity);
            //move it under the parent
            GameObject parent = GameObject.Find("MainAssessment");
            textinput.transform.SetParent(parent.transform);
            // wipe date inside of object
            textinput.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = "";
            textinput.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().tag = "InputField";
            textinput.transform.GetChild(0).GetChild(1).GetComponent<Toggle>().isOn = false;
            textinput.GetComponent<AnswerAttributes>().number = countGameObjectsWithTag(textinput) -1;
            //addToAnswerPool();
        }
        else if(PlayerPrefs.GetString("TypeOfQuestion") == "Matching")
        {
            Vector3 position = FindLowestInputField().transform.position;

            position.x = 134;
            position.y -=45;
            //moveNewQuestionObject(position, "down");
            GameObject textinput = Instantiate(MatchingInputObject, position, Quaternion.identity);
            //move it under the parent
            GameObject parent = GameObject.Find("MainAssessment");
            textinput.transform.SetParent(parent.transform);
            // wipe date inside of object
            textinput.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = "";
            textinput.transform.GetChild(0).GetChild(2).GetComponent<TMP_InputField>().text = "";
            textinput.GetComponent<AnswerAttributes>().number = countGameObjectsWithTag(textinput) -1;

            //addToAnswerPool();
        }
    }

    public void onLessAnswerPoolButton()
    {
        GameObject objectToDestroy = null;
        //check which type of question we are doing so we can assign correct parent
        if(PlayerPrefs.GetString("TypeOfQuestion") == "MultipleChoice")
        {
            Vector3 position = MultipleChoiceInputObject.transform.position;
            /*GameObject[]*/ var objects = GameObject.FindGameObjectsWithTag("MultipleChoice");
            foreach (var choices in objects)
            {
                if(position.y > choices.transform.position.y)
                {
                    objectToDestroy = choices;
                }   
            }
            Destroy(objectToDestroy);
           // moveNewQuestionObject(position, "up");

        }
        else if(PlayerPrefs.GetString("TypeOfQuestion") == "Matching")
        {
            Vector3 position = MatchingInputObject.transform.position;
            /*GameObject[]*/ var objects = GameObject.FindGameObjectsWithTag("Matching");
            foreach (var choices in objects)
            {
                if(position.y > choices.transform.position.y)
                {
                    objectToDestroy = choices;
                }               
            }
            Destroy(objectToDestroy);
            //moveNewQuestionObject(position, "up");
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
        GameObject lowestObject = GameObject.Find("Header");

        GameObject[] inputFields = GameObject.FindGameObjectsWithTag("InputField");
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("InputField") as GameObject[])
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

    public void SaveAssessment(Button button)
    {
        SaveDataHandler save = new SaveDataHandler();
        Debug.Log("Im called right now");
        //-------------------------FIRST TIME FILE-----------------------//
        //
        //
        //
        if(PlayerPrefs.GetInt("QuestionNumber") == 1)
        {
        Debug.Log(button.transform.position);
        // find the nearest text input box
        GameObject parent = button.transform.parent.gameObject;
        GameObject grandfather = parent.transform.parent.gameObject;
        GameObject inputfield = parent.transform.GetChild(0).gameObject;

        //start saving stuff-----------------------//

        //save course name
        save._Course.CourseName = PlayerPrefs.GetString("CourseName");
        //save course module
        //save._Course.MODULE
        //save assessment name
        save._Course.MODULE.ASSESSMENTS.AssessmentName = PlayerPrefs.GetString("NameOfAssessment");
        //save due date
        save._Course.MODULE.ASSESSMENTS.DueDate = PlayerPrefs.GetString("DueDate");
        //save question type
        save._Course.MODULE.ASSESSMENTS.QUESTION.newQuestionList(PlayerPrefs.GetInt("QuestionNumber"), PlayerPrefs.GetString("QuestionName"), PlayerPrefs.GetString("QuestionType"));
        Debug.Log(PlayerPrefs.GetString("QuestionType"));
        if(PlayerPrefs.GetString("QuestionType") == "Multiple Choice")
        {
            Debug.Log(PlayerPrefs.GetString("QuestionType"));
           foreach (GameObject go in GameObject.FindGameObjectsWithTag("MultipleChoice") as GameObject[])
            {
                // loop through answers and save them
                save._Course.MODULE.ASSESSMENTS.QUESTION.
                questions[0].
                updateMultipleChoicesList(go.GetComponent<AnswerAttributes>().number,
                                  go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_InputField>().text,
                                  go.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Toggle>().isOn,
                                  int.Parse(go.transform.GetChild(0).GetChild(5).GetComponent<TMP_InputField>().text));
                Debug.Log("IM ASSSSS");
            
            

            } 
        }
        else if(PlayerPrefs.GetString("QuestionType") == "Matching")
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Matching") as GameObject[])
            {
                // loop through answers and save them
                save._Course.MODULE.ASSESSMENTS.QUESTION.
                questions[0].
                updateMatchingChoicesList(go.GetComponent<AnswerAttributes>().number,
                                          go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_InputField>().text,
                                          go.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TMP_InputField>().text,
                                          int.Parse(go.transform.GetChild(0).GetChild(4).gameObject.GetComponent<TMP_InputField>().text));
                Debug.Log("I am called");
            
            

            }
            

        }
        else if(PlayerPrefs.GetString("QuestionType") == "Fill_in_Blank")
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Fill_in_Blank") as GameObject[])
            {
                // loop through answers and save them
                save._Course.MODULE.ASSESSMENTS.QUESTION.
                questions[0].
                updateMatchingChoicesList(go.GetComponent<AnswerAttributes>().number,
                                          go.transform.GetChild(0).gameObject.GetComponent<TMP_InputField>().text,
                                          int.Parse(go.transform.GetChild(0).GetChild(4).gameObject.GetComponent<TMP_InputField>().text));
                Debug.Log("I am called");
            
            

            }
            

        }
        //save answer pool
        save.SaveIntoJson();
        }
        //-----------------------UPDATE EXISTING JSON FILE--------------------------//
        else 
        {
        // Debug.Log("Ready to update");
        // //save question type
        // save._Course.MODULE.ASSESSMENTS.QUESTION.newQuestionList(PlayerPrefs.GetInt("QuestionNumber"), PlayerPrefs.GetString("QuestionName"), PlayerPrefs.GetString("QuestionType"));
        // Debug.Log(save._Course.MODULE.ASSESSMENTS.QUESTION.questions[0].QuestionName);
        // if(PlayerPrefs.GetString("QuestionType") == "MultipleChoice")
        // {
        //    foreach (GameObject go in GameObject.FindGameObjectsWithTag("MultipleChoice") as GameObject[])
        //     {
        //         // loop through answers and save them
        //         save._Course.MODULE.ASSESSMENTS.QUESTION.
        //         questions[PlayerPrefs.GetInt("QuestionNumber") -1].
        //         updateMultipleChoicesList(go.GetComponent<AnswerAttributes>().number,
        //                           go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_InputField>().text,
        //                           go.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Toggle>().isOn,
        //                           int.Parse(go.transform.GetChild(0).GetChild(5).GetComponent<TMP_InputField>().text));
            
            

        //     } 
        // }
        // else if(PlayerPrefs.GetString("QuestionType") == "Matching")
        // {
        //     foreach (GameObject go in GameObject.FindGameObjectsWithTag("Matching") as GameObject[])
        //     {
        //         // loop through answers and save them
        //         save._Course.MODULE.ASSESSMENTS.QUESTION.
        //         questions[PlayerPrefs.GetInt("QuestionNumber") -1].
        //         updateMatchingChoicesList(go.GetComponent<AnswerAttributes>().number,
        //                                   go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_InputField>().text,
        //                                   go.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TMP_InputField>().text,
        //                                   int.Parse(go.transform.GetChild(0).GetChild(4).gameObject.GetComponent<TMP_InputField>().text));
        //         Debug.Log("I am called");
            
            

        //     }

        // }
        save.UpdateJson(PlayerPrefs.GetString("FilePath"));
        }
    }

    public void UpdatePlayerPrefs()
    {
        PlayerPrefs.SetString("QuestionName", QuestionName.text);
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
