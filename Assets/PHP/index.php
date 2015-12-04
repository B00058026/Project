<?php
include_once 'scoreFunctions.php';
include_once 'dbFunctions.php';

if(!isset($_GET["action"])){
    $action = 'defaultIndexPage';
} else {
    $action = $_GET["action"];
}

switch ($action) {
    case 'html':
        toHTML();
        break;
    case 'defaultIndexPage':
        include_once 'htmlDefault.php';
        print $htmlDefaultPage;
        break;

    default:
        print 'Error - unknown value for "action" parameter';
}

print '<br>';
print '<a href="index.php">index</a>';
?>