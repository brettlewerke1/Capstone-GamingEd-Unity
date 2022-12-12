<?php
    $con = mysqli_connect('gaminged-db.czq2j2udebs7.us-west-2.rds.amazonaws.com', 'superAdmin', 'ghahyat8', 'RTX');

    if(mysqli_connect_error())
    {
        echo ("1: Connection failed"); // error code #1 = connection failed
        exit();
    }



    $role = "admin";



    $username = $_POST["Username"];
    $queryString = "SELECT *
    FROM ((`RTX`.`Account` 
        JOIN `RTX`.`Admin` ON ((`RTX`.`Account`.`Account_ID` = `RTX`.`Admin`.`Admin_AccountID`))) 
        JOIN `RTX`.`Course` ON ((`RTX`.`Admin`.`Admin_CourseID` = `RTX`.`Course`.`Course_ID`))) 
    WHERE ((`RTX`.`Account`.`Account_Username` = '".$username."' ) AND (`RTX`.`Account`.`Account_Role` = 'admin'));";
    // SQL query for specific teacher
    //$queryString = "SELECT *  FROM RTX.Account WHERE Account_Username='".$username."' AND Account_Role = '".$role."';";

    $teachers = mysqli_query($con, $queryString) or die("2:...DB issue"); // error code 2 = name already in Db
    if(mysqli_num_rows($teachers) <= 0)
    {
        $queryString = "SELECT *  FROM `RTX`.`Account` WHERE `RTX`.`Account`.`Account_Username`='".$username."' AND `RTX`.`Account`.`Account_Role` = 'admin';";
        $teachers = mysqli_query($con, $queryString) or die("2:...DB issue"); // error code 2 = name already in Db
        $response = mysqli_fetch_assoc($teachers);
        echo("No Courses Found%/%/%/%/%/%/%/%/%/%/*&^".$response["Account_Username"]."%/%/%/%/%/%/%/%/%/%/*&^".$response["Account_Password"]."%/%/%/%/%/%/%/%/%/%/*&^");
    }
    else
    {
        $rows = array();

        while($result =mysqli_fetch_assoc($teachers))
        {
            $rows[] = $result;
    
        }
    
        foreach($rows as $row){
            
            
            echo ($row["Account_Username"]."%/%/%/%/%/%/%/%/%/%/*&^".$row["Account_Password"]."%/%/%/%/%/%/%/%/%/%/*&^".$row["Course_Name"]."%/%/%/%/%/%/%/%/%/%/*&^".$row["Course_Tag"]."////////////////////%%%%%"); 
    
            
        }
    }


?>