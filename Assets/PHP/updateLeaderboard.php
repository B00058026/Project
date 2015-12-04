<?php
	$user = $_POST['user'];
	$score = $_POST['score'];
	
	$con = mysql_connect("localhost","gamesdev_user","Pass","gamesdev_inventory") or ("Cannot connect" . mysql_error());

	if(!$con)
	die('Could not connect: ' . mysql_error());
	mysql_select_db("gamesdev_inventory", $con) or die("Could not load the database" . mysql_error());
	
	$check = mysql_query("SELECT * FROM highscores");
	
	$numrows = mysql_num_rows($check);
	

	if($numrows == 0)
	{
		$ins = mysql_query("SELECT * FROM  `gamesdev_inventory`.`highscores`");
		
		if($ins)
			die( $row[$ins]);
		
		else
			die("Error: " . mysql_error);
	}


?>