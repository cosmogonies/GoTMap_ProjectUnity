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
		
	void Start () 
	{

	}

	void OnGUI () 
	{
		TITLE_HEIGHT=Screen.height*0.1f;	// 10% of height goes to TimeSlider.

		float DATE_WIDTH = Screen.width*0.1f;
		float JUMP_WIDTH = Screen.width*0.1f*0.5f; //both jump buttons will occupy 10% of screen width

		float TIME_SLIDER_WIDTH = Screen.width-DATE_WIDTH-2*JUMP_WIDTH;

		GUI.skin = 	theGUISkin;
		GUI.skin.label.fontSize = 20;
		//string SelectedTomeName = "T"+this.GetComponent<BHV_Storyline>().SelectedTome.Order +": "+ this.GetComponent<BHV_Storyline>().SelectedTome.Name;





		//GUI.Label(new Rect(0.0f,Screen.height-TITLE_HEIGHT,Screen.width-DATE_WIDTH,TITLE_HEIGHT) , "    "+SelectedTomeName);

		ScrollValue = GUI.HorizontalSlider(new Rect(JUMP_WIDTH,Screen.height-TITLE_HEIGHT,TIME_SLIDER_WIDTH,TITLE_HEIGHT) , ScrollValue, 0.0f,1.0f);
		//ScrollValue = GUI.HorizontalSlider(new Rect(0.0f,20.0f,Screen.width-100.0f,50.0f) , ScrollValue, 0.0f,1.0f,SliderSkin,SliderThumb);



		if( GUI.Button(new Rect(0.0f,Screen.height-TITLE_HEIGHT,JUMP_WIDTH,TITLE_HEIGHT),"<[-]") )
		{


		}

		if( GUI.Button(new Rect(JUMP_WIDTH+TIME_SLIDER_WIDTH,Screen.height-TITLE_HEIGHT,JUMP_WIDTH,TITLE_HEIGHT),"[+]>") )
		{
			
			
		}



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
