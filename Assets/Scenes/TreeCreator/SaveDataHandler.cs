using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SaveDataHandler : MonoBehaviour
{
    [SerializeField] public CourseData _Course = new CourseData();



    public void SaveIntoJson()
    {
        Debug.Log(_Course);
        string Course = JsonUtility.ToJson(_Course);
        System.IO.File.WriteAllText( PlayerPrefs.GetString("FilePath"), Course);
    }

    public void UpdateJson(string FilePath)
    {
        SaveDataHandler save = new SaveDataHandler();
        
        Debug.Log("Ready to update");

        //save question type
        save._Course.MODULE.ASSESSMENTS.QUESTION.newQuestionList(PlayerPrefs.GetInt("QuestionNumber"), PlayerPrefs.GetString("QuestionName"), PlayerPrefs.GetString("QuestionType"));
        Debug.Log(save._Course.MODULE.ASSESSMENTS.QUESTION.questions[0].QuestionName);
        if(PlayerPrefs.GetString("QuestionType") == "Multiple Choice")
        {
           foreach (GameObject go in GameObject.FindGameObjectsWithTag("MultipleChoice") as GameObject[])
            {
                // loop through answers and save them
                save._Course.MODULE.ASSESSMENTS.QUESTION.
                questions[0].
                updateMultipleChoicesList(go.GetComponent<AnswerAttributes>().number,
                                  go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_InputField>().text,
                                  go.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Toggle>().isOn,
                                  int.Parse(go.transform.GetChild(0).GetChild(5).GetComponent<TMP_InputField>().text));
            
            

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
            
            

            }

        }

        Question saveData = save._Course.MODULE.ASSESSMENTS.QUESTION.questions[0];
        Question emptyQuestion = new Question();
        string JsonString;
        // JsonUtility.FromJsonOverWrite(s);
        using( var fs = File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using(var sr = new StreamReader(fs))
            {
                var json = sr.ReadToEnd();                                                                                                                                                                                                                                                                                                                                  
                var JsonObject = JsonUtility.FromJson<CourseData>(json);
                //string stringNum = PlayerPrefs.GetString("QuestionNumber");
                //Debug.Log(stringNum);
                //int questionNum = int.Parse();
                //Debug.Log(questionNum);
                Question questionToUpdate = JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[PlayerPrefs.GetInt("QuestionNumber") - 1];
                questionToUpdate.QuestionNumber = saveData.QuestionNumber;
                Debug.Log(saveData.QuestionName + "Quesiton name");
                questionToUpdate.QuestionName = saveData.QuestionName;
                questionToUpdate.QuestionType = saveData.QuestionType;
                questionToUpdate.matchingChoices = saveData.matchingChoices;
                questionToUpdate.multipleChoices = saveData.multipleChoices;
                questionToUpdate.fillBlankChoices = saveData.fillBlankChoices;
                JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[PlayerPrefs.GetInt("QuestionNumber") - 1] = questionToUpdate;
                JsonObject.MODULE.ASSESSMENTS.QUESTION.newQuestionList(0, "", "");
                JsonString = JsonUtility.ToJson(JsonObject);


            }
        }
        System.IO.File.WriteAllText(PlayerPrefs.GetString("FilePath"), JsonString);


    }


    //------------------------------------------------//
    [System.Serializable]
    public class CourseData
    {
        public string CourseName = "";

        public Module MODULE = new Module();





    }
    [System.Serializable]
    public class Module
    {

        public int moduleNumber;

        public Assessments ASSESSMENTS = new Assessments();



    }

    [System.Serializable]
    public class Assessments
    {
        public string AssessmentName = "";

        public string DueDate;

        public QuestionObj QUESTION = new QuestionObj();

    }


    [System.Serializable]
    public class QuestionObj
    {
        public List<Question> questions = new List<Question>();

        public void newQuestionList(int questionNumber, string questionName, string questionType)
        {
            Question question = new Question();
            question.QuestionName = questionName;
            question.QuestionNumber = questionNumber;
            question.QuestionType = questionType;
            questions.Add(question);
            Question blankQuestion = new Question();
            questions.Add(blankQuestion);
        }


    }

    [System.Serializable]
    public class Question
    {
        public int QuestionNumber=0;

        public string QuestionName="";

        public string QuestionType = "";

        public List<MultipleChoice> multipleChoices = new List<MultipleChoice>();

        public List<Fill_in_Blank> fillBlankChoices = new List<Fill_in_Blank>();

        public List<Matching> matchingChoices = new List<Matching>();

        public void updateMultipleChoicesList(int answerNumber, string multipleChoiceAnswer, bool isCorrectAnswer, int points)
        {
            MultipleChoice answer = new MultipleChoice();
            answer.AnswerNumber = answerNumber;
            answer.AnswerName = multipleChoiceAnswer;
            answer.CorrectAnswer = isCorrectAnswer;
            answer.Points = points;
            multipleChoices.Add(answer);
            Debug.Log("IM ASS");
        }

        public void updateMatchingChoicesList(int answerNumber, string leftTwin, string rightTwin, int points)
        {
            Matching answer = new Matching();
            answer.AnswerNumber = answerNumber;
            answer.leftTwin = leftTwin;
            answer.rightTwin = rightTwin;
            answer.Points = points;
            matchingChoices.Add(answer);
        }

    }

    [System.Serializable]
    public class MultipleChoice
    {
        public int AnswerNumber;
        public string AnswerName = "";

        public bool CorrectAnswer = false;

        public int Points;

    }

    [System.Serializable]
    public class Fill_in_Blank
    {
        public string QuestionName = "";

        public string QuestionAnswer="";

    }

    [System.Serializable]
    public class Matching
    {
        public int AnswerNumber;
        public string leftTwin = "";

        public string rightTwin = "";

        public int Points;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
