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
    public void SaveGateIntoJson()
    {
        string Course = JsonUtility.ToJson(_Course);
        System.IO.File.WriteAllText(PlayerPrefs.GetString("FilePath"), Course);
    }

    public void SaveAssignmentIntoJson()
    {
        string Course = JsonUtility.ToJson(_Course);
        System.IO.File.WriteAllText(PlayerPrefs.GetString("FilePath"), Course);
    }



    public void SaveAssessmentIntoJson()
    {
        Debug.Log(Application.persistentDataPath);
        string Course = JsonUtility.ToJson(_Course);
        System.IO.File.WriteAllText( PlayerPrefs.GetString("FilePath"), Course);
    }

    public void UpdateAssessmentJson(string FilePath)
    {
        SaveDataHandler save = new SaveDataHandler();
        //save question type
        save._Course.MODULE.ASSESSMENTS.QUESTION.newQuestionList(PlayerPrefs.GetInt("QuestionNumber"), PlayerPrefs.GetString("QuestionName"), PlayerPrefs.GetString("QuestionType"));
        //Debug.Log(save._Course.MODULE.ASSESSMENTS.QUESTION.questions[0].QuestionName);
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
                //parameters are int answerNumber, string multipleChoiceAnswer, bool isCorrectAnswer, int points, int coins, int attempts
            

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
                //string type = JsonObject.Type;
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

        public string Type;

        public Module MODULE = new Module();





    }
    [System.Serializable]
    public class Module
    {

        public int moduleNumber;

        public Assessments ASSESSMENTS = new Assessments();

        public Assignments ASSIGNMENT = new Assignments();

        public Gate GATE = new Gate();



    }

    [System.Serializable]
    public class Assignments
    {
        public string AssignmentName = "";

        public string DueDate;

        public string Description = " ";

        public string Points;

        public string UnlockCriteriaType = "Attempt";

        public string UnlockCriteriaValue = "3";

        public string Coins;
    }

    [System.Serializable]
    public class Assessments
    {
        public string AssessmentName = "";

        public string DueDate;

        public string UnlockCriteriaAttempts = "Attempts";

        public string UnlockCriteriaAttemptsValue = "3";

        public string UnlockCriteriaScore = "Score";

        public string UnlockCriteriaScoreValue = "60";

        public bool DisplayAnswers;

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

        public Fill_in_Blank fillBlankChoices = new Fill_in_Blank();

        public List<Matching> matchingChoices = new List<Matching>();

        public void updateMultipleChoicesList(int answerNumber, string multipleChoiceAnswer, bool isCorrectAnswer, int points, int coins)
        {
            MultipleChoice answer = new MultipleChoice();
            answer.AnswerNumber = answerNumber;
            answer.AnswerName = multipleChoiceAnswer;
            answer.CorrectAnswer = isCorrectAnswer;
            answer.Points = points;
            answer.Coins = coins;
            multipleChoices.Add(answer);
        }

        public void deleteMultipleChoiceFromList(int index)
        {
            multipleChoices.RemoveAt(index);
        }

        public void updateMatchingChoicesList(int answerNumber, string leftTwin, string rightTwin, int points,int coins)
        {
            Matching answer = new Matching();
            answer.AnswerNumber = answerNumber;
            answer.leftTwin = leftTwin;
            answer.rightTwin = rightTwin;
            answer.Points = points;
            answer.Coins = coins;
            matchingChoices.Add(answer);
        }

        public void deleteMatchingChoiceFromList(int index)
        {
            multipleChoices.RemoveAt(index);
        }

    }

    [System.Serializable]
    public class MultipleChoice
    {
        public int AnswerNumber;
        public string AnswerName = "";

        public bool CorrectAnswer = false;

        public int Points;

        public int Coins;

    }

    [System.Serializable]
    public class Fill_in_Blank
    {

        public string QuestionAnswer="";

        public int Points;

        public int Coins;

    }

    [System.Serializable]
    public class Matching
    {
        public int AnswerNumber;
        public string leftTwin = "";

        public string rightTwin = "";

        public int Coins;

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
