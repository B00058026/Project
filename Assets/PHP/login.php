<?php
	$user = $_POST['user'];
	$pass = $_POST['password'];
	
	$con = mysql_connect("localhost","gamesdev_user","Pass","gamesdev_login") or ("Cannot connect" . mysql_error());
	
	if(!$con)
	die('Could not connect: ' . mysql_error());
	mysql_select_db("gamesdev_login", $con) or die("Could not load the database" . mysql_error());

	$check = mysql_query("SELECT * FROM Login WHERE `user` = '".$user."'");
$numrows = mysql_num_rows($check);
if ($numrows == 0)
{
	die ("Username does not exist \n");
}
else
{
	$pass = md5($pass);
	while($row = mysql_fetch_assoc($check))
	{
		if ($pass == $row['password'])
		{
			die("login-SUCCESS");
		}
		else
		{
			die("Password does not match \n");
		}
	}
}

?>