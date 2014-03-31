using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BHV_PathTime : MonoBehaviour 
{	//Display of path according to time and given character.
	//We create "segments", as moving cubes to materialize path.
	//Scale indicates how near we are of current Date.
	
	public Color CharacterColor;
	
	public float REACH_THRESHOLD=10.0f;
	
	public float Speed=1000.0f;
	
	public List<GameObject>DestinationArray;


	
	private Dictionary<GameObject,int> GoalDict; // segment => its goal index in DestinationArray
	
	private bool isInitialising=true;
	
	//a "segment" is a path portion materialised as moving cubes.
	//private List<GameObject> MovingSegments;	
	//public int SEGMENT_OFFSET=5; //frame separated each segments	
	//private Vector3 SEGMENT_SCALE = new Vector3( 30.0f, 30.0f, 100.0f );
	
	void Start () 
	{
		//MovingSegments = new List<GameObject>();
		this.GoalDict = new Dictionary<GameObject, int>();
		
		//All segments are created at the first destination.		
//		for(int i=0;i<SEGMENT_NUMBER;i++)
//		{
//			GameObject newOne = GameObject.CreatePrimitive(PrimitiveType.Cube);
//			newOne.name = this.name+"_"+i;
//			newOne.transform.position = DestinationArray[0].transform.position;
//			newOne.transform.parent = this.transform;
//			
//			//newOne.transform.localScale *=100.0f;
//			newOne.transform.localScale = new Vector3( newOne.transform.localScale.x*30.0f, newOne.transform.localScale.y*30.0f, 100.0f );
//			
//			MovingSegments[i] = newOne;
//			//this.GoalDict[newOne] = DestinationArray[1];
//			this.GoalDict[newOne] = 1;
//		}
	}

	/*
	GameObject createSegment()
	{
		GameObject newOne = GameObject.CreatePrimitive(PrimitiveType.Cube);
		newOne.name = this.name; //+"_"+i;
		newOne.transform.position = DestinationArray[0].transform.position;
		newOne.transform.parent = this.transform;
		newOne.renderer.material.color = this.CharacterColor;
		newOne.transform.localScale = new Vector3( newOne.transform.localScale.x*30.0f, newOne.transform.localScale.y*30.0f, 100.0f );
		
		this.GoalDict[newOne] = 1;
		this.MovingSegments.Add(newOne);
		return newOne;
	}
	*/

	void Update () 
	{
		//Getting current Time (ulgy method):
		GUI_Main comp = Camera.main.GetComponent<GUI_Main>() as GUI_Main;
		float TimeScrubbRatio = comp.ScrollValue;






		/*
		if (this.isInitialising)
		{
			if((Time.renderedFrameCount)%this.SEGMENT_OFFSET==0 )
			{
				this.MovingSegments.Add( createSegment() );
			}
		}
		*/


		/*
		foreach( GameObject currentSegment in this.MovingSegments)
		{
			GameObject currentGoal = this.DestinationArray[this.GoalDict[currentSegment]];			
			Vector3 delta = (currentSegment.transform.position - currentGoal.transform.position);
			
			if(delta.magnitude < this.REACH_THRESHOLD)
			{	//Goal is reached, must aimed the next one
				int idx = this.GoalDict[currentSegment];
				
				if(idx >= this.DestinationArray.Count-1)//0-based
				{
					this.isInitialising=false;
					
					idx =0;
					currentSegment.transform.position = this.DestinationArray[0].transform.position;
				}
				GameObject newGoal = this.DestinationArray[idx+1];
				this.GoalDict[currentSegment] = idx+1;
			}

			
			Vector3 goal = currentGoal.transform.position;
			currentSegment.transform.LookAt(goal);
			currentSegment.transform.Translate(0.0f,0.0f,this.Speed*Time.deltaTime);
			
			#region Scale Management
			//Scale management:
			float currentSegmentRatio = this.GoalDict[currentSegment] / (float)this.DestinationArray.Count;
			float currentSegmentDeltaScrubbTime = Mathf.Abs(TimeScrubbRatio-currentSegmentRatio);
			//float RemainingPathToGoal = (currentSegment.transform.position - currentGoal.transform.position).magnitude;
			float scaleOffset = 1.0f+ currentSegmentDeltaScrubbTime*10 ;
			currentSegment.transform.localScale = this.SEGMENT_SCALE/scaleOffset;
			#endregion
			
		}
		*/
	}
}
