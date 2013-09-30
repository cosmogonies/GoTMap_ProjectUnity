using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BHV_Storyline : MonoBehaviour 
{	//Management of interaction involving the story.
	
	public GameObject Pawn_Male;
	public GameObject Pawn_Female;
	
	CLS_Database TheDatabase;
	private List<Evvent> StoryLine;
	
	private Dictionary<string,GameObject> CharacterDict;
	
	private Dictionary<string,List<Evvent>> CharacterMotion; // CharName => <event1,event2>
	
	private Dictionary<GameObject,GameObject> CharacterComponents; // CharName => CharacterMotion

	public Font MyFont;
	
	
	
	void Start () 
	{
		this.TheDatabase = CLS_Database.Instance;
		
		this.CharacterDict = new Dictionary<string, GameObject>();
		this.CharacterMotion = new Dictionary<string, List<Evvent>>();
		this.CharacterComponents = new Dictionary<GameObject, GameObject>();
		
		string Buffer=""+
					"01|The seed is strong|King's Landing|Robert,Cersei,Joffrey,Jaime,Tyrion, Tommen, Myrcella\n"+
					"05|A sister's accusation|Winterfell|Catelyn\n"+
					"10|New Hand|Winterfell|Eddard,Catelyn,Robb,Bran,Rickon,Sansa,Arya,Theon,Jon,Robert,Cersei,Joffrey,Jaime,Tyrion\n"+
					"11|Witness fall|Winterfell|Bran,Jaime,Cersei\n"+
					//"15|Childish quarrels|-|Eddard,Sansa,Arya,Robert,Cersei,Joffrey\n"+
					"20|Hand Tourney|King's Landing|Eddard,Sansa,Arya,Robert,Cersei,Joffrey,Jaime,Tyrion\n"+
					"22|Noble Brothel|King's Landing|Catelyn,Littlefinger\n"+
					"25|Putch Proposal|King's Landing|Eddard,Renly\n"+
					"30|Crow's vows|Castle Black|Jon,Sam\n"+
					"40|Little Meeting|-|Catelyn,Tyrion\n"+
					"45|Bethroted|King's Landing|Sansa,Joffrey\n"+
					"50|Boar Hunt|Kingswood|Robert,Lancel?\n"+
					"50|Threat|King's Landing|Eddard,Cersei\n"+
					"55|Open prison|Eyrie|Tyrion,Lysa\n"+
					"60|Headless Realm|Red Keep|Eddard,Joffrey\n"+
					"65|King in the north|Winterfell|Robb,Theon,Catelyn\n"+
					"66|Yoren Hiring|-|Arya\n"+
					"70|Frey's Pact|The Twins|Robb,Theon,Catelyn\n"+
					"80|Mountains Clansmen hiring|Bloody Gate|Tyrion\n"+
					"90|Riverrun Trap|Riverrun|Robb,Catelyn,Jaime";
		
		
		//StoryLine creation from Buffer analysis	
		string[] LineList = Buffer.Split('\n');
		
		for(int i=0; i<LineList.Length;i++)
		{
			string[] Tokenize = LineList[i].Split('|');
			
			Evvent newEvent = new Evvent();
			newEvent.HappeningTime = float.Parse( Tokenize[0] ) *0.01f;
			newEvent.Name = Tokenize[1];
			
			newEvent.Location = GameObject.Find(Tokenize[2]);  
			if(newEvent.Location == null)
			{	//Location unknown, looking for matching event name:
				newEvent.Location = GameObject.Find(Tokenize[1]);
				if(newEvent.Location == null)//BUG
					Debug.LogError("ERROR, no location found @ ="+Tokenize[1]+" and "+Tokenize[2]);
			}
			
			string[] CharacterList = Tokenize[3].Split(',');
			for(int j=0; j<CharacterList.Length;j++)
			{
				string CharacterName = CharacterList[j];
				//Debug.Log ("CharacterName="+CharacterName);
				if(! this.CharacterDict.ContainsKey(CharacterName))
				{
					GameObject newCharacter = GameObject.Instantiate(this.Pawn_Male) as GameObject;
					newCharacter.name = CharacterName;
					
					//newCharacter.transform.Rotate(new Vector3(0.0f,180.0f,0.0f),Space.Self);
					newCharacter.transform.Rotate(new Vector3(-30.0f,180.0f,0.0f),Space.Self);
					
					this.CharacterDict[CharacterName] = newCharacter;
					
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
							children[k].gameObject.renderer.material.color = getCharacterColor(CharacterName);
						}

					}
				}
				
				newEvent.Characters.Add( this.CharacterDict[CharacterName] );
				
				if(! CharacterMotion.ContainsKey(CharacterName))
					CharacterMotion[CharacterName] = new List<Evvent>();
				CharacterMotion[CharacterName].Add(newEvent);
								
			}
		}
		
		//Now we have to add a starting point for every characters:
		//foreach(string CharacterName in this.CharacterMotion.Keys)
		//{
		//	this.CharacterMotion[0.0f] = Vector3.zero;
		//}
		
		//Debug
		foreach( KeyValuePair<string, List<Evvent>> kvp in this.CharacterMotion)
		{
			Debug.Log ("CharacterName="+kvp.Key);
			foreach( Evvent ev in kvp.Value)
				Debug.Log ("\t     "+ev.HappeningTime +" => "+ ev.Name);
		}
		
		//MotionPath Creation
		foreach( KeyValuePair<string, List<Evvent>> kvp in this.CharacterMotion)
		{
			Debug.Log(kvp.Key);
			Debug.Log(kvp.Value.Count);
			
			if(kvp.Value.Count>=2)
			{
				GameObject nulle = new GameObject(kvp.Key+"_Motion");
				BHV_PathTime comp = nulle.AddComponent<BHV_PathTime>() as BHV_PathTime;
				
				comp.CharacterColor = getCharacterColor(kvp.Key);
				
				List<GameObject> temp = new List<GameObject>();
				for(int i=0;i<kvp.Value.Count;i++)
					temp.Add(kvp.Value[i].Location);
					//temp[i] = kvp.Value[i].Location;
				comp.DestinationArray = temp;
				
				//comp.enabled = false;
				nulle.SetActive(false);
				
				GameObject Character = this.CharacterDict[kvp.Key];
				//nulle.transform.parent = Character.transform;
				
				//this.CharacterComponents[Character] = comp;
				this.CharacterComponents[Character] = nulle;
			}
		}	
	}
	
	void Update () 
	{
		GUI_Main comp = this.GetComponent<GUI_Main>() as GUI_Main;
		updateCharacters(comp.ScrollValue);
	}
	
	
	void updateCharacters(float _CurrentTimeValue)
	{
		foreach( KeyValuePair<string, List<Evvent>> kvp in this.CharacterMotion)
		{
			string CharName = kvp.Key;
			
			Dictionary<float,Vector3> WayPoints = new Dictionary<float, Vector3>();
			foreach( Evvent ev in kvp.Value)
			{
				//Offset Generation (avoid character stacking by spherical)
				//Vector3 offset = getCharacterOffset(CharName);
				//int i=0;
				int idx = ev.Characters.IndexOf( this.CharacterDict[CharName] );
				float ratio = (float)idx / ev.Characters.Count;
				//ratio = 360.0*ratio;
				ratio = Mathf.PI*2*ratio;
				float Radius=200.0f;
				Vector3 offset = new Vector3( Mathf.Cos(ratio)*Radius, 0.0f, Mathf.Sin(ratio)*Radius );
				
				WayPoints[ev.HappeningTime] = ev.Location.transform.position +offset;
				//Debug.Log (ev.HappeningTime+" => "+ev.Location.name);
			}
			float min = getMinInDict(WayPoints);
			WayPoints[0.0f] = WayPoints[min];
			
			float max = getMaxInDict(WayPoints);
			WayPoints[1.0f] = WayPoints[max];
			
			
			Vector3 pos = GetPosition(_CurrentTimeValue, WayPoints);
			this.CharacterDict[kvp.Key].transform.position = pos;
		}
	}
	

	Vector3 GetPosition(float _TimeValue, Dictionary<float,Vector3> _Dict)
	{	//Main fonction to give a character its position according to current Time and Database dictionary.
		
		float NextTimeEvent = 1.0f;
		float PreviousTimeEvent = 0.0f;

		float NextTimeEvent_Delta = 1.0f;
		float PreviousTimeEvent_Delta = 1.0f;		
		
		
		foreach(KeyValuePair<float,Vector3> kvp in _Dict)
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
		/*
		if(PreviousTimeEvent==0.0f) //everyone must start somewhere
		{
			return _Dict[min];
			//return Vector3.zero;
		}
		if(NextTimeEvent==1.0f) //hum... someone has died I guess
		{
			return _Dict[max];
			//return Vector3.zero;
		}
		*/
		Vector3  prev = _Dict[PreviousTimeEvent];
		Vector3  next = _Dict[NextTimeEvent];
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
		CharacterColorDict["Daenery"] = new Color(1.0f,0.0f,0.0f);
		CharacterColorDict["Eddard"] = new Color(0.550f,0.645f,0.753f);
		
		CharacterColorDict["Jaime"] = new Color(0.5f,0.5f,0.5f);
		CharacterColorDict["Joffrey"] = new Color(0.873f,0.882f,0.484f);
		CharacterColorDict["Jon"] = new Color(1.0f,1.0f,1.0f);
		
		CharacterColorDict["Renly"] = new Color(0.435f,0.652f,0.319f);
		CharacterColorDict["Rickon"] = new Color(0.463f,0.537f,0.620f);
		CharacterColorDict["Robb"] = new Color(0.780f,0.780f,0.780f);
		
		CharacterColorDict["Robert"] = new Color(0.246f,0.431f,0.245f);
		CharacterColorDict["Sansa"] = new Color(0.496f,0.379f,0.569f);
		
		CharacterColorDict["Stannis"] = new Color(0.031f,0.196f,0.031f);
		CharacterColorDict["Theon"] = new Color(0.268f,0.249f,0.471f);
		CharacterColorDict["Tyrion"] = new Color(0.686f,0.471f,0.256f);
		
		if(CharacterColorDict.ContainsKey(_CharacterName))
		{
			return CharacterColorDict[_CharacterName];
		}
		else
		{
			return Color.black;
		}	
	}
	
	
	void refreshCharacterButton(GameObject _Character)
	{
	
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
		
		//GUI.Label( new Rect(targetScreenPosition.x,Screen.height-targetScreenPosition.y,LetterWidth*theName.Length,LetterWidth*1.5f) , theName,LabelStyle);//MAGIC 0.5f: we assume that in average a width letter is half of its height in our font
		if(GUI.Button( new Rect(targetScreenPosition.x,Screen.height-targetScreenPosition.y,100.0f,25.0f) , _Character.name))
		{
			//Declaring if following :
			if(this.CharacterComponents[_Character].activeSelf)
				Camera.main.GetComponent<BHV_CameraMotion>().Following = null;
			else
				Camera.main.GetComponent<BHV_CameraMotion>().Following = _Character;
			
			
			toggleCharacterSelection(_Character);
			
		}
		
	}
	
	void toggleCharacterSelection(GameObject _Character)
	{
		
		this.CharacterComponents[_Character].SetActive( ! this.CharacterComponents[_Character].activeSelf );
		
	}
	
	void OnGUI()
	{
		foreach(KeyValuePair<string,GameObject> kvp in this.CharacterDict)
		{
			refreshCharacterButton(kvp.Value);
		}
		
		
	}
}
