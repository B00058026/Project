<?php
	
	$user = $_POST['user'];
	$score = $_POST['score'];

	$con1 = mysql_connect("localhost","gamesdev_user","Pass","gamesdev_inventory") or ("Cannot connect" . mysql_error());
	
	if(!$con1)
	die('Could not connect: ' . mysql_error());
	mysql_select_db("gamesdev_inventory", $con1) or die("Could not load the database" . mysql_error());
	
	$check1 = mysql_query("SELECT * FROM highscores WHERE `user` = '".$user."'");
	
	$numrows1 = mysql_num_rows($check1);
	
	if($numrows1 == 0)
	{
		$ins1 = mysql_query("INSERT INTO  `gamesdev_inventory`.`highscores` (`id`,`user` ,`Score`)VALUES (NULL ,  '".$user."',  '".$score."');");
		
		if($ins1)
			die("Success");
		
		else
			die("Error: " . mysql_error);
	}
?>