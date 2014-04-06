using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GUI_TimeLine : MonoBehaviour
{
	BHV_Storyline StorylineComponent;

	float MAX_LENGTH = 0f;	//All years in pixels.

	float baseCursor = 0f;
	float currentCursorIt = 0f;
	
	List<DateTime> EventList ;

	public bool isDisplayingTimeLine = false;
	public bool isCameraGoingBack = false;
	
	private Transform CameraTransform;
	//Init
	private Vector3 CameraInitPos;
	private Quaternion CameraInitRot;

	public  GameObject TimeLine_CameraPos;
	public  GameObject TimeLine_GoalLookAt;
	public float damping = 1.0f;	//to control the rotation 
	float threshold = 1f;
	Vector3 currentVelocity;

	private Dictionary<Evvent, float> EventToCursorDict ; // Evvent => Cursor position in timeline (pixels)

	//void OnEnable ()
	void Start ()
	{
		//this.TheDatabase = CLS_Database.Instance;	//singleton pattern
		StorylineComponent = this.gameObject.GetComponent<BHV_Storyline>();

		this.EventToCursorDict = new Dictionary<Evvent, float>();

		currentVelocity = Vector3.forward;//CameraTransform.forward;

		CameraTransform = UnityEngine.Camera.main.transform;
		CameraInitPos = CameraTransform.position;
		CameraInitRot = CameraTransform.rotation;
	}

	void Update()
	{
		if( isDisplayingTimeLine )
		{
			this.gameObject.transform.position = Vector3.SmoothDamp(this.gameObject.transform.position, TimeLine_CameraPos.transform.position, ref currentVelocity,1.0f);

			Quaternion RotResult = Quaternion.LookRotation(TimeLine_GoalLookAt.transform.position - this.gameObject.transform.position, Vector3.forward);
			this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, RotResult, Time.deltaTime * damping);
		}
		else
		{
			if(isCameraGoingBack)
			{
				this.gameObject.transform.position = Vector3.SmoothDamp(this.gameObject.transform.position, CameraInitPos, ref currentVelocity,1.0f);
				this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, CameraInitRot, Time.deltaTime * damping);

				if( Vector3.Distance(this.gameObject.transform.position, CameraInitPos)<100f)
					isCameraGoingBack =false;

			}
			else
			{
			}
		}
	}

	void OnGUI()
	{
		if( isDisplayingTimeLine )
		{

			/*
			if(this.EventToCursorDict.Count==0)
				baseCursor = 0;
			else
			{
				//Get nearest Event from current Date
				int smallestDelta = int.MaxValue;
				Evvent TheEvent = null;
				foreach( KeyValuePair<Evvent, float> kvp in this.EventToCursorDict)
				{
					int duration = Mathf.Abs((kvp.Key.Date - this.StorylineComponent.currentDate).Days);
					if( duration < smallestDelta)
					{
						TheEvent = kvp.Key;
						smallestDelta = duration;
					}
				}
				baseCursor = -this.EventToCursorDict[TheEvent]*1.0f;
			}
			*/


			System.TimeSpan duration = StorylineComponent.SelectedTome.End -StorylineComponent.SelectedTome.Start;
			System.TimeSpan delta = StorylineComponent.currentClosestEvent.Date -StorylineComponent.SelectedTome.Start;

			float ratio = delta.Days/ (float)duration.Days;
			baseCursor = -(ratio) * MAX_LENGTH;	//THIS IS NOT ACCURATE

			//currentCursor = 0;
			/*
			int smallestDelta= (StorylineComponent.SelectedTome.End -StorylineComponent.SelectedTome.Start).Days;

			if(this.EventToCursorDict.Count==0)
				currentCursor = 0;
			else
			{
				//Get nearest Event from current Date
				float chosenCursor=0f;
				Evvent TheEvent = null;
				foreach( KeyValuePair<Evvent, float> kvp in this.EventToCursorDict)
				{
					if( (kvp.Key.Date -StorylineComponent.SelectedTome.Start).Days  < smallestDelta)
						TheEvent = kvp.Key;
				}
				chosenCursor = this.EventToCursorDict[TheEvent];
				currentCursor = chosenCursor;
			}*/

			//Maybe we have to put the current Date at the CENTER of the Screen
			//currentCursor += Screen.width *0.5f;

			drawCalendar();

			//GUI.Button( new Rect(Screen.width*0.3f,Screen.height*0.5f,Screen.width*0.6f,Screen.height*0.3f),  StorylineComponent.getCurrentEvent().Name+"\n"+this.StorylineComponent.getCurrentEvent().Info.Replace('.','\n'));
			GUI.Button( new Rect(Screen.width*0.3f,Screen.height*0.5f,Screen.width*0.6f,Screen.height*0.1f),  StorylineComponent.getCurrentEvent().Name);
			GUI.Button( new Rect(Screen.width*0.3f,Screen.height*0.6f,Screen.width*0.6f,Screen.height*0.3f),  this.StorylineComponent.getCurrentEvent().Info.Replace('.','\n'));


			/*
			if(Input.touchCount>0)
			{
				Vector2 FrameMovement = Input.GetTouch(0).deltaPosition;
				currentCursor += FrameMovement.x;
			}
			*/


		}
	}




	void drawCalendar()
	{	//Draw the whole saga outside of the bounds/
		float ATOM_WIDTH = Screen.width * 0.2f; //smallest bucket is the day.
		
		float ATOM_HEIGHT = Screen.height * 0.1f; //std line
		
		int currentMonth = 0;
		int currentYear = 0;
		MAX_LENGTH =0f;

		float SmallestDeltaFromScreen=float.MaxValue;
		Evvent centralEvent;
		centralEvent = StorylineComponent.getCurrentEvent();

		currentCursorIt = this.baseCursor;

		foreach(Evvent currentEvent in StorylineComponent.StoryLine)    //Be carefull, The list must be ordered chronologically...
		{
			System.DateTime currentEventDate = currentEvent.Date;
			
			#region YEAR_LINE
			//Do we have change year ?
			if( currentEventDate.Year !=  currentYear)
			{
				int cnt = countEventsForYear( currentEventDate.Year );
				if(GUI.Button( new Rect ( currentCursorIt,ATOM_HEIGHT, cnt*ATOM_WIDTH, ATOM_HEIGHT) , currentEventDate.Year.ToString() ))
					Debug.Log (currentEventDate.Year.ToString());
				currentYear = currentEventDate.Year;
				MAX_LENGTH += cnt*ATOM_WIDTH;
			}
			#endregion
			
			#region MONTH_LINE
			//Do we have change month ?
			if( currentEventDate.Month !=  currentMonth)
			{
				int cnt = countEventsForMonth( currentEventDate.Month, currentEventDate.Year );
				GUI.Button( new Rect ( currentCursorIt,ATOM_HEIGHT*2, cnt*ATOM_WIDTH, ATOM_HEIGHT) , currentEventDate.ToString("MMMM") );
				currentMonth = currentEventDate.Month;

			}
			#endregion
			
			GUI.Button( new Rect ( currentCursorIt,ATOM_HEIGHT*3, ATOM_WIDTH, ATOM_HEIGHT) , currentEventDate.Day.ToString() );
			this.EventToCursorDict[currentEvent]= currentCursorIt;	//storing the offset from base

			//Debug.Log(new Rect ( currentCursor,ATOM_HEIGHT*3, ATOM_WIDTH, ATOM_HEIGHT) );
			//Debug.Log( currentEventDate.Day.ToString() );
			currentCursorIt += ATOM_WIDTH;

			//calculate delta
			if( ( Mathf.Abs(Screen.width*0.5f-currentCursorIt))< SmallestDeltaFromScreen )
			{
				SmallestDeltaFromScreen = Mathf.Abs(Screen.width*0.5f-currentCursorIt);
				centralEvent= currentEvent;
			}


		}

		//Debug.Log ("Central Event is ="+centralEvent.Date.Day+"/"+centralEvent.Date.Month+"/"+centralEvent.Date.Year);
		//System.TimeSpan delta = centralEvent.Date - StorylineComponent.currentDate;
		//TimeSpan currentDateAsDuration = StorylineComponent.currentDate - StorylineComponent.SelectedTome.Start;
		//TimeSpan centralEventDateAsDuration = centralEvent.Date - StorylineComponent.SelectedTome.Start;
		//float ratio = (currentDateAsDuration.Days+1) / (float)centralEventDateAsDuration.Days;

		//Debug.Log ( StorylineComponent.SelectedTome.Start );

		//Debug.Log (currentDateAsDuration.Days);
		//Debug.Log (centralEventDateAsDuration.Days);
		//Debug.Log (ratio);
		//currentCursor *= ratio;

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
