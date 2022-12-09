<?php
 $con = mysqli_connect('gaminged-db.czq2j2udebs7.us-west-2.rds.amazonaws.com', 'superAdmin', 'ghahyat8', 'RTX');

 //check for connection success
 if(mysqli_connect_error())
 {
     echo ("1: Connection failed"); // error code #1 = connection failed
     exit();
 }

 $newIGN = $_POST["StudentIGN"];

 $oldIGN = $_POST["oldIGN"];

 $oldIGN_query = "SELECT `RTX`.`Player`.`Player_ID` FROM `RTX`.`Player` WHERE `RTX`.`Player`.`Player_IGN` = '".$oldIGN."';";

 $player_id = mysqli_query($con, $oldIGN_query) or die("2: Name check query failed...DB issue"); // error code 2 = name already in Db

 $userInfo = mysqli_fetch_assoc($player_id);

 $userId = $userInfo["Player_ID"];

 $query =  "UPDATE `RTX`.`Player` SET  `Player_IGN` = '".$newIGN."' WHERE `Player_ID` = '".$userId."';";

 $result = mysqli_query($con, $query) or die("2: Name check query failed...DB issue"); // error code 2 = name already in Db

 $new_player_IGN = mysqli_fetch_assoc($result);
 echo("Update Successful: ".$new_player_IGN);

?>