<?php
function open_database_connection() {
    // DB connection details
    $username = 'gamesdev_user';
    $password = 'Pass';
    $host = 'localhost';
    $db = 'gamesdev_inventory';

    // try to connect to DB
    $connection = mysqli_connect($host, $username, $password, $db);

    // check connection successful
    // redirect with message to error page
    if (mysqli_connect_errno()) {
        $errorMessage = 'DB connection failed: '.mysqli_connect_error();
        die($errorMessage);
    }

    return $connection;
}

function close_database_connection($connection)
{
    mysqli_close($connection);
}

function getScoreByPlayerName($playerName){
    $connection=open_database_connection();
    $query = "select * from highscores WHERE Name='$playerName'";
    if($result = mysqli_query($connection,$query)){
        if($row = mysqli_fetch_assoc($result)){
            $Score = $row['Score'];
        }
    } else {
        $errorMessage = 'Query failed:'.$query;
        die($errorMessage);
    }
    close_database_connection($connection);
    return $Score;
}

function getIdByPlayerName($playerName){
    $id = null;

    $connection=open_database_connection();
    $query = "select * from highscores WHERE Name='$playerName'";
    if($result = mysqli_query($connection,$query)){
        if($row = mysqli_fetch_assoc($result)){
            $id = $row['id'];
        }
    } else {
        $errorMessage = 'Query failed:'.$query;
        die($errorMessage);
    }
    return $id;
}

function setScoreByPlayerName($playerName, $newScore){
    $connection = open_database_connection();

    $oldScore = getScoreByPlayerName($playerName);
    $playerId = getIdByPlayerName($playerName);

    $shouldUpdate = $newScore > $oldScore;

    if($shouldUpdate){
        //---------
        // update Score //
        $query = "UPDATE highscores SET Score=$newScore WHERE id=$playerId";

        mysqli_query($connection, $query);
        $numAffectedRows = mysqli_affected_rows($connection);
        close_database_connection($connection);
        return ($numAffectedRows > 0);
    } else {
        close_database_connection($connection);
        return false;
    }
}

function deleteAllRecords()
{
    $connection = open_database_connection();

    $query = "DELETE FROM highscores";
    mysqli_query($connection, $query);
    close_database_connection($connection);
}


function insertNewRecord($playerName, $Score)
{
    $connection = open_database_connection();

    $query = "INSERT INTO highscores(Name, Score) VALUES ('$playerName', $Score)";
    print '<br>';
    print($query);
    mysqli_query($connection, $query);
    close_database_connection($connection);
}