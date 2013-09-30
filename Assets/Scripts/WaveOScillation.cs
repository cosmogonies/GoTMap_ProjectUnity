using UnityEngine;
using System.Collections;

public class WaveOScillation : MonoBehaviour 
{	//Utility behaviour to simulate waves animation.
	
	public float radius=0.001f;
	public float OscillationFrequency=0.01f;
	public float OffSet=0.0f; //in frames
	
	private Vector2 Delta = Vector2.zero;
	
	void Start () 
	{
	
	}
	
	
	void Update () 
	{
		this.Delta.x = this.radius*Mathf.Sin(this.OscillationFrequency*(this.OffSet+Time.frameCount));
		this.Delta.y = this.radius*Mathf.Cos(this.OscillationFrequency*(this.OffSet+Time.frameCount));
		this.Delta.y *= 0.5f; //Z is on Y-Axis. We squatch both the delta. 
		
		this.transform.Translate( this.Delta.x,0.0f,  this.Delta.y) ;
	}
}
