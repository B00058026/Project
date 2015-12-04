// file: WebLeaderBoard.cs
using UnityEngine;
using System.Collections;
using System;

using UnityEngine.UI;

public class WebLeaderBoard : MonoBehaviour {
	public Text ui_lastURL;
	public Text ui_lastURLValue;
	public Text ui_textFile;

	public string leaderBoardURL = "http://gamesdevproject.x10host.com/highscores/index.php";
	private string url = "(empty)";
	private string action;
	private string parameters;
	private string textFileContents = "(still loading file ...)";

	void Start(){
		UpdateUI();
	}

	private void UpdateUI() {
		ui_lastURL.text = "LAST URL = " + url;
		ui_lastURLValue.text = StringToInt(textFileContents);
		ui_textFile.text = MakePretty(textFileContents);
	}

	private string MakePretty(string s){

		// hide closing tag
		string prettyText = s.Replace("</", "?@?"); 
		
		// prefix opening tag with newline
		prettyText = prettyText.Replace("<", "\n<"); 
		
		// return closing tag 
		prettyText = prettyText.Replace("?@?", "</"); 

		return prettyText;

		/*
		GUILayout.Label ( "last url = " + url );
		GUILayout.Label ( StringToInt(textFileContents) );
		GUILayout.Label ( "results from last url = " + prettyText );
		
		WebButtons();
		*/
	}

	private void WebButtons() {
		bool htmlButtonWasClicked = GUILayout.Button("Get html for all players");

		if( htmlButtonWasClicked )
			HTMLAction();
	}
	
	private string StringToInt(string s) {
		string intMessage = "integer received = ";
		try{
			int integerReturned = Int32.Parse(s);
			intMessage += integerReturned;
		}
		catch(System.Exception e){
			intMessage += "(not an integer) ";
//			print (e);
		}	
		return intMessage;
	}

	
	private IEnumerator LoadWWW(){
		url = leaderBoardURL + "?action=" + action + parameters;
		WWW www = new WWW (url);
		yield return www;
		textFileContents = www.text;
		UpdateUI();
	}
	
	//
	// public button Methods
 	//

	public void HTMLAction() {
		action = "html";
		parameters = "";
		StartCoroutine( LoadWWW() );
	}
}