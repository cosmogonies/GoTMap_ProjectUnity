using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class BHV_Storyline : MonoBehaviour 
{	//Management of interaction involving the story.
	public Tome SelectedTome = null ; //if not a Tome, it is a FullTimeLine

	public System.DateTime currentDate;

	public GameObject Pawn_Male;
	public GameObject Pawn_Female;
	
	public CLS_Database TheDatabase;

	//Maybe StoryLine could be moved into CLS_Database..
	public  List<Evvent> StoryLine;
	
	public Dictionary<string,GameObject> CharacterDict;
	public Dictionary<string,List<Evvent>> CharacterMotion; // CharName => <event1,event2>
	public Dictionary<GameObject,LineRenderer> CharacterPath; // CharName => LineRenderer

	//public Font MyFont;

	public TextAsset DatabaseDumpFile;

	void Start () 
	{
		this.TheDatabase = CLS_Database.Instance;
				
		this.CharacterDict = new Dictionary<string, GameObject>();
		this.CharacterMotion = new Dictionary<string, List<Evvent>>();
		this.CharacterPath = new Dictionary<GameObject, LineRenderer>();

		this.StoryLine = new List<Evvent>();

		//Deactivate Kernel MainMenu (and read player options)
		GameObject Kernel = GameObject.FindGameObjectWithTag("Core");
		GUI_MainMenu KernelComp = Kernel.GetComponent<GUI_MainMenu>();


		this.SelectedTome = this.TheDatabase.TomeDict[KernelComp.SelectedTome];	
		Kernel.SetActive(false);

		//Start to read StoryDatabase.
		string[] LineBuffer = DatabaseDumpFile.text.Split('\n');
		Debug.Log ("Read "+LineBuffer.Length+" lines from "+DatabaseDumpFile.name);

		foreach(string currentLine in LineBuffer)
		{
			if(currentLine.Contains("\t"))
			{
				string[] TokkenizedLine = currentLine.Split('\t');
				if( TokkenizedLine[8].Length>0 )	//We consider only the lines with Characters defined in it.
				{
					//Debug.Log (currentLine);

					int Year;
					int.TryParse(TokkenizedLine[0],out Year);

					int Month,Day;
					int.TryParse(TokkenizedLine[1].Split('/')[0], out Month);
					int.TryParse(TokkenizedLine[1].Split('/')[1], out Day);

					string EventName = TokkenizedLine[2];

					string ChapterCode = TokkenizedLine[3];
					string ChapterCharacter = TokkenizedLine[4];
					string BookCode = TokkenizedLine[5];
					int Chapter;
					int.TryParse(TokkenizedLine[6],out Chapter);

					string Citation = TokkenizedLine[7];

					string[] Characters = TokkenizedLine[8].Trim().Split(',');

					string Location = TokkenizedLine[9];

					Evvent newEvent = new Evvent();
					newEvent.Date = new System.DateTime(Year, Month, Day);
					newEvent.Name = EventName;

					if(BookCode!="")
						if(BookCode!="TWOW")
							newEvent.Book = this.TheDatabase.TomeDict[BookCode];


					newEvent.Characters = new List<GameObject>();
					foreach(string currentCharacterName in Characters)
					{
						string currentCharacterName2 = currentCharacterName.Trim();
						//currentCharacterName = currentCharacterName.Trim();
						if(this.CharacterDict.ContainsKey(currentCharacterName2) )
						{
							//Debug.LogWarning ("this.CharacterDict["+currentCharacterName+"]="+this.CharacterDict[currentCharacterName].name);
						}
						else
						{
							//Debug.Log (currentCharacterName+" is NOT in the Dict!!!");
							this.createCharacterPawn(currentCharacterName2);
						}
						newEvent.Characters.Add( this.CharacterDict[currentCharacterName2] );

						if(! CharacterMotion.ContainsKey(currentCharacterName2))
							CharacterMotion[currentCharacterName2] = new List<Evvent>();
						CharacterMotion[currentCharacterName2].Add(newEvent);
					}
					
					newEvent.Location = GameObject.Find(Location);
					if(newEvent.Location == null)
						Debug.LogError("ERROR, no location found for ="+Location);
					//Debug.Log (newEvent.Location.name);

					//Now we have to set the HappeningTime:
					//newEvent.HappeningTime = this.SelectedTome.getRatio(newEvent.Date);
					newEvent.HappeningTime = this.TheDatabase.TomeDict["ASOIAF"].getRatio(newEvent.Date);

					newEvent.Info = Citation;

					StoryLine.Add(newEvent);
				}
				
				//Now we have to add a starting point for every characters:
				//foreach(string CharacterName in this.CharacterMotion.Keys)
				//{
				//	this.CharacterMotion[0.0f] = Vector3.zero;
				//}
				


			}
		}

		//initForTome();

		createMotionPaths();

		#region StoryLineDebugging
		string LoggedBuffer="";
		foreach( KeyValuePair<string, List<Evvent>> kvp in this.CharacterMotion)
		{
			Debug.Log ("CharacterName="+kvp.Key);
			//Tome currentBook=null;
			LoggedBuffer+="\nCharacter: "+kvp.Key;
			foreach( Evvent ev in kvp.Value)
			{
				/*
				if(ev.Book!=null)
				{
					if(ev.Book!=currentBook)
					{
						currentBook = ev.Book;
						LoggedBuffer+="\n\t"+ ev.Book.CodeName;
					}
				}
				*/
				int percent = Mathf.RoundToInt(ev.HappeningTime*100f);
				//Debug.Log ("\t     "+ percent+"% => "+ ev.Location.name+" ("+ev.Name+")");


				LoggedBuffer+="\n\t"+ev.Date.ToString("d/MM/yyy")+" ("+ percent+"%) => "+ ev.Location.name+" ("+ev.Name+")";
			}
		}
		Debug.LogWarning(LoggedBuffer);
		#endregion
	}

	/*
	void initForTome()
	{	// if Tome are not the first ones, we have to put characters in correct previous tome position.

	}
	*/

	void createMotionPaths()
	{
		//MotionPath Creation
		float YOffset = 333f;
		float LineWidth = 50f;
		foreach( KeyValuePair<string, List<Evvent>> kvp in this.CharacterMotion)
		{
			if(kvp.Value.Count>=2)
			{
				GameObject Line = new GameObject(kvp.Key+"_Motion");
				LineRenderer TheLineRendererComp = Line.AddComponent<LineRenderer>();
				TheLineRendererComp.SetWidth(LineWidth,LineWidth);
				
				//TheLineRendererComp.material = new Material (Shader.Find("Diffuse"));
				//TheLineRendererComp.material = new Material (Shader.Find("Mobile/Unlit (Supports Lightmap)"));
				TheLineRendererComp.material = new Material (Shader.Find("Custom/MobileTransparent"));
				TheLineRendererComp.material.color = getCharacterColor(kvp.Key);
				Line.SetActive(false);
				
				CharacterPath[ this.CharacterDict[kvp.Key] ] = TheLineRendererComp;
				TheLineRendererComp.SetVertexCount(kvp.Value.Count);
				for(int i=0;i<kvp.Value.Count;i++)
				{
					TheLineRendererComp.SetPosition(i, kvp.Value[i].Location.transform.position + new Vector3(0,YOffset,0)); 
				}
			}
		}	
	}

	
	void createCharacterPawn(string _CharacterName)
	{
		Debug.Log ("Creating pawn for char="+_CharacterName);
		GameObject newCharacter = GameObject.Instantiate(this.Pawn_Male) as GameObject;
		newCharacter.name = _CharacterName;
		newCharacter.transform.Rotate(new Vector3(-30.0f,180.0f,0.0f),Space.Self);
		
		Transform[] children = newCharacter.GetComponentsInChildren<Transform>();
		for(int k=0;k<children.Length;k++)
		{
			if (children[k].name=="Label")
			{
				Transform LabelTransform = children[k];
				LabelTransform.Rotate(new Vector3(0.0f,180.0f,0.0f),Space.Self);  //to clean in mode
				
				//GUI_Label comp = children[k].gameObject.AddComponent<GUI_Label>() as GUI_Label;
				//comp.MyFont = this.MyFont;
				//comp.Label = CharacterName;
				
				/*
				TextMesh MyTextMesh =  LabelTransform.gameObject.gameObject.AddComponent("TextMesh") as TextMesh;
				MyTextMesh.text = CharacterName ;
				MyTextMesh.transform.position = LabelTransform.position;
				MyTextMesh.font = MyFont;
				MeshRenderer mr = LabelTransform.gameObject.AddComponent("MeshRenderer") as MeshRenderer;
				mr.material = MyFont.material;
				mr.material.color = new Color(0.8f,0.2f,0.2f,1.0f);//Color.red;
				*/
				LabelTransform.localScale = new Vector3(50.0f,50.0f,50.0f); //TOCLEAN
				
			}

			if (children[k].name=="Body" || children[k].name=="Head")
			{
				children[k].gameObject.renderer.material.color = getCharacterColor(_CharacterName);
			}

		}		
		
		this.CharacterDict[_CharacterName] = newCharacter;
	}


	void Update () 
	{
		GUI_Main comp = this.GetComponent<GUI_Main>() as GUI_Main;

		if(this.SelectedTome == this.TheDatabase.TomeDict["ASOIAF"])
		{
			updateCharacters(comp.ScrollValue);
			this.currentDate = convertRatioToDate(comp.ScrollValue);
		}
		else{

			float LocalTimeRatio = comp.ScrollValue;

			// ScrollValue means the ratio for current selected TOme,
			// so we have to convert it to a maximum update
			DateTime TheDate = convertRatioToDate(comp.ScrollValue);
		
			//float GlobalTimeRAtio = convertDateToRatio(TheDate);

			Tome MasterTome = this.TheDatabase.TomeDict["ASOIAF"]; //TODO: create a method that returns the MasterTome.
			int DateNumbers = (TheDate - MasterTome.Start).Days;
			int TotalNumbers = (MasterTome.End-MasterTome.Start).Days;
			float GlobalTimeRAtio = ( DateNumbers / (float)TotalNumbers );

			updateCharacters(GlobalTimeRAtio);
			this.currentDate = TheDate;
		}


	}


	System.DateTime convertRatioToDate(float _Ratio)
	{
		int DayNumbers = (this.SelectedTome.End-this.SelectedTome.Start).Days;
		return this.SelectedTome.Start + new TimeSpan(Mathf.RoundToInt(_Ratio*(DayNumbers)),0,0,0);	//TODO: too random, must round to AN EXISTING CLOSE EVENT...
	}
	public float convertDateToRatio(System.DateTime _Date)
	{

		int DateNumbers = (_Date - this.SelectedTome.Start).Days;
		int TotalNumbers = (this.SelectedTome.End-this.SelectedTome.Start).Days;
		return ( DateNumbers / (float)TotalNumbers );

		/*
		Tome MasterTome = this.TheDatabase.TomeDict["ASOIAF"]; //TODO: create a method that returns the MasterTome.
		int DateNumbers = (_Date - MasterTome.Start).Days;
		int TotalNumbers = (MasterTome.End-MasterTome.Start).Days;
		return ( DateNumbers / (float)TotalNumbers );
		*/
	}




	
	void updateCharacters(float _CurrentTimeValue)
	{
		foreach( KeyValuePair<string, List<Evvent>> kvp in this.CharacterMotion)
		{	//Parsing Characters
			string CharName = kvp.Key;
			
			Dictionary<float,Vector3> WayPoints = new Dictionary<float, Vector3>();
			foreach( Evvent currentEvent in kvp.Value)
			{	//Parsing Character's Events

				//Offset Generation (avoid character stacking by spherical positionning)
				int idx = currentEvent.Characters.IndexOf( this.CharacterDict[CharName] );
				float ratio = (float)idx / currentEvent.Characters.Count;
				ratio = Mathf.PI*2*ratio;
				float Radius=200.0f; //TODO: change RAdius into place occupied in pixels from screen ratio.
				Vector3 offset = new Vector3( Mathf.Cos(ratio)*Radius, 0.0f, Mathf.Sin(ratio)*Radius );

				//Getting a PointBased-MotionPath :
				WayPoints[currentEvent.HappeningTime] = currentEvent.Location.transform.position +offset;
			}

			float min = getMinInDict(WayPoints);
			WayPoints[0.0f] = WayPoints[min];
			
			float max = getMaxInDict(WayPoints);
			WayPoints[1.0f] = WayPoints[max];

			Vector3 pos = GetPosition(_CurrentTimeValue, WayPoints);
			this.CharacterDict[kvp.Key].transform.position = pos;
		}
	}
	

	Vector3 GetPosition(float _TimeValue, Dictionary<float,Vector3> _WayPoints)
	{	//Main fonction to give a character its position according to current Time and Database dictionary.
		
		float NextTimeEvent = 1.0f;
		float PreviousTimeEvent = 0.0f;

		float NextTimeEvent_Delta = 1.0f;
		float PreviousTimeEvent_Delta = 1.0f;		

		foreach(KeyValuePair<float,Vector3> kvp in _WayPoints)
		{
			if(kvp.Key <= _TimeValue)
			{
				if(_TimeValue - kvp.Key < PreviousTimeEvent_Delta)
				{
					PreviousTimeEvent = kvp.Key;
					PreviousTimeEvent_Delta = _TimeValue - kvp.Key;
				}
			}

			if(kvp.Key > _TimeValue)
			{
				if(kvp.Key-_TimeValue< NextTimeEvent_Delta)
				{
					NextTimeEvent = kvp.Key;
					NextTimeEvent_Delta = kvp.Key-_TimeValue;
				}
			}
		}
		Vector3  prev = _WayPoints[PreviousTimeEvent];
		Vector3  next = _WayPoints[NextTimeEvent];
		float LocalRatio = PreviousTimeEvent_Delta / (PreviousTimeEvent_Delta+NextTimeEvent_Delta);
		
		return Vector3.Lerp( prev, next, LocalRatio );
	}	
	
	float getMinInDict(Dictionary<float,Vector3> _Dict)
	{
		float min=float.MaxValue;
		foreach(KeyValuePair<float,Vector3> kvp in _Dict)
		{
			if(kvp.Key<min)
				min = kvp.Key;
		}
		return min;
	}
	float getMaxInDict(Dictionary<float,Vector3> _Dict)
	{
		float max=float.MinValue;
		foreach(KeyValuePair<float,Vector3> kvp in _Dict)
		{
			if(kvp.Key>max)
				max = kvp.Key;
		}
		return max;
	}
		
	
	
	Vector3 getCharacterOffset(string _CharacterName)
	{
		_CharacterName = _CharacterName.ToLower();
		
		int val0 = System.Convert.ToInt32(_CharacterName[0]) -97;
		int val1 = System.Convert.ToInt32(_CharacterName[1]) -97;
		int val2 = System.Convert.ToInt32(_CharacterName[2]) -97;
		// letter are from [0 <-> 26]
		
		val0 -=13;
		val1 -=13;
		val2 -=13;
		
		//Shameless algorithm from name to offset
		//return new Vector3(256-val0-val2, 0.0f, 0+val1+val2)*2.0f;
		Vector3 offset = new Vector3( val0, 0.0f, val2);
		offset.Normalize();
		
		return offset*200.0f;
	}
	
	
	Color getCharacterColor(string _CharacterName)
	{
		//TODO: dict to be externalized
		Dictionary<string, Color> CharacterColorDict = new Dictionary<string, Color>();
		
		CharacterColorDict["Arya"] = new Color(0.463f,0.446f,0.784f);
		CharacterColorDict["Bran"] = new Color(0.444f,0.647f,0.559f);
		CharacterColorDict["Catelyn"] = new Color(0.601f,1.0f,0.894f);
		
		CharacterColorDict["Cersei"] = new Color(0.725f,0.114f,0.318f);
		//CharacterColorDict["Daenery"] = new Color(1.0f,0.0f,0.0f);
		CharacterColorDict["Ned"] = new Color(0.550f,0.645f,0.753f);

		CharacterColorDict["Lysa"] = new Color(0.601f,1.0f,0.894f);
		CharacterColorDict["LittleFinger"] = new Color(0.601f,1.0f,0.894f);
		
		CharacterColorDict["Jaime"] = new Color(0.5f,0.5f,0.5f);
		CharacterColorDict["Joffrey"] = new Color(0.873f,0.882f,0.484f);

		CharacterColorDict["Jon"] = new Color(1.0f,1.0f,1.0f);
		CharacterColorDict["Sam"] = new Color(1.0f,0.8f,0.8f);
		
		CharacterColorDict["Renly"] = new Color(0.435f,0.652f,0.319f);
		CharacterColorDict["Rickon"] = new Color(0.463f,0.537f,0.620f);
		CharacterColorDict["Robb"] = new Color(0.780f,0.780f,0.780f);
		
		CharacterColorDict["Robert"] = new Color(0.246f,0.431f,0.245f);
		CharacterColorDict["Sansa"] = new Color(0.496f,0.379f,0.569f);
		
		CharacterColorDict["Stannis"] = new Color(0.031f,0.196f,0.031f);
		CharacterColorDict["Theon"] = new Color(0.268f,0.249f,0.471f);
		CharacterColorDict["Tyrion"] = new Color(0.686f,0.471f,0.256f);
		CharacterColorDict["Tommen"] = new Color(0.786f,0.471f,0.256f);
		CharacterColorDict["Tywinn"] = new Color(0.886f,0.471f,0.256f);
		
		if(CharacterColorDict.ContainsKey(_CharacterName))
		{
			return CharacterColorDict[_CharacterName];
		}
		else
		{
			//return Color.black;
			return Color.yellow;
		}	
	}
	
	
	public void refreshCharacterButton(GameObject _Character)
	{
	
		//Am I Dead ?
		if (this.TheDatabase.amIDead(this.currentDate, _Character.name))
		{
			return;
		}


		//Vector3 offset = new Vector3(1.0f,3.0f,0.0f); 
		Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(_Character.transform.position);
			
		/*
		float LetterWidth = 16.0f;
		if(Screen.width<500)
			LetterWidth = 8.0f;
		
		if(targetScreenPosition.x + LetterWidth*theName.Length > Screen.width)
			targetScreenPosition.x -= ( (targetScreenPosition.x + LetterWidth*theName.Length)-Screen.width);
		
		if(Screen.height-targetScreenPosition.y < LetterWidth*1.5f )
			targetScreenPosition.y = Screen.height- LetterWidth*1.5f ;
				 */
		GUIContent current = new GUIContent( _Character.name);

		float neededWidth = GUI.skin.GetStyle("button").CalcSize(current).x;

		float SmallOffsetX=24f;

		//GUI.Label( new Rect(targetScreenPosition.x,Screen.height-targetScreenPosition.y,LetterWidth*theName.Length,LetterWidth*1.5f) , theName,LabelStyle);//MAGIC 0.5f: we assume that in average a width letter is half of its height in our font
		if(GUI.Button( new Rect(targetScreenPosition.x-SmallOffsetX,Screen.height-targetScreenPosition.y,neededWidth,25.0f) , current))
		{
			//Declaring if following :
			/*
			if(this.CharacterPath[_Character].gameObject.activeSelf)
				Camera.main.GetComponent<BHV_CameraMotion>().Following = null;
			else
				Camera.main.GetComponent<BHV_CameraMotion>().Following = _Character;

			//toggleCharacterSelection(_Character);
			this.CharacterPath[_Character].gameObject.SetActive( ! this.CharacterPath[_Character].gameObject.activeSelf );
			*/

			toggleCharacterFocus(_Character);
		}
	}




	
	public void toggleCharacterFocus(GameObject _Character)
	{
		Debug.Log ("toggleCharacterFocus("+_Character.name+")");

		bool currentStatus = this.CharacterPath[_Character].gameObject.activeSelf;
		foreach(GameObject currentCharGO in  this.CharacterDict.Values )
		{
			if(currentCharGO==_Character)
				this.CharacterPath[_Character].gameObject.SetActive( ! currentStatus);	//toggling currentOne.
			else
				this.CharacterPath[currentCharGO].gameObject.SetActive(false);
		}

		BHV_CameraMotion CameraMotionComp = Camera.main.GetComponent<BHV_CameraMotion>();
		if(CameraMotionComp.Following == null)
			CameraMotionComp.Following = _Character;
		else
			CameraMotionComp.Following = null;
	}

	public Evvent getCurrentEvent()
	{
		Evvent ClosestEvent = StoryLine[0];
		float DeltaSmallest=1.0f;

		float currentDateAsRatio = this.convertDateToRatio(this.currentDate);	//FIXME: could directly acess to GUIMAIn.ScrollValue...
		foreach(Evvent currentEvent in StoryLine  )
		{
			float currentRatio = this.convertDateToRatio(currentEvent.Date);

			if( Mathf.Abs(currentDateAsRatio-currentRatio)< DeltaSmallest )
			{
				DeltaSmallest = Mathf.Abs(currentDateAsRatio-currentRatio);
				ClosestEvent = currentEvent;
			}
		}

		return ClosestEvent;

		//Okay, BUT, a same ratio could lead to several SAME DATE eventS ....
		//List<Evvent> ClosestEvents = new List<Evvent>();
		//foreach(Evvent currentEvent in StoryLine  )
		//	if(currentEvent.Date == ClosestEvent.Date)
		//		ClosestEvents.Add(currentEvent);
		//if(ClosestEvents.Count>1)
		//	return ClosestEvents[ UnityEngine.Random.Range(0,ClosestEvents.Count) ];
		//else
		//	return ClosestEvent;
	}
	public Evvent getPreviousEvent()
	{
		Evvent PreviousEvent = StoryLine[0];
		float DeltaSmallest=1.0f;
		float currentDateAsRatio = this.convertDateToRatio(this.currentDate);	//FIXME: could directly acess to GUIMAIn.ScrollValue...
		foreach(Evvent currentEvent in StoryLine  )
		{
			if( (this.SelectedTome==this.TheDatabase.TomeDict["ASOIAF"]) || (currentEvent.Book == this.SelectedTome) )
			{
				float currentRatio = this.convertDateToRatio(currentEvent.Date);
				if( currentDateAsRatio-currentRatio>0 )	//currentEvent is a previous one.
				{
					if( currentDateAsRatio-currentRatio< DeltaSmallest )
					{
						DeltaSmallest = currentDateAsRatio-currentRatio;
						PreviousEvent = currentEvent;
					}
				}
			}
		}
		return PreviousEvent;
	}

	//public float getNextEvent()
	public Evvent getNextEvent()
	{
		Evvent NextEvent = StoryLine[0];
		float DeltaSmallest=1.0f;
		float currentDateAsRatio = this.convertDateToRatio(this.currentDate);	//FIXME: could directly acess to GUIMAIn.ScrollValue...
		foreach(Evvent currentEvent in StoryLine  )
		{
			if( (this.SelectedTome==this.TheDatabase.TomeDict["ASOIAF"]) || (currentEvent.Book == this.SelectedTome) )
			{

				float currentRatio = this.convertDateToRatio(currentEvent.Date);
				if( currentRatio - currentDateAsRatio>0 )	//currentEvent is a next one.
				{
					if( currentRatio-currentDateAsRatio< DeltaSmallest )
					{
						DeltaSmallest = currentRatio-currentDateAsRatio;
						NextEvent = currentEvent;
					}
				}
			}
		}
		return NextEvent;
	}


	public string getCurrentEventName()
	{
		/*
		foreach(Evvent currentEvent in this.StoryLine  )
		{
			if( this.currentDate == currentEvent.Date)	//Maybe we can add here a almost equal threshold.... (avoid float aproximation)
				return currentEvent.Name;
		}
		*/
		string SelectedTomeName = "T"+this.SelectedTome.Order +": "+ this.SelectedTome.Name;
		return SelectedTomeName;
	}

	/*
	void OnGUI()
	{
		//We refresh all GUIButtons of each characters :
		foreach(KeyValuePair<string,GameObject> kvp in this.CharacterDict)
		{
			refreshCharacterButton(kvp.Value);
		}

		//Background of Timeslider:
		string SelectedTomeName = "T"+this.GetComponent<BHV_Storyline>().SelectedTome.Order +": "+ this.GetComponent<BHV_Storyline>().SelectedTome.Name;
		GUI.Label(new Rect(Screen.width*0.1f*0.5f,Screen.height*0.9f,Screen.width*0.9f,Screen.height*0.1f) , "    "+this.SelectedTome.Name);

		if ( GUI.Button( new Rect(Screen.width*0.9f,Screen.height-Screen.height*0.05f,Screen.width*0.1f,Screen.height*0.05f), this.currentDate.ToString("dd/MM/yyy")))	//TO Do
		{
			//Call the Zoom Window, this Event Description
		}

	}
	*/
}
