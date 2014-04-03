using UnityEngine;
using System.Collections;
using System.IO;

public class GUI_MainMenu : MonoBehaviour
{
	public GUISkin TheGUISkin;

	private bool isSelectingTome=false;

	public string SelectedTome="ASOIAF";

	public Vector2 cursor;

	void Start ()
	{
		DontDestroyOnLoad(this.gameObject); 	// Keep the Core

		/*
		if (PlayerPrefs.HasKey ("haveReadTome1")) {
			haveReadTome1 = PlayerPrefs.GetInt ("haveReadTome1");
		}
		if (PlayerPrefs.HasKey ("haveReadTome2")) {
			haveReadTome2 = PlayerPrefs.GetInt ("haveReadTome2");
		}
		if (PlayerPrefs.HasKey ("haveReadTome3")) {
			haveReadTome3 = PlayerPrefs.GetInt ("haveReadTome3");
		}
		if (PlayerPrefs.HasKey ("haveReadTome4")) {
			haveReadTome4 = PlayerPrefs.GetInt ("haveReadTome4");
		}
		if (PlayerPrefs.HasKey ("haveReadTome5")) {
			haveReadTome5 = PlayerPrefs.GetInt ("haveReadTome5");
		}
		*/

		/*
		if (PlayerPrefs.HasKey ("ChoosenTome"))
		{
			PlayerPrefs.SetInt("ChoosenTome", 0);
		}
		else
		{
			PlayerPrefs.("ChoosenTome", 0);
		}
		*/

	}
	
	void Update ()
	{
	
	}
	
	void OnGUI ()
	{
		GUI.skin = TheGUISkin;
		
		//Vector2 Margin = new Vector2 (Screen.width * 0.1f, Screen.height * 0.1f);
		Vector2 Margin = new Vector2 (Screen.width * 0.0f, Screen.height * 0.0f);
		float TITLE_HEIGHT = Screen.height * 0.2f;	// 1 +2 +2 first half of Height
		float HeightPosition = 0;

		if(! isSelectingTome)
		{
			GUI.skin.label.fontSize = Mathf.RoundToInt( Screen.height/10f );;
			//GUI.Label (new Rect (Margin.x, Margin.y , Screen.width - Margin.x, TITLE_HEIGHT), " A SONG OF ICE AND FIRE");
			//GUI.Label (new Rect (Margin.x, Margin.y+TITLE_HEIGHT , Screen.width - Margin.x, TITLE_HEIGHT), " a \"Game of Thrones\"\n interactive map");

			GUILayout.BeginArea( new Rect (Margin.x, Margin.y , Screen.width - Margin.x, Screen.height * 0.6f) );

			//GUILayout.Label("A SONG OF ICE AND FIRE\na \"Game of Thrones\" interactive map");
			GUILayout.Label("A SONG OF ICE AND FIRE");
			GUI.skin.label.fontSize = Mathf.RoundToInt(0.7f*GUI.skin.label.fontSize);
			GUILayout.Label("a \"Game of Thrones\" interactive map");

			GUILayout.EndArea();
		
			
			//GUI.skin.label.fontSize = 25;
			GUI.skin.label.fontSize = Mathf.RoundToInt( Screen.height/10f );	//Magic stuff
			
			float CHOICE_HEIGHT = TITLE_HEIGHT*0.5f;
			HeightPosition += CHOICE_HEIGHT;


			HeightPosition = Screen.height*0.6f;
			if (GUI.Button( new Rect (Margin.x, Margin.y + HeightPosition, Screen.width - 2*Margin.x, CHOICE_HEIGHT), "THE WHOLE TIME LINE" ))
			{
				SelectedTome="ASOIAF";
				Application.LoadLevel("Map");
			}

			if (GUI.Button( new Rect (Margin.x, Margin.y + HeightPosition+CHOICE_HEIGHT*2, Screen.width - 2*Margin.x, CHOICE_HEIGHT), "SELECT A TOME" ))
				isSelectingTome = true;
		}
		else
		{
			GUI.skin.label.fontSize = Mathf.RoundToInt( Screen.height/10f );	//Magic stuff

			float CHOICE_HEIGHT = Screen.height / (6*2); //Nb Tome*2
			if (GUI.Button( new Rect (Margin.x, Margin.y + HeightPosition, Screen.width*0.2f, CHOICE_HEIGHT), "<= BACK" ))
			{
				isSelectingTome = false;
			}
			HeightPosition += CHOICE_HEIGHT*2;

			if (GUI.Button( new Rect (Margin.x, Margin.y + HeightPosition, Screen.width*0.8f, CHOICE_HEIGHT), "TOME 1: A GAME OF THRONES" ))
			{
				SelectedTome="AGOT";
				Application.LoadLevel("Map");
			}
			HeightPosition += CHOICE_HEIGHT*2;
			if (GUI.Button( new Rect (Margin.x, Margin.y + HeightPosition, Screen.width*0.8f, CHOICE_HEIGHT), "TOME 2: A CLASH OF KINGS" ))
			{
				SelectedTome="ACOK";
				Application.LoadLevel("Map");
			}
			HeightPosition += CHOICE_HEIGHT*2;
			if (GUI.Button( new Rect (Margin.x, Margin.y + HeightPosition, Screen.width*0.8f, CHOICE_HEIGHT), "TOME 3: A STORM OF SWORDS" ))
			{
				SelectedTome="ASOS";
				Application.LoadLevel("Map");
			}
			HeightPosition += CHOICE_HEIGHT*2;
			if (GUI.Button( new Rect(Margin.x, Margin.y + HeightPosition, Screen.width*0.8f, CHOICE_HEIGHT), "TOME 4: A FEAST FOR CROWS" ))
			{
				SelectedTome="AFFC";
				Application.LoadLevel("Map");
			}
			HeightPosition += CHOICE_HEIGHT*2;
			if (GUI.Button( new Rect (Margin.x, Margin.y + HeightPosition, Screen.width*0.8f, CHOICE_HEIGHT), "TOME 5: A DANCE WITH DRAGONS" ))
			{
				SelectedTome="ADWD";
				Application.LoadLevel("Map");
			}

			GUI.skin.label.fontSize = 20;
			GUI.skin.label.font.material.color = Color.gray;
			GUI.Label( new Rect (Margin.x+Screen.width*0.3f, Margin.y, Screen.width*0.5f, CHOICE_HEIGHT), "Do not pick an unread book to avoid spoilers!" );
			GUI.skin.label.font.material.color = Color.white;
		}
	}
}
