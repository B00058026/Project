<?php
	$user = $_POST['user'];	
	$name = $_POST['name'];
	$pass = $_POST['password'];
	
	$con = mysql_connect("localhost","gamesdev_user","Pass","gamesdev_login") or ("Cannot connect" . mysql_error());
	
	if(!$con)
	die('Could not connect: ' . mysql_error());
	mysql_select_db("gamesdev_login", $con) or die("Could not load the database" . mysql_error());

	$check = mysql_query("SELECT * FROM Login WHERE `user` = '".$user."'");
	$numrows = mysql_num_rows($check);
	
	if($numrows == 0)
	{
		$pass = md5($pass);
		
		$ins = mysql_query("INSERT INTO  `gamesdev_login`.`Login` (`id`,`user`,`name` ,`password`)VALUES (NULL ,  '".$user."',  '".$name."',  '".$pass."');");
	
	if($ins)
		die("Successfully Created User");
	
	else
		die("Error: " . mysql_error());
	}
	else
	{
		die('User already exists');
	}
?>