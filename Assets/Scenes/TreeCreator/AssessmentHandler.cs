using System.Collections;
using System.Collections.Generic;
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

    public Button button;

    public string courseName;




 



    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(NewQuestionButton);

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
        Button newButton = Instantiate(button, newButtonPosition, Quaternion.identity);
        newButton.name = questionName;
        newButton.GetComponentInChildren<Text>().text = questionName;
        newButton.GetComponent<QuestionAttributes>().QuestionNumber = number;
        newButton.onClick.AddListener( () => { LoadCorrectScene(newButton); });
        setParent(newButton, GameObject.Find("Main"));


    }

    public Vector3 FindLowestButton()
    {
        Vector3 position = button.transform.position;

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

    public void LoadCorrectScene(Button clickedButton)
    {
        SceneManager.LoadScene("EditAssessmentIcon", LoadSceneMode.Additive);
        PlayerPrefs.SetInt("QuestionNumber", clickedButton.GetComponent<QuestionAttributes>().QuestionNumber);
        Debug.Log("set number as: " +PlayerPrefs.GetInt("QuestionNumber"));

    }

    public void GoBack()
    {
        SceneManager.UnloadSceneAsync("AssessmentPage");
        Vector3 newCameraPosition = new Vector3( Camera.main.transform.position.x, 1095, 4965);
        Camera.main.transform.position = newCameraPosition;
    }

    public void SaveAssessment()
    {


        //save the Name of the assessment
        // PlayerPrefs.SetString("CS350-" + NameOfAssessment.text, NameOfAssessment.text);
        // // save the due date of the assessment
        // PlayerPrefs.SetString("CS350-" + NameOfAssessment.text + "-"+"DueDate", DueDate.text);


    }

    public void PlayerPref()
    {
        PlayerPrefs.SetString("NameOfAssessment", NameOfAssessment.text );
        PlayerPrefs.SetString("DueDate", DueDate.text);
    }

}
