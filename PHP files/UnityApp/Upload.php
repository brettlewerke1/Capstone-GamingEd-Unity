<?php

$con = mysqli_connect('gaminged-db.czq2j2udebs7.us-west-2.rds.amazonaws.com', 'superAdmin', 'ghahyat8', 'RTX');
// Assign variables for data being passed to here from UploadCourse.cs
// these will be in order of when they are passed into the form starting on line 190
$admin_username = $_POST["admin_username"];
$course_name = $_POST["course_name"];
$course_tag = $_POST["course_tag"];
$join_key = $_POST["join_key"];
$type = $_POST["type"];
$level_number = $_POST["level_number"];

//default values
$display_answers = 1;
$answer_number = "";
$answer_name = "";
$correct_answer = "";
$points = "";
$coins = "";
$left_twin = "";
$right_twin = "";
$correct_answer = "";
$assessment_name = "";
//make some basic requests
// so can can get information from the course

$admin_string = "SELECT * FROM RTX.Account WHERE Account_Username = '".$admin_username."';";
$execute = mysqli_query($con, $admin_string) or die("2:...DB issue");
$rows = array();
while($result =mysqli_fetch_assoc($execute))
{
    $rows[] = $result;

}
foreach($rows as $row){
    $account_ID = $row["Account_ID"];
}


//make the module ID's
if($type == "Assessment")
{
    $module_ID = 2;
}
else if($type == "Assignment")
{
    $module_ID = 3;
}



if($type == "Assessment")
{
    $assessment_name = $_POST["assessment_name"];
    $due_date = $_POST["due_date"];
    $attempts_name = $_POST["attempts_name"];
    $attempts_value = $_POST["attempts_value"];
    $score_name = $_POST["score_name"];
    $score_value = $_POST["score_value"];
    //convert to boolean
    $display_answers = $_POST["display_answers"];
    if($display_answers == "True")
    {
        $display_answers = '1';
    }
    else
    {
        $display_answers = 0;
    }
    $question_number = $_POST["question_number"];
    $question_name = $_POST["question_name"];
    $question_type = $_POST["question_type"];

    //now do each type of question
    if($question_type == "Multiple Choice")
    {
        $answer_number = $_POST["answer_number"];
        $answer_name = $_POST["answer_name"];
        $correct_answer = $_POST["correct_answer"];
        $points = $_POST["points"];
        $coins = $_POST["coins"];
    }
    else if($question_type == "Matching")
    {
        $answer_number = $_POST["answer_number"];
        $left_twin = $_POST["left_twin"];
        $right_twin = $_POST["right_twin"];
        $coins = $_POST["coins"];
        $points = $_POST["points"];
    }
    else if($question_type == "Fill_in_Blank")
    {
        $correct_answer = $_POST["correct_answer"];
        $points = $_POST["points"];
        $coins = $_POST["coins"];
    }
    echo(" ".$assessment_name. " ".$level_number);
}
else if($type == "Assignment")
{
    $assignment_name = $_POST["assignment_name"];
    $due_date = $_POST["due_date"];
    $description = $_POST["description"];
    $points = $_POST["points"];
    $unlock_criteria = $_POST["unlock_criteria"];
    $unlock_value = $_POST["unlock_value"];
    $coins = $_POST["coins"];
    echo($assignment_name);
}

//----------------Send the request---------------------//


// add course to db
insertCourseIntoDB($course_name, $course_tag, $join_key, $con);

//get course_ID to save time and resources
$query = "SELECT * FROM RTX.Course WHERE Course_Name = '".$course_name."' AND Course_Tag = '".$course_tag."' AND Course_Key = '".$join_key."';";
$excecute = mysqli_query($con, $query) or die("2:...DB issue");
$rows = array();
while($result =mysqli_fetch_assoc($excecute))
{
    $rows[] = $result;

}
foreach($rows as $row){
    $course_ID = $row["Course_ID"];
}

// add admin account to admin table
// we do this after inserting course to get Course_ID
insertAccountIntoAdmin($course_ID,$account_ID,$con);

//insert level into level table
insertLevelIntoLevel($level_number, $course_ID, $con);

if($type == "Assessment")
{
    insertIntoAssessHead($assessment_name, $course_ID,$display_answers, $attempts_value, $con);
    // get assesshead ID to save time and resources
    $query= "SELECT * FROM RTX.AssessHead WHERE AssessHead_Title = '".$assessment_name."' AND AssessHead_CourseID = '".$course_ID."';";
    $excecute = mysqli_query($con, $query) or die("2:...DB issue");
    $rows = array();
    while($result =mysqli_fetch_assoc($excecute))
    {
        $rows[] = $result;

    }
    foreach($rows as $row){
        $assesshead_ID = $row["AssessHead_ID"];
    }


    insertIntoAssessment($question_type, $question_name, $display_answers, $assesshead_ID, $points, $con);
    $query= "SELECT * FROM RTX.Assessment WHERE Assessment_Question = '".$question_name."' AND Assessment_AssessHeadID = '".$assesshead_ID."';";
    $excecute = mysqli_query($con, $query) or die("2:...DB issue");
    $rows = array();
    while($result =mysqli_fetch_assoc($excecute))
    {
        $rows[] = $result;

    }
    foreach($rows as $row){
        $assessment_ID = $row["Assessment_ID"];
    }


    insertIntoAssessmentAnswer($question_type, $correct_answer, $answer_name, $assesshead_ID, $assessment_ID, $coins, $right_twin, $left_twin, $con);
}
//get the level ID to save time and resources
$level_name = "Level ".$level_number;
$query = "SELECT * FROM RTX.Level WHERE Level_Name ='".$level_name."' AND Level_CourseID = '".$course_ID."';";
$excecute = mysqli_query($con, $query) or die("2:...DB issue");
$rows = array();
while($result =mysqli_fetch_assoc($excecute))
{
    $rows[] = $result;

}
foreach($rows as $row){
    $level_ID = $row["Level_ID"];
}
insertIntoLevelContent($type,$level_ID, $assessment_name, $assesshead_ID, $con);
insertIntoLevelTree($con, $course_ID);


// $nameCheckString = "SELECT * FROM RTX.Account WHERE Account_Username='".$username."';";


// $nameCheck = mysqli_query($con, $nameCheckString) or die("2: Name check query failed...DB issue"); // error code 2 = name already in Db

// if(mysqli_num_rows($nameCheck) > 0)
// {
//     echo ("3: Name already exists");
//     exit();
// }





//-----------------Helper Functions--------------------//
function checkAsessmentinDB($question_number)
{

    return true;
}
function getAssessHeadID($question_title)
{
    //get course ID from name

    //check if assesshead_id, courseID, and enddate are the same.
}

function insertCourseIntoDb($course_name, $course_tag, $join_key, $con)
{
    $query = "SELECT * FROM RTX.Course WHERE Course_Name = '".$course_name."' AND Course_Tag = '".$course_tag."' AND Course_Key = '".$join_key."';";
    //$query = "SELECT * FROM RTX.Course WHERE Course_Tag = CS386c;";
    $excecute = mysqli_query($con, $query);
    try {
        // perform some task
        if(mysqli_num_rows($excecute) == 0)
        {
            $insert_course_query = "INSERT INTO RTX.Course (Course_Name, Course_Tag, Course_MarketFlag, Course_Key) 
            VALUES ('".$course_name."','".$course_tag."', 1, '".$join_key."');";
            $excecute = mysqli_query($con, $insert_course_query) or die("2:...DB issue insertCourseIntoDb");

        }
    } catch (Exception $ex) {
        // jump to this part
        // if an exception occurred

    }
    //if course not created yet


}
function insertAccountIntoAdmin($course_ID,$account_ID,$con)
{
    //check if admin already exists in table for this course
    $query = "SELECT * FROM RTX.Admin WHERE Admin_AccountID = '".$account_ID."' AND Admin_CourseID = '".$course_ID."';";
    $excecute = mysqli_query($con, $query) or die("2:...DB issue");
    if(mysqli_num_rows($excecute) <=0)
    {
        $insert_admin_query = "INSERT INTO RTX.Admin (Admin_AccountID, Admin_CourseID) VALUES ('".$account_ID."', '".$course_ID."');";
        $excecute = mysqli_query($con, $insert_admin_query) or die("2:...DB issue  insertAccountIntoAdmin");
    }
}

function insertLevelIntoLevel($level_number, $course_ID, $con)
{
    $level_name = "Level ".$level_number;
    $query = "SELECT * FROM RTX.Level WHERE Level_Name ='".$level_name."' AND Level_CourseID = '".$course_ID."';";
    $excecute = mysqli_query($con, $query) or die("2:...DB issue");
    if(mysqli_num_rows($excecute) <=0)
    {
        $insert_level_query = "INSERT INTO RTX.Level (Level_Name, Level_CourseID) VALUES ('".$level_name."', '".$course_ID."');";
        $excecute = mysqli_query($con, $insert_level_query) or die("2:...DB issue");
    }

}

function insertIntoAssessHead($assessment_name, $course_ID,$display_answers, $attempts_value, $con)
{
    $query= "SELECT * FROM RTX.AssessHead WHERE AssessHead_Title = '".$assessment_name."' AND AssessHead_CourseID = '".$course_ID."';";
    $excecute = mysqli_query($con, $query) or die("2:...DB issue");
    if(mysqli_num_rows($excecute) <=0)
    {
        $insert_assesshead_query = "INSERT INTO RTX.AssessHead (AssessHead_Title, AssessHead_DisplayAnswers, AssessHead_CourseID, AssessHead_MaxAttempts)
        VALUES('".$assessment_name."', '".$display_answers."', '".$course_ID."', '".$attempts_value."');";
        $excecute = mysqli_query($con, $insert_assesshead_query) or die("2:...DB issue");
    }
}

function insertIntoAssessment($question_type, $question_name, $display_answers, $assesshead_ID, $points, $con)
{
    $query = "SELECT * FROM RTX.Assessment WHERE Assessment_Question = '".$question_name."';";
    $excecute = mysqli_query($con, $query) or die("2:...DB issue");
    if(mysqli_num_rows($excecute) <=0)
    {
        $query = "INSERT INTO RTX.Assessment (Assessment_QuestionType,Assessment_Question, Assessment_DisplayAnswers, Assessment_AssessHeadID, Assessment_PointsPerQuestion)
        VALUES ('".$question_type."', '".$question_name."', '".$display_answers."', '".$assesshead_ID."', '".$points."');";

    
    $excecute = mysqli_query($con, $query) or die("2:...DB issue"); // error code 2 = name already in Db
    }

}

function insertIntoAssessmentAnswer($question_type, $correct_answer, $answer_name, $assesshead_ID, $assessment_ID, $coins, $right_twin, $left_twin, $con)
{
    if($question_type == "Multiple Choice")
    {
        if($correct_answer == "true")
        {
            $correct_answer = $answer_name;
            $query = "INSERT INTO RTX.AssessmentAnswer (AssessmentAnswer_CorrectAnswers, AssessmentAnswer_AssessHeadID, AssessmentAnswer_AssessmentID, AssessmentAnswer_MultipleChoiceOption, AssessmentAnswer_Coins)
                VALUES('".$answer_name."', '".$assesshead_ID."', '".$assessment_ID."', '".$answer_name."', '".$coins."');";
        }
        else
        {
            $query = "INSERT INTO RTX.AssessmentAnswer (AssessmentAnswer_AssessHeadID, AssessmentAnswer_AssessmentID, AssessmentAnswer_MultipleChoiceOption, AssessmentAnswer_Coins)
            VALUES('".$assesshead_ID."', '".$assessment_ID."', '".$answer_name."', '".$coins."');";
        }
        $excecute = mysqli_query($con, $query) or die("2:...DB issue"); // error code 2 = name already in Db

    }
    else if($question_type == "Matching")
    {
        $query = "INSERT INTO RTX.AssessmentAnswer (AssessmentAnswer_MatchingDropDownOption, AssessmentAnswer_MatchingOption, AssessmentAnswer_AssessHeadID, AssessmentAnswer_AssessmentID, AssessmentAnswer_Coins)
                VALUES('".$left_twin."', '".$right_twin."', '".$assesshead_ID."', '".$assessment_ID."', '".$coins."');";
        $excecute = mysqli_query($con, $query) or die("2:...DB issue"); // error code 2 = name already in Db
    }
    else if($question_type == "Fill_in_Blank")
    {
        $query = "INSERT INTO RTX.AssessmentAnswer (AssessmentAnswer_CorrectAnswers, AssessmentAnswer_Coins, AssessmentAnswer_AssessHeadID, AssessmentAnswer_AssessmentID)
                VALUES('".$correct_answer."', '".$coins."', '".$assesshead_ID."', '".$assessment_ID."');";
        $excecute = mysqli_query($con, $query) or die("2:...DB issue"); // error code 2 = name already in Db
    }

}

function insertIntoLevelContent($type,$level_ID, $assessment_name, $assesshead_ID, $con)
{
    if($type == "Assessment")
    {
        $query = "SELECT * FROM RTX.LevelContent WHERE LvlContent_LevelID = '".$level_ID."' AND LvlContent_Name = '".$assessment_name."';";
        $excecute = mysqli_query($con, $query) or die("2:...DB issue"); // error code 2 = name already in Db
        if(mysqli_num_rows($excecute) <=0)
        {
            $query = "INSERT INTO RTX.LevelContent (LvlContent_LevelID, LvlContent_ObjectID, LvlContent_ModuleID, LvlContent_Name) 
                VALUES ('".$level_ID."', '".$assesshead_ID."', 2, '".$assessment_name."');";
            $excecute = mysqli_query($con, $query) or die("2:...DB issue"); // error code 2 = name already in Db
        }

    }

}
function insertIntoLevelTree($con, $course_ID)
{
    $index = 0;
    $query = "SELECT * FROM RTX.Level WHERE Level_CourseID = '".$course_ID."';";
    $excecute = mysqli_query($con, $query) or die("2:...DB issue"); // error code 2 = name already in Db
    while($result =mysqli_fetch_assoc($excecute))
    {
        $rows[] = $result;

    }

    foreach($rows as $row){
        if($row["Level_ID"] > $index)
        {
            $index = $row["Level_ID"];
        }
    }
    $nextLevelID = (int)$index;
    $nextLevelID = $nextLevelID +1;
    $nextLevelID = (string) $nextLevelID;
    echo("INDEX is ".$row["Level_ID"]);
    $query = "SELECT * FROM RTX.LevelTree WHERE LevelTree_CurrentLevelID = '".$index."';";
    $excecute = mysqli_query($con, $query) or die("2:...DB issue"); // error code 2 = name already in Db
    if(mysqli_num_rows($excecute) <=0)
    {
        $query = "INSERT INTO RTX.LevelTree (LevelTree_CurrentLevelID, LevelTree_NextLevelID) VALUES ('".$index."', '".$nextLevelID."');";
        $excecute = mysqli_query($con, $query); // error code 2 = name already in Db

    }
}
?>