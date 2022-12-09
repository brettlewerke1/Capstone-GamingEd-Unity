<?php
    $con = mysqli_connect('gaminged-db.czq2j2udebs7.us-west-2.rds.amazonaws.com', 'superAdmin', 'ghahyat8', 'RTX');
    if(mysqli_connect_error())
    {
        echo ("1: Connection failed"); // error code #1 = connection failed
        exit();
    }

    $username = $_POST["Username"];
    $password = $_POST["Password"];
    $role = "admin";

    $queryString = "INSERT INTO `RTX`.`Account`  (`Account_Username`, `Account_Password`, `Account_Role`) VALUES ('".$username."', '".$password."','".$role."';";

    
    $added_teacher = mysqli_query($con, $queryString) or die("2:...DB issue"); // error code 2 = name already in Db
    if($added_teacher == true)
    {
        echo ("Teacher ".$username." sucessfully added");

        exit();
    }
    else
    {
        echo("Teacher not added correctly....failed");

    }

?>