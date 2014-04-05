using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GUI_TimeLine : MonoBehaviour
{
	//CLS_Database TheDatabase;
	BHV_Storyline StorylineComponent;

	float MAX_LENGTH = 0f;	//All years in pixels.
	float currentCursor = 0f;

	//System.DateTime currentDate;
	List<DateTime> EventList ;
	
	void OnEnable ()
	//void Start ()
	{
		//this.TheDatabase = CLS_Database.Instance;	//singleton pattern
		StorylineComponent = this.gameObject.GetComponent<BHV_Storyline>();

	}

	void OnGUI()
	{
		//Tome FullTome = StorylineComponent.TheDatabase.TomeDict["ASOIAF"];

		System.TimeSpan duration = StorylineComponent.SelectedTome.End -StorylineComponent.SelectedTome.Start;
		System.TimeSpan delta = StorylineComponent.currentDate -StorylineComponent.SelectedTome.Start;
		currentCursor = -(delta.Days/ (float)duration.Days) * MAX_LENGTH;

		//currentCursor *= 0.001f;

		//Debug.Log (delta.Days/ (float)duration.Days);

		//GUI.Button( new Rect(Screen.width*0.1f,Screen.height*0.1f,Screen.width*0.8f,Screen.height*0.7f),  "HUHU");

		/*
		if(Input.touchCount>0)
		{
			Vector2 FrameMovement = Input.GetTouch(0).deltaPosition;
			currentCursor += FrameMovement.x;
		}
		*/
		drawCalendar();



	}



	void drawCalendar()
	{	//Draw the whole saga outside of the bounds/
		float ATOM_WIDTH = Screen.width * 0.2f; //smallest bucket is the day.
		
		float ATOM_HEIGHT = Screen.height * 0.1f; //std line
		
		int currentMonth = 0;
		int currentYear = 0;

		foreach(Evvent currentEvent in StorylineComponent.StoryLine)    //Be carefull, The list must be ordered chronologically...
		{
			System.DateTime currentEventDate = currentEvent.Date;
			
			#region YEAR_LINE
			//Do we have change year ?
			if( currentEventDate.Year !=  currentYear)
			{
				int cnt = countEventsForYear( currentEventDate.Year );
				GUI.Button( new Rect ( currentCursor,ATOM_HEIGHT, cnt*ATOM_WIDTH, ATOM_HEIGHT) , currentEventDate.Year.ToString() );
				currentYear = currentEventDate.Year;
				MAX_LENGTH += cnt*ATOM_WIDTH;
			}
			#endregion
			
			#region MONTH_LINE
			//Do we have change month ?
			if( currentEventDate.Month !=  currentMonth)
			{
				int cnt = countEventsForMonth( currentEventDate.Month, currentEventDate.Year );
				GUI.Button( new Rect ( currentCursor,ATOM_HEIGHT*2, cnt*ATOM_WIDTH, ATOM_HEIGHT) , currentEventDate.ToString("MMMM") );
				currentMonth = currentEventDate.Month;
			}
			#endregion
			
			GUI.Button( new Rect ( currentCursor,ATOM_HEIGHT*3, ATOM_WIDTH, ATOM_HEIGHT) , currentEventDate.Day.ToString() );
			//Debug.Log(new Rect ( currentCursor,ATOM_HEIGHT*3, ATOM_WIDTH, ATOM_HEIGHT) );
			//Debug.Log( currentEventDate.Day.ToString() );
			currentCursor += ATOM_WIDTH;
		}

	}

	int countEventsForMonth( int _Month, int _Year )
	{
		int result=0;
		foreach(Evvent currentEvent in StorylineComponent.StoryLine)
			if(currentEvent.Date.Year == _Year)
				if(currentEvent.Date.Month == _Month)
					result++;
		return result;
	}
	
	int countEventsForYear(  int _Year )
	{
		int result=0;
		foreach(Evvent currentEvent in StorylineComponent.StoryLine)
			if(currentEvent.Date.Year == _Year)
				result++;
		return result;
	}

}
