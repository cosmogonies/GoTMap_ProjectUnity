using UnityEngine;
using System.Collections;

public class BHV_CameraMotion : MonoBehaviour 
{
	
	public float Speed;

	public Camera selectedCamera; 
	
	private Vector2 Inertia;
	
	private float FingerDistance=0.0f;
	
	private Vector3 LastMousePosition;
	private Vector3 LastCameraPosition;
	
	public float LIMIT_Down;
	public float LIMIT_Up;
	public float LIMIT_Right;
	public float LIMIT_Left;
	
	private float ScaleValue;
	
	public GameObject Following;


	Vector2 currentFinger1 = Vector2.zero;
	Vector2 previousFinger1 = Vector2.zero;
	Vector2 currentFinger2 = Vector2.zero;
	Vector2 previousFinger2 = Vector2.zero;

	//used for holding our distances and calculating our zoomFactor
	float currentFingerDistance = 0.0f;
	float previousFingerDistance = 0.0f;
	float zoomFactor = 0.0f;

	
	private const float TITLE_HEIGHT=50.0f; // TO_CLEAN: this hardcoded value is stored also in GUI_Main.cs
	
	
	private Vector3 MapLocationAimed;


	void Start () 
	{
		this.Following=null;
		ScaleValue = Camera.main.transform.position.y;
	}
	
	void Update () 
	{
		//Inertia management

		if (this.Inertia.magnitude>1.0f)
		{
			this.Inertia *= 0.9f;
			Move(this.Inertia.x,this.Inertia.y);
		}


		#region Keyboard Control
		/*
		//if( Input.GetKeyDown(KeyCode.PageUp))
		if( Input.GetKey(KeyCode.PageUp))
		{
			Dolly(-1.0f*Speed);
			//selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView - (1 * Speed),2,90);
		}
		if( Input.GetKey(KeyCode.PageDown))
		{
			Dolly(Speed);
			//selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView - (1 * Speed),2,90);
		}		
		if( Input.GetKey(KeyCode.UpArrow))
			Move(0.0f,Speed);
		if( Input.GetKey(KeyCode.DownArrow))
			Move(0.0f,-1.0f*Speed);
		if( Input.GetKey(KeyCode.RightArrow))
			Move(Speed,0.0f);
		if( Input.GetKey(KeyCode.LeftArrow))
			Move(-1.0f*Speed,0.0f);
			*/
		#endregion
		
		//Warning, TOUCH is considered as A MOUSE, to be careful to COMMENT this whole region when releasing to touchDevices !
//		#region Mouse Control
//		if(Input.GetMouseButtonDown(0))
//		{
//			LastMousePosition = Input.mousePosition;
//			
//			LastCameraPosition = Camera.main.transform.position;
//			/*
//			Debug.Log ("Starting slide from="+LastMousePosition.ToString());
//			Vector3 ProjectedFinger = Camera.main.ScreenToWorldPoint(LastMousePosition);
//			Debug.Log ("ProjectedFinger="+ProjectedFinger.ToString());
//			
//			LastMousePosition.y =42.0f;
//			
//			GameObject test = GameObject.CreatePrimitive(PrimitiveType.Cube) ;
//			test.transform.position = LastMousePosition;
//			test.transform.localScale*=15.0f;
//			*/
//		}
//		
//		if(Input.GetMouseButton(0))
//		{
//			//if(Screen.height - Input.mousePosition.y>TITLE_HEIGHT) //do not activate when
//			if(Input.mousePosition.y>TITLE_HEIGHT*1.5f)
//			{
//				
//				//Vector3 delta = Input.mousePosition - LastMousePosition;
//				//Debug.Log("delta is ="+delta.ToString());
//				//delta.Normalize();
//				//Move(-1.0f*delta.x*Speed*0.1f,-1.0f*delta.y*Speed*0.1f);
//				
//				//Vector3 FingerPosition2D = Input.mousePosition;				
//				//Vector3 FingerPosition3D = Camera.mainCamera.ScreenToWorldPoint(FingerPosition2D);
//				
//				//Vector3 delta = LastMousePosition - FingerPosition3D;
//				
//				//Debug.Log (delta.ToString());
//				//Vector3 basePosition = Camera.mainCamera.ScreenToWorldPoint(LastMousePosition);
//				
//				
//				Vector3 delta = Input.mousePosition - LastMousePosition;
//				delta.z = delta.y;
//				delta.y =0.0f;
//				delta *=-1.0f;
//				
//				delta *=Speed;
//				
//				//Camera.mainCamera.transform.position = LastMousePosition + delta;
//				//Camera.main.transform.Translate(delta);
//				
//				Vector3 newPostion = LastCameraPosition + delta;
//				
//				newPostion.y = Camera.main.transform.position.y; //keeping scale for external dolly behaviours.
//
//				//Camera.main.transform.position =  newPostion;
//			}
//		}
//		
//        if (Input.GetAxis("Mouse ScrollWheel") != 0) 
//		{
//            float delta = Input.GetAxis("Mouse ScrollWheel");
//			Dolly(delta*Speed*10f);
//        }
//		#endregion
		
		#region Touch Control
		if(Input.touchCount>0)
		{
			//if(Input.GetTouch(0).position.y< Screen.height-TITLE_HEIGHT) //do not activate when
			if(Input.GetTouch(0).position.y>TITLE_HEIGHT) //do not activate when
			{
				if(Input.touchCount==1)
				{
					Move(Input.GetTouch(0).deltaPosition.x*-1.0f,Input.GetTouch(0).deltaPosition.y*-1.0f);
					
					/*
					if (Input.touches[0].phase == TouchPhase.Began)
					{
					Vector2 FingerPosition2D = Input.GetTouch(0).position-Input.GetTouch(1).position;
					Vector3 FingerPosition3D = Camera.mainCamera.ScreenToWorldPoint(FingerPosition2D);
					
					MapLocationAimed = new Vector3( FingerPosition3D.x, 0.0f, FingerPosition3D.z ); 
						
					}
					else
					{
						Vector2 delta = Input.GetTouch(0).deltaPosition ;
						
						Vector2 PreviousFinger = Input.GetTouch(0).position - delta;
							
						Vector3 preV = Camera.mainCamera.ScreenToWorldPoint(PreviousFinger);
						
					}
					*/
					//Camera.mainCamera.scre
					//Goal;
					
					
					//Vector2 delta = Input.GetTouch(0).deltaPosition;
					//Vector2 FingerPosition2D = Input.GetTouch(0).position;
					//Vector3 FingerPosition3D = Camera.mainCamera.ScreenToWorldPoint(FingerPosition2D);
					//Camera.mainCamera.transform.position = FingerPosition3D;
					
					
					
					
					// TOUCH PHASE = Begin
					
					//1- First we have to  project current Finger on MAP
					
					
					
					// ELSE (DRAGGING)
					
					
					//1- Project finger position on map.
					
					//2- Substract to begin position to get a delta vector.
					
					//3- Move the camera according to the opposite of this vector. 
					// The location aimed by beginPhase MUST always be under the finger.
					
				}

				/*
				if (Input.touchCount == 2) // Pinch And Zoom
				{

					float currentDistance = (Input.GetTouch(0).position-Input.GetTouch(1).position).magnitude ;
					//currentDistance *=10.0f;
					
					if(this.FingerDistance==0.0f)
					{
						this.FingerDistance = currentDistance;
					}
					else
					{
						if (currentDistance > this.FingerDistance)
						{
							Dolly(currentDistance);
						}
						else
						{
							Dolly(-1.0f*currentDistance);
						}
						this.FingerDistance = currentDistance;
					}

				}
				else
				{
					this.FingerDistance=0.0f;
				}
				*/
			}

			if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
			{
				Zoom();
			}

			/*
			if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
			{
				curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
				prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
				touchDelta = curDist.magnitude - prevDist.magnitude;
				speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
				speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;
				
				if ((touchDelta + varianceInDistances <= 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
				{
					//selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView + (1 * speed),15,90);
					//selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView + (1 * speed),2,90);
				}
				
				if ((touchDelta +varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
				{
					//selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView - (1 * speed),2,90);
				}
			} 			
			*/
		}
		#endregion
		
		
		if( this.Following != null )
		{
			Vector3 vDelta = this.transform.position - this.Following.transform.position;
			vDelta.y =0.0f;
			this.transform.Translate( -0.1f * vDelta, Space.World);
		}
		
	}
	
	void OnGUI()
	{
		/*	Let's disable vertical Slider for now..
		float PreviousScale = ScaleValue;
		ScaleValue = GUI.VerticalSlider(new Rect(Screen.width-10.0f,50.0f,10.0f,Screen.height-50.0f) , ScaleValue, this.LIMIT_Up, this.LIMIT_Down);
		if(ScaleValue != PreviousScale)
		{
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, ScaleValue, Camera.main.transform.position.z);
		}
		*/
		
		GUI.Label( new Rect(Screen.width-100,Screen.height-Screen.height*0.1f,50.0f,Screen.height*0.05f), "Follow:");
		string DisplayName="";
		if(this.Following != null)
			DisplayName = this.Following.name;
		else
			DisplayName = "-";
		if ( GUI.Button( new Rect(Screen.width-50,Screen.height-Screen.height*0.1f,50.0f,Screen.height*0.05f), DisplayName ) )
		{
			this.Following = null;
		}
	}
	
	void Move(float _OffsetX=0.0f, float _OffsetY=0.0f)
	{
		//Debug.Log ("Move Called with "+_OffsetX+" "+_OffsetY);

		//this.transform.Translate(_OffsetX*Time.deltaTime,_OffsetY*Time.deltaTime,0.0f, Space.Self);
		//this.transform.Translate(_OffsetX*Time.deltaTime,_OffsetY*Time.deltaTime,0.0f, Space.World);

		Vector3 realMovement = new Vector3(_OffsetX*Speed* Time.deltaTime,0.0f,_OffsetY*Speed* Time.deltaTime);

		Vector3 newPosition = this.transform.position + realMovement;

		newPosition.x = Mathf.Clamp(newPosition.x,-1200f, 3000f);
		newPosition.z = Mathf.Clamp(newPosition.z,-6000f, 6000f);

		this.transform.position = newPosition;
		//Debug.Log ("Move to "+newPosition);

		//this.transform.Translate(realMovement, Space.World);
		//this.Inertia = new Vector2(_OffsetX ,_OffsetY);
	}
	
	void Dolly(float _Offset)
	{
		/*
		if(_Offset > 0.0f) //forward
		{
			if( this.transform.position.y < this.LIMIT_Down)
				return;
		}	
		if(_Offset < 0.0f)
		{
			if( this.transform.position.y > this.LIMIT_Up)
				return;
		}
		*/


		//this.transform.Translate(0.0f,0.0f,_Offset*Speed, Space.Self);		
		//apply zoom to our camera
		//Camera.mainCamera.transform.Translate(Vector3.forward * _Offset * Speed * Time.deltaTime);
		Vector3 vOffset = new Vector3(0f, _Offset*Speed*Time.deltaTime,0f);

		Vector3 newPosition = Camera.mainCamera.transform.position + vOffset;

		newPosition.y = Mathf.Clamp( newPosition.y, this.LIMIT_Down, this.LIMIT_Up);
		Camera.mainCamera.transform.position = newPosition;
	}

	//Source from http://www.devination.com/2013/07/unity-tutorial-touch-pinch-zoom.html
	//find distance between the 2 touches 1 frame before & current frame
	//if the delta distance increased, zoom in, if delta distance decreased, zoom out
	void Zoom()
	{
		//Caches touch positions for each finger
		this.currentFinger1 = Input.GetTouch(0).position;
		this.previousFinger1 = this.currentFinger1 - Input.GetTouch(0).deltaPosition;
		this.currentFinger2 = Input.GetTouch(1).position;
		this.previousFinger2 = this.currentFinger2 - Input.GetTouch(1).deltaPosition;
		
		//finds the distance between your moved touches
		//we dont want to find the distance between 1 finger and nothing
		if (Input.touchCount >= 2)
		{
			this.currentFingerDistance = Vector2.Distance(this.currentFinger1, this.currentFinger2);
			this.previousFingerDistance = Vector2.Distance(this.previousFinger1, this.previousFinger2);
		}
		else
		{
			this.currentFingerDistance = 0.0f;
			this.previousFingerDistance = 0.0f;
		}
		
		//Calculate the zoom magnitude
		//zoomFactor = Mathf.Clamp(this.previousFingerDistance - this.currentFingerDistance, -30.0f, 30.0f);
		
		//apply zoom to our camera
		Vector3 vOffset = new Vector3(0f, (this.previousFingerDistance - this.currentFingerDistance)*Speed*Time.deltaTime,0f);
		
		Vector3 newPosition = Camera.mainCamera.transform.position + vOffset;
		
		newPosition.y = Mathf.Clamp( newPosition.y, this.LIMIT_Down, this.LIMIT_Up);
		Camera.mainCamera.transform.position = newPosition;

		//Camera.mainCamera.transform.Translate(Vector3.forward * -1*zoomFactor * Speed * Time.deltaTime);
		
	}
	
}
