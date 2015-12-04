<?php
function getPlayer() {
    $playerName = $_GET["Name"];
    $Score = getScoreByPlayerName($playerName);

    print $Score;

}

function setPlayer()
{
    $playerName = $_GET["Name"];
    $newScore = $_GET["Score"];

    $feedback = ACTION_FAILED;

    $success = setScoreByPlayerName($playerName, $newScore);

    if ($success) {
        $feedback = $newScore;
    }

    print $feedback;
}


function toHTML() {
    $connection=open_database_connection();
    $query = "SELECT * FROM highscores order by Score desc";

    $html_output = "<!DOCTYPE HTML><html><head><title>highscores</title></head><body><ol>";
    if($result = mysqli_query($connection,$query)) {
        while ($row = mysqli_fetch_assoc($result)) {
            $player = $row['Name'];
            $Score = $row['Score'];
            $html_output .= "<li>$player = $Score</li>";
        }
    }

    $html_output .= "</ol></body></html>";
    print $html_output;
}

function toXML() {
    $connection=open_database_connection();
    $query = "SELECT * FROM highscores order by Score desc";

    $xml_output = "<?xml><playerScoreList>";
    if($result = mysqli_query($connection,$query)) {
        while ($row = mysqli_fetch_assoc($result)) {
            $player = $row['Name'];
            $Score = $row['Score'];
            $xml_output .= "<Name>";
            $xml_output .= "<playerName>$player</playerName>";
            $xml_output .= "<Score>$Score</Score>";
            $xml_output .= "</Name>";
        }
    }

    $xml_output .= "</playerScoreList>";
    print $xml_output;
}