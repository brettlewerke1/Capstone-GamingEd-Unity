<?php
////This PHP gives you all the information you need for one course for the admin to view
    $con = mysqli_connect('gaminged-db.czq2j2udebs7.us-west-2.rds.amazonaws.com', 'superAdmin', 'ghahyat8', 'RTX');

    
    if(mysqli_connect_error())
    {
        echo ("1: Connection failed"); // error code #1 = connection failed
        exit();
    }

    $studentUsername = $_POST["StudentUsername"];

    $queryString = "SELECT 
        `RTX`.`Assignment`.`Assignment_Title` AS `Assignment_Title`,
        `RTX`.`Assignment`.`Assignment_EndDate` AS `Assignment_EndDate`,
        `RTX`.`Assignment`.`Assignment_Desc` AS `Assignment_Desc`
    FROM
        (((`RTX`.`Assignment`
        JOIN `RTX`.`AssignmentSubmissions` ON ((`RTX`.`Assignment`.`Assignment_ID` = `RTX`.`AssignmentSubmissions`.`AssignmentS_AssignmentID`)))
        JOIN `RTX`.`Player` ON ((`RTX`.`AssignmentSubmissions`.`AssignmentS_PlayerID` = `RTX`.`Player`.`Player_ID`)))
        JOIN `RTX`.`Account` ON ((`RTX`.`Account`.`Account_ID` = `RTX`.`Player`.`Player_AccountID`)))
    WHERE
        (`RTX`.`Account`.`Account_Username` = '".$studentUsername."')";

    $student = mysqli_query($con, $queryString) or die("2:...DB issue"); // error code 2 = name already in Db
    $rows = array();

    while($result =mysqli_fetch_assoc($student))
    {
        $rows[] = $result;

    }

    foreach($rows as $row){
        echo ($row["Assignment_Title"]."%/%/%/%/%/%/%/%/%/%/*&^".$row["Assignment_EndDate"]."%/%/%/%/%/%/%/%/%/%/*&^".$row["Assignment_Desc"]."////////////////////%%%%%"); 
    }

?>