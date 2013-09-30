using UnityEngine;
using System.Collections;
using System.IO;

public class GUI_MainMenu : MonoBehaviour
{
	public GUISkin TheGUISkin;
	/*
	private int haveReadTome1 = -1;
	private int haveReadTome2 = -1;
	private int haveReadTome3 = -1;
	private int haveReadTome4 = -1;
	private int haveReadTome5 = -1;
	*/
	private bool haveReadTome1 = false;
	private bool haveReadTome2 = false;
	private bool haveReadTome3 = false;
	private bool haveReadTome4 = false;
	private bool haveReadTome5 = false;
	
	void Start ()
	{
		/*
		if (PlayerPrefs.HasKey ("haveReadTome1")) {
			haveReadTome1 = PlayerPrefs.GetInt ("haveReadTome1");
		}
		if (PlayerPrefs.HasKey ("haveReadTome2")) {
			haveReadTome1 = PlayerPrefs.GetInt ("haveReadTome2");
		}
		if (PlayerPrefs.HasKey ("haveReadTome3")) {
			haveReadTome1 = PlayerPrefs.GetInt ("haveReadTome3");
		}
		if (PlayerPrefs.HasKey ("haveReadTome4")) {
			haveReadTome1 = PlayerPrefs.GetInt ("haveReadTome4");
		}
		if (PlayerPrefs.HasKey ("haveReadTome5")) {
			haveReadTome1 = PlayerPrefs.GetInt ("haveReadTome5");
		}
		*/	
		
	}
	
	void Update ()
	{
	
	}
	
	void OnGUI ()
	{
		GUI.skin = TheGUISkin;
		
		GUI.skin.label.fontSize = 50;
		
		Vector2 Margin = new Vector2 (Screen.width * 0.2f, Screen.height * 0.2f);
		
		float TITLE_HEIGHT = Screen.height * 0.1f;
		
		float HeightPosition = 200.0f;
		
		GUI.Label (new Rect (Margin.x, Margin.y + HeightPosition, Screen.width - Margin.x, TITLE_HEIGHT), "A SONG OF ICE AND FIRE");
		HeightPosition += TITLE_HEIGHT;
		
		GUI.Label (new Rect (Margin.x, Margin.y + HeightPosition, Screen.width - Margin.x, TITLE_HEIGHT), "a \"Game of Thrones\" interactive map");
		HeightPosition += TITLE_HEIGHT;
		
		GUI.skin.label.fontSize = 25;
		
		float CHOICE_HEIGHT = TITLE_HEIGHT*0.5f;
		HeightPosition += CHOICE_HEIGHT;
		
		haveReadTome1 = GUI.Toggle (new Rect (Margin.x, Margin.y + HeightPosition, Screen.width - Margin.x, CHOICE_HEIGHT), haveReadTome1, "I have read Tome 1");
		HeightPosition += CHOICE_HEIGHT;
		haveReadTome2 = GUI.Toggle (new Rect (Margin.x, Margin.y + HeightPosition, Screen.width - Margin.x, CHOICE_HEIGHT), haveReadTome2, "I have read Tome 2");
		HeightPosition += CHOICE_HEIGHT;
		haveReadTome3 = GUI.Toggle (new Rect (Margin.x, Margin.y + HeightPosition, Screen.width - Margin.x, CHOICE_HEIGHT), haveReadTome3, "I have read Tome 3");
		HeightPosition += CHOICE_HEIGHT;
   
		if (GUI.Button( new Rect (Margin.x, Margin.y + HeightPosition, Screen.width - Margin.x, CHOICE_HEIGHT), "TOME 1 : A GAME OF THRONES" ))
			Application.LoadLevel("Map_v30");
		
	}
}
