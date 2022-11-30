<?php
////This PHP gives you all the information you need for one course for the admin to view
    $con = mysqli_connect('gaminged-db.czq2j2udebs7.us-west-2.rds.amazonaws.com', 'superAdmin', 'ghahyat8', 'RTX');

    
    if(mysqli_connect_error())
    {
        echo ("1: Connection failed"); // error code #1 = connection failed
        exit();
    }

    $classNum = $_POST["ClassNumber"];

    $queryString = "SELECT `RTX`.`Account`.`Account_Username` AS `Account_Username`,
    `RTX`.`Player`.`Player_IGN` AS `Player_IGN`,
    `RTX`.`Player`.`Player_Coins` AS `Player_Coins`,
    `RTX`.`Player`.`Player_Letter_Grade` AS `Letter_Grade` 
    FROM ((`RTX`.`Account` 
        JOIN `RTX`.`Player` ON ((`RTX`.`Account`.`Account_ID` = `RTX`.`Player`.`Player_AccountID`))) 
        JOIN `RTX`.`Course` ON ((`RTX`.`Player`.`Player_CourseID` = `RTX`.`Course`.`Course_ID`))) 
    WHERE ((`RTX`.`Course`.`Course_Tag` = '".$classNum."' ) AND (`RTX`.`Account`.`Account_Role` = 'student'));";

    $students = mysqli_query($con, $queryString) or die("2:...DB issue"); // error code 2 = name already in Db
    $rows = array();

    while($result =mysqli_fetch_assoc($students))
    {
        $rows[] = $result;

    }

    foreach($rows as $row){
        echo ($row["Account_Username"]."%/%/%/%/%/%/%/%/%/%/*&^".$row["Player_IGN"]."%/%/%/%/%/%/%/%/%/%/*&^".$row["Player_Coins"]."%/%/%/%/%/%/%/%/%/%/*&^".$row["Letter_Grade"]."////////////////////%%%%%"); 
    }

?>
