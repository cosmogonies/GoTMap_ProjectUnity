using UnityEngine;
using System.Collections;

public class GUI_Label : MonoBehaviour 
{	//This Behaviour attach the name of each GameObject's Gizmos (character, locations, etc..)
	
	private TextMesh MyTextMesh;
	public Font MyFont;

	void Start () 
	{
		MyTextMesh =  gameObject.AddComponent("TextMesh") as TextMesh;
		MyTextMesh.text = this.transform.name ;  //We use object name as display name.
		MyTextMesh.transform.position = this.transform.position;
		MyTextMesh.font = MyFont;
		
		MeshRenderer mr = gameObject.GetComponent<MeshRenderer>() as MeshRenderer; 
		mr.material = MyFont.material;
		
		this.transform.localScale = new Vector3(50.0f,50.0f,50.0f); //TODO: CLEAN this magic number

		this.transform.Rotate(75.0f,0.0f,0.0f,Space.Self); // we orient it a little to better feedback
	}
	
	void Update()
	{
		//	Commented because weird looking feedback
		//if(Label!=null)
		//	MyTextMesh.text = this.Label;
		//this.transform.LookAt( Camera.mainCamera.transform.position);
	}
	
	
}
