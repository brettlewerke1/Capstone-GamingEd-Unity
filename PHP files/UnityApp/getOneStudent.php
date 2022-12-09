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
    `RTX`.`Account`.`Account_Username` AS `Account_Username`,
    `RTX`.`Player`.`Player_IGN` AS `Player_IGN`,
    `RTX`.`Account`.`Account_Password` AS `Account_Password`,
    `RTX`.`Player`.`Player_Coins` AS `Player_Coins`,
    `RTX`.`Player`.`Player_Percentage_Grade` AS `Player_Percentage_Grade`
    FROM
    (`RTX`.`Account`
    JOIN `RTX`.`Player` ON ((`RTX`.`Account`.`Account_ID` = `RTX`.`Player`.`Player_AccountID`)))
    WHERE
    (`RTX`.`Account`.`Account_Username` = '".$studentUsername."')";

    $student = mysqli_query($con, $queryString) or die("2:...DB issue"); // error code 2 = name already in Db
    $rows = array();

    while($result =mysqli_fetch_assoc($student))
    {
        $rows[] = $result;

    }

    foreach($rows as $row){
        echo ($row["Account_Username"]."%/%/%/%/%/%/%/%/%/%/*&^".$row["Player_IGN"]."%/%/%/%/%/%/%/%/%/%/*&^".$row["Account_Password"]."%/%/%/%/%/%/%/%/%/%/*&^".$row["Player_Coins"]."%/%/%/%/%/%/%/%/%/%/*&^".$row["Player_Percentage_Grade"]."////////////////////%%%%%"); 
    }

?>