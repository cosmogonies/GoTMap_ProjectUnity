using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_Main : MonoBehaviour 
{
	public GUIStyle SliderSkin;
	//public GUIStyle SliderThumb;
	public GUISkin theGUISkin;
	
	public float ScrollValue=0.0f;
		
	public GameObject Pawn;

	public float TITLE_HEIGHT;

	private BHV_Storyline StoryLineComp;
	private BHV_CameraMotion CameraMotionComp;

	bool isDisplayingInfo = false;
	bool isDisplayingTimeLine = false;
		
	void Start () 
	{
		StoryLineComp = this.GetComponent<BHV_Storyline>();
		CameraMotionComp = this.GetComponent<BHV_CameraMotion>();
	}

	void OnGUI () 
	{

		#region GUI_HEADER
		if(GUI.Button(new Rect(0f,0f,Screen.width*0.1f,Screen.height*0.05f),"<back"))
		{
			GameObject Kernel = GameObject.FindGameObjectWithTag("Core");
			DestroyImmediate(Kernel);
			Application.LoadLevel("MainMenu");
		}
		if(GUI.Button(new Rect(Screen.width*0.8f,0f,Screen.width*0.2f,Screen.height*0.05f),"TimeLine"))
		{
			isDisplayingTimeLine = ! isDisplayingTimeLine;
			this.gameObject.GetComponent<GUI_TimeLine>().isDisplayingTimeLine = !this.gameObject.GetComponent<GUI_TimeLine>().isDisplayingTimeLine;
			if(isDisplayingTimeLine)
				this.gameObject.GetComponent<GUI_TimeLine>().isCameraGoingBack = true;
			//this.gameObject.GetComponent<GUI_TimeLine>().enabled =isDisplayingTimeLine;
		}
		#endregion



		TITLE_HEIGHT=Screen.height*0.1f;	// 10% of height goes to TimeSlider.
		float INFO_HEIGHT=Screen.height*0.05f;	// 5% of height goes to Info display.

		//float DATE_WIDTH = Screen.width*0.1f;
		float DATE_WIDTH = Screen.width*0.3f;
		float INFO_WIDTH = Screen.width*0.4f;
		float FOLLOW_WIDTH = Screen.width*0.3f;

		//float JUMP_WIDTH = Screen.width*0.1f*0.5f; //both jump buttons will occupy 10% of screen width
		float JUMP_WIDTH = Screen.width*0.1f; //both jump buttons will occupy 10% of screen width

		//float TIME_SLIDER_WIDTH = Screen.width-DATE_WIDTH-2*JUMP_WIDTH;
		float TIME_SLIDER_WIDTH = Screen.width-2*JUMP_WIDTH;

		GUI.skin = 	theGUISkin;
		GUI.skin.label.fontSize = 18;
		//string SelectedTomeName = "T"+this.GetComponent<BHV_Storyline>().SelectedTome.Order +": "+ this.GetComponent<BHV_Storyline>().SelectedTome.Name;


		//GUI.Label(new Rect(0.0f,Screen.height-TITLE_HEIGHT,Screen.width-DATE_WIDTH,TITLE_HEIGHT) , "    "+SelectedTomeName);
		//ScrollValue = GUI.HorizontalSlider(new Rect(0.0f,20.0f,Screen.width-100.0f,50.0f) , ScrollValue, 0.0f,1.0f,SliderSkin,SliderThumb);
		//ScrollValue = GUI.HorizontalSlider(new Rect(JUMP_WIDTH,Screen.height-TITLE_HEIGHT,TIME_SLIDER_WIDTH,TITLE_HEIGHT) , ScrollValue, 0.0f,1.0f);
		ScrollValue = GUI.HorizontalSlider(new Rect(JUMP_WIDTH,Screen.height-TITLE_HEIGHT,TIME_SLIDER_WIDTH,TITLE_HEIGHT) , ScrollValue, 0.0f,1.0f);


		if( GUI.Button(new Rect(0.0f,Screen.height-TITLE_HEIGHT,JUMP_WIDTH,TITLE_HEIGHT),"<[-]") )
		{
			//ScrollValue = StoryLineComp.getPreviousEvent();
			Evvent previous = StoryLineComp.getPreviousEvent();
			ScrollValue = StoryLineComp.convertDateToRatio( previous.Date );
		}

		if( GUI.Button(new Rect(JUMP_WIDTH+TIME_SLIDER_WIDTH,Screen.height-TITLE_HEIGHT,JUMP_WIDTH,TITLE_HEIGHT),"[+]>") )
		{
			Evvent next = StoryLineComp.getNextEvent();
			ScrollValue = StoryLineComp.convertDateToRatio( next.Date );
		}



		//We refresh all GUIButtons of each characters :
		foreach(KeyValuePair<string,GameObject> kvp in StoryLineComp.CharacterDict)
		{
			StoryLineComp.refreshCharacterButton(kvp.Value);
		}
		
		//Background of Timeslider:
		//string SelectedTomeName = "T"+this.GetComponent<BHV_Storyline>().SelectedTome.Order +": "+ this.GetComponent<BHV_Storyline>().SelectedTome.Name;
		//GUI.Label(new Rect(Screen.width*0.1f*0.5f,Screen.height*0.9f,Screen.width*0.9f,Screen.height*0.1f) , "    "+StoryLineComp.SelectedTome.Name);
		GUI.Label(new Rect(Screen.width*0.1f*0.5f,Screen.height*0.9f,Screen.width*0.9f,Screen.height*0.1f) , this.StoryLineComp.getCurrentEventName());


		//GUI.skin.button.fontSize = Mathf.RoundToInt(0.5f*GUI.skin.button.fontSize);
		if ( GUI.Button( new Rect(0f,Screen.height-(INFO_HEIGHT+TITLE_HEIGHT),DATE_WIDTH,INFO_HEIGHT), StoryLineComp.currentDate.ToString("dd/MM/yyy")))	//TO Do
		{
			//Call the Zoom Window, this Event Description
			isDisplayingInfo = ! isDisplayingInfo;
		}
		GUI.skin.button.fontSize =22;
		if(isDisplayingInfo)
		{
			if ( GUI.Button( new Rect(Screen.width*0.1f,Screen.height*0.3f,Screen.width*0.8f,Screen.height*0.1f),  this.StoryLineComp.getCurrentEvent().Name  ))	
				isDisplayingInfo = false;
			if ( GUI.Button( new Rect(Screen.width*0.1f,Screen.height*0.4f,Screen.width*0.8f,Screen.height*0.4f),  this.StoryLineComp.getCurrentEvent().Info.Replace('.','\n')  ))	
				isDisplayingInfo = false;
		}


		#region FOLLOWING_GUI_LOGIC
		string DisplayName="";
		if(CameraMotionComp.Following != null)
			DisplayName = CameraMotionComp.Following.name;
		else
			DisplayName = "-";

		if ( GUI.Button( new Rect(DATE_WIDTH+INFO_WIDTH,Screen.height-(INFO_HEIGHT+TITLE_HEIGHT),FOLLOW_WIDTH,INFO_HEIGHT), DisplayName ) )
		{
			//if(CameraMotionComp.Following != null){}
			//else
			{
				//let's follow the next one
				CameraMotionComp.Following = getNextFollowableCharacter();

			}
		}
		#endregion



		/*	Test to callback/event manage of GUI system.
		if(ScrollValue != PreviousScrollValue )
			this.isScrubbing=true;
		else
			this.isScrubbing=false;
		*/
		
		
		/*	Test to drop-down menu (comboBox)
		List<string> myList =  new List<string>();
		myList.Add("test1");
		myList.Add("test2");
		this.currentSeletion = SelectList( myList, this.currentSeletion, OnCheckboxItemGUI );
		 */

		
	
		
		/*
		if ( GUI.Button( new Rect(Screen.width-100,0.0f,100.0f,25.0f), "Menu") )
		{
			Application.Quit();
		}
		*/
	}
	




	GameObject getNextFollowableCharacter()
	{
		bool found=false;

		foreach(GameObject currentCharGO in this.StoryLineComp.CharacterDict.Values )
		{
			if(found)
				if (!StoryLineComp.TheDatabase.amIDead(StoryLineComp.currentDate, currentCharGO.name))
					return currentCharGO;

			if( CameraMotionComp.Following == currentCharGO ) 
				found=true;

		}

		foreach(GameObject currentCharGO in this.StoryLineComp.CharacterDict.Values )
			if (!StoryLineComp.TheDatabase.amIDead(StoryLineComp.currentDate, currentCharGO.name))
				return currentCharGO;

		return null;
	}









	/*
	
	
	private bool OnCheckboxItemGUI( object item, bool selected, ICollection list )
	{
		return GUILayout.Toggle( selected, item.ToString() );
	}
	
	
	
	
	
	
	
	//FROM: http://wiki.unity3d.com/index.php?title=SelectList
	public static object SelectList( ICollection list, object selected, GUIStyle defaultStyle, GUIStyle selectedStyle )
	{			
		foreach( object item in list )
		{
			if( GUILayout.Button( item.ToString(), ( selected == item ) ? selectedStyle : defaultStyle ) )
			{
				if( selected == item )
				// Clicked an already selected item. Deselect.
				{
					selected = null;
				}
				else
				{
					selected = item;
				}
			}
		}

		return selected;
	}
	public delegate bool OnListItemGUI( object item, bool selected, ICollection list );
	public static object SelectList( ICollection list, object selected, OnListItemGUI itemHandler )
	{
		ArrayList itemList;

		itemList = new ArrayList( list );

		foreach( object item in itemList )
		{
			if( itemHandler( item, item == selected, list ) )
			{
				selected = item;
			}
			else if( selected == item )
			// If we *were* selected, but aren't any more then deselect
			{
				selected = null;
			}
		}

		return selected;
	}

	
	*/
	
	
	
	
	
	
	
	
	
}
