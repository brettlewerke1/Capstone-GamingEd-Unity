using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UploadCourse : MonoBehaviour
{
    public string UploadCourseURL = "http://localhost/UnityApp/Upload.php";
    public SaveDataHandler save;

    public Slider ProgressSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {                
        if(ProgressSlider.value == 1)
        {
            Debug.Log("Upload Complete");
            //change scenes here
        }
    }

    public void GetFiles ()
    {
        string folderPath = Application.persistentDataPath + "/" + PlayerPrefs.GetString("ClassTag");
        string[] files = Directory.GetFiles(folderPath);
        save = new SaveDataHandler();

        foreach (string file in files)
        {
            //Debug.Log(file);
            using (var fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    var json = sr.ReadToEnd();
                    var JsonObject = JsonUtility.FromJson<SaveDataHandler.CourseData>(json);
                    // loop through questions



                    save._Course.CourseName = JsonObject.CourseName;
                    save._Course.Type = JsonObject.Type;
                    save._Course.MODULE.moduleNumber = JsonObject.MODULE.moduleNumber;
                    //do Assessment
                    save._Course.MODULE.ASSESSMENTS.AssessmentName = JsonObject.MODULE.ASSESSMENTS.AssessmentName;
                    save._Course.MODULE.ASSESSMENTS.DueDate = JsonObject.MODULE.ASSESSMENTS.DueDate;
                    save._Course.MODULE.ASSESSMENTS.UnlockCriteriaAttemptsValue =JsonObject.MODULE.ASSESSMENTS.UnlockCriteriaAttemptsValue;
                    save._Course.MODULE.ASSESSMENTS.UnlockCriteriaScoreValue =JsonObject.MODULE.ASSESSMENTS.UnlockCriteriaScoreValue;
                    save._Course.MODULE.ASSESSMENTS.DisplayAnswers = JsonObject.MODULE.ASSESSMENTS.DisplayAnswers;   


                    //do Assignment
                    save._Course.MODULE.ASSIGNMENT.AssignmentName = JsonObject.MODULE.ASSIGNMENT.AssignmentName;
                    save._Course.MODULE.ASSIGNMENT.DueDate = JsonObject.MODULE.ASSIGNMENT.DueDate;
                    save._Course.MODULE.ASSIGNMENT.Description = JsonObject.MODULE.ASSIGNMENT.Description;
                    save._Course.MODULE.ASSIGNMENT.Points = JsonObject.MODULE.ASSIGNMENT.Points;
                    save._Course.MODULE.ASSIGNMENT.UnlockCriteriaType = JsonObject.MODULE.ASSIGNMENT.UnlockCriteriaType;
                    save._Course.MODULE.ASSIGNMENT.UnlockCriteriaValue = JsonObject.MODULE.ASSIGNMENT.UnlockCriteriaValue;
                    save._Course.MODULE.ASSIGNMENT.Coins = JsonObject.MODULE.ASSIGNMENT.Coins;

                    if(save._Course.Type == "Assessment")
                    {
                        int index = 0;
                        foreach (SaveDataHandler.Question question in JsonObject.MODULE.ASSESSMENTS.QUESTION.questions)
                        {

                            int otherIndex=0;
                            if (question.QuestionNumber != 0)
                            {
                                save._Course.MODULE.ASSESSMENTS.QUESTION.newQuestionList(
                                    question.QuestionNumber,
                                    question.QuestionName,
                                    question.QuestionType
                                    );
                                //Debug.Log(JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[index].QuestionName);
                                // workaround because it adds a blank question everytime
                                // this happens because we have to assign the question number
                                // (when clicking on the button in Assessment Scene) to something in the list
                                save._Course.MODULE.ASSESSMENTS.QUESTION.questions.RemoveAt(save._Course.MODULE.ASSESSMENTS.QUESTION.questions.Count -1);

                                // save question type
                                string questionType =  question.QuestionType;


                                if (questionType == "Multiple Choice")
                                {
                                    foreach (SaveDataHandler.MultipleChoice multipleChoice in question.multipleChoices)
                                    {
                                        save._Course.MODULE.ASSESSMENTS.QUESTION.questions[index].updateMultipleChoicesList(
                                            multipleChoice.AnswerNumber,
                                            multipleChoice.AnswerName,
                                            multipleChoice.CorrectAnswer,
                                            multipleChoice.Points,
                                            multipleChoice.Coins
                                        );
                                        StartCoroutine(StartUploadCourse(save, index, otherIndex));
                                        otherIndex++;

                                    }

                                }
                                else if (questionType == "Fill_in_Blank")
                                {
                                    save._Course.MODULE.ASSESSMENTS.QUESTION.questions[index].fillBlankChoices.QuestionAnswer =
                                        JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[index].fillBlankChoices.QuestionAnswer;

                                    save._Course.MODULE.ASSESSMENTS.QUESTION.questions[index].fillBlankChoices.Points =
                                        JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[index].fillBlankChoices.Points;

                                    save._Course.MODULE.ASSESSMENTS.QUESTION.questions[index].fillBlankChoices.Coins =
                                        JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[index].fillBlankChoices.Coins;
                                    StartCoroutine(StartUploadCourse(save, index, otherIndex));

                                }
                                else if (questionType == "Matching")
                                {
                                    foreach (SaveDataHandler.Matching matching in question.matchingChoices)
                                    {
                                        save._Course.MODULE.ASSESSMENTS.QUESTION.questions[index].updateMatchingChoicesList(
                                            matching.AnswerNumber,
                                            matching.leftTwin,
                                            matching.rightTwin,
                                            matching.Coins,
                                            matching.Points
                                        );
                                        StartCoroutine(StartUploadCourse(save, index, otherIndex));
                                        otherIndex++;
                                    }
                                }
                                //StartCoroutine(StartUploadCourse(save, index, otherIndex));
                                

                                index++;
                            }

                        }
                    }
                    else if(save._Course.Type == "Assignment")
                    {
                        save._Course.MODULE.ASSIGNMENT.AssignmentName = JsonObject.MODULE.ASSIGNMENT.AssignmentName;
                        save._Course.MODULE.ASSIGNMENT.DueDate = JsonObject.MODULE.ASSIGNMENT.DueDate;
                        save._Course.MODULE.ASSIGNMENT.Description = JsonObject.MODULE.ASSIGNMENT.Description;
                        save._Course.MODULE.ASSIGNMENT.Points = JsonObject.MODULE.ASSIGNMENT.Points;
                        save._Course.MODULE.ASSIGNMENT.UnlockCriteriaType = JsonObject.MODULE.ASSIGNMENT.UnlockCriteriaType;
                        save._Course.MODULE.ASSIGNMENT.UnlockCriteriaValue = JsonObject.MODULE.ASSIGNMENT.UnlockCriteriaValue;
                        save._Course.MODULE.ASSIGNMENT.Coins = JsonObject.MODULE.ASSIGNMENT.Coins;
                        StartCoroutine(StartUploadCourse(save, 0, 0));
                        

                    }

                    
                    //end loop
                }
            }
            //StartCoroutine(StartUploadCourse(save));
        }


    }

    IEnumerator StartUploadCourse(SaveDataHandler savedData, int questionIndex, int answerIndex)
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("admin_username", PlayerPrefs.GetString("AdminUsername")));
        wwwForm.Add(new MultipartFormDataSection("course_name", savedData._Course.CourseName));
        wwwForm.Add(new MultipartFormDataSection("type", savedData._Course.Type));
        wwwForm.Add(new MultipartFormDataSection("level_number", savedData._Course.MODULE.moduleNumber.ToString()));
        

        // If the object is an assessment go here
        if(savedData._Course.Type == "Assessment")
        {
            wwwForm.Add(new MultipartFormDataSection("assessment_name", savedData._Course.MODULE.ASSESSMENTS.AssessmentName));
            wwwForm.Add(new MultipartFormDataSection("due_date", savedData._Course.MODULE.ASSESSMENTS.DueDate));
            wwwForm.Add(new MultipartFormDataSection("attempts_name", savedData._Course.MODULE.ASSESSMENTS.UnlockCriteriaAttempts));
            wwwForm.Add(new MultipartFormDataSection("attempts_value", savedData._Course.MODULE.ASSESSMENTS.UnlockCriteriaAttemptsValue));
            wwwForm.Add(new MultipartFormDataSection("score_name", savedData._Course.MODULE.ASSESSMENTS.UnlockCriteriaScore));
            wwwForm.Add(new MultipartFormDataSection("score_value", savedData._Course.MODULE.ASSESSMENTS.UnlockCriteriaScoreValue));
            //format true or false from bool to string
            string booleanToString =  savedData._Course.MODULE.ASSESSMENTS.DisplayAnswers.ToString();
            wwwForm.Add(new MultipartFormDataSection("display_answers", booleanToString));
            
            wwwForm.Add(new MultipartFormDataSection("question_number",
                savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].QuestionNumber.ToString()));

            wwwForm.Add(new MultipartFormDataSection("question_name",
                savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].QuestionName));

            wwwForm.Add(new MultipartFormDataSection("question_type",
                savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].QuestionType));

            // Send up Multiple Choice
            //
            if(savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].QuestionType == "Multiple Choice")
            {
                wwwForm.Add(new MultipartFormDataSection("answer_number",
                    savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].multipleChoices[answerIndex].AnswerNumber.ToString()));


                wwwForm.Add(new MultipartFormDataSection("answer_name",
                    savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].multipleChoices[answerIndex].AnswerName));
                
                // check for correct answer, if true then send up the correct answer
                if(savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].multipleChoices[answerIndex].CorrectAnswer)
                {
                    wwwForm.Add(new MultipartFormDataSection("correct_answer",
                        savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].multipleChoices[answerIndex].AnswerName));
                }
                //Debug.Log("After index error?");
                wwwForm.Add(new MultipartFormDataSection("points",
                    savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].multipleChoices[answerIndex].Points.ToString()));

                wwwForm.Add(new MultipartFormDataSection("coins",
                    savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].multipleChoices[answerIndex].Coins.ToString()));

            }
            // Send up Matching
            //
            else if(savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].QuestionType == "Matching")
            {
                wwwForm.Add(new MultipartFormDataSection("answer_number",
                    savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].matchingChoices[answerIndex].AnswerNumber.ToString()));

                wwwForm.Add(new MultipartFormDataSection("left_twin",
                    savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].matchingChoices[answerIndex].leftTwin));

                wwwForm.Add(new MultipartFormDataSection("right_twin",
                    savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].matchingChoices[answerIndex].rightTwin));

                wwwForm.Add(new MultipartFormDataSection("coins",
                    savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].matchingChoices[answerIndex].Coins.ToString()));

                wwwForm.Add(new MultipartFormDataSection("points",
                    savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].matchingChoices[answerIndex].Points.ToString()));

            }
            // Send up Fill_in_Blank
            //
            else if(savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].QuestionType == "Fill_in_Blank")
            {
                wwwForm.Add(new MultipartFormDataSection("correct_answer", 
                    savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].fillBlankChoices.QuestionAnswer));
                
                wwwForm.Add(new MultipartFormDataSection("points",
                    savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].fillBlankChoices.Points.ToString()));

                wwwForm.Add(new MultipartFormDataSection("coins",
                    savedData._Course.MODULE.ASSESSMENTS.QUESTION.questions[questionIndex].fillBlankChoices.Coins.ToString()));

            }
        }
        // Send up Assignments
        else if(savedData._Course.Type == "Assignment")
        {
            wwwForm.Add(new MultipartFormDataSection("assignment_name", savedData._Course.MODULE.ASSIGNMENT.AssignmentName));
            wwwForm.Add(new MultipartFormDataSection("due_date", savedData._Course.MODULE.ASSIGNMENT.DueDate));
            wwwForm.Add(new MultipartFormDataSection("description", savedData._Course.MODULE.ASSIGNMENT.Description));
            wwwForm.Add(new MultipartFormDataSection("points", savedData._Course.MODULE.ASSIGNMENT.Points));
            wwwForm.Add(new MultipartFormDataSection("unlock_criteria", savedData._Course.MODULE.ASSIGNMENT.UnlockCriteriaType));
            wwwForm.Add(new MultipartFormDataSection("unlock_value", savedData._Course.MODULE.ASSIGNMENT.UnlockCriteriaValue));
            wwwForm.Add(new MultipartFormDataSection("coins", savedData._Course.MODULE.ASSIGNMENT.Coins));
        }



        UnityWebRequest www = UnityWebRequest.Post(UploadCourseURL, wwwForm);
        yield return www.SendWebRequest();
            string text = www.downloadHandler.text;
            Debug.Log(text);
        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);

        }
        else
        {
        }

       


    }

    IEnumerator UploadQuestionsToCourse(SaveDataHandler.Question question)
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        UnityWebRequest www = UnityWebRequest.Post(UploadCourseURL, wwwForm);
        yield return www.SendWebRequest();
            string text = www.downloadHandler.text;
            Debug.Log(text);
        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);

        }
        else
        {
        }
    }
}
