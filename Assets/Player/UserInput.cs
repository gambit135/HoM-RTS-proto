using RTS;
using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour {
	private Player player;
	// Use this for initialization
	void Start () {
		player = transform.root.GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.isHuman) {
			MoveCamera();
			RotateCamera();
			MouseActivity();
		}
	}

	void MouseActivity (){
		if (Input.GetMouseButtonDown (0)) {
			LeftMouseClick ();
		} 
		else {
			if(Input.GetMouseButtonDown (1)){
				RightMouseClick();
			}
		}
	}
	
	private void LeftMouseClick(){
		if (this.player.hud.MouseInBounds ()) {
			GameObject hitObject = FindHitObject();
			Vector3 hitPoint = FindHitPoint();
			if(hitObject && hitPoint != ResourceManager.getInvalidPosition){
				if(this.player.selectedObject){
					player.selectedObject.MouseClick(hitObject, hitPoint, player);
				}
				else{
					if(hitObject.name != "Ground"){
						WorldObject worldObject = hitObject.transform.root.GetComponent< WorldObject >();
						if(worldObject){
							//"We already know the player has no selected object"
							player.selectedObject = worldObject;
							worldObject.SetSelection(true);
						}
					}
				}
			}
		}
	}

	/**
	 * Currently used for deselecting objects. 
	 * Override this behavior, as right click can be used to delegate a destination to move,
	 * or to set a gathering point for a building
	 * 
	 * //IMPROVE
	 */
	private void RightMouseClick (){
		if (player.hud.MouseInBounds () && !Input.GetKey (KeyCode.LeftAlt) && player.selectedObject) {
			player.selectedObject.SetSelection(false);
			player.selectedObject = null;
		}
	}

	private void MoveCamera(){
		float xpos = Input.mousePosition.x;
		float ypos = Input.mousePosition.y;
		Vector3 movement = new Vector3(0,0,0);

		//horizontal camera movement
		//If cursor is on the scrollable area of the screen
		//On x, on the left of the screen
		if(xpos >= 0 && xpos < ResourceManager.ScrollWidth || (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))){ 
			//Substract from Vector3 x value the scrollSpeed value 
			movement.x -= ResourceManager.ScrollSpeed;
		}
		//Else, if it's within the width of the screen, and within the scrollable area
		//On x, on the right of the screen
		else if(xpos <= Screen.width && xpos >Screen.width - ResourceManager.ScrollWidth  || (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))){
			//Substract from Vector3 x value the scrollSpeed value 
			movement.x += ResourceManager.ScrollSpeed;
		}

		//Vertical camera movement
		//If cursor is on the scrollable area of the screen
		//On y, on the top of the screen
		if (ypos >= 0 && ypos < ResourceManager.ScrollWidth  || ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !Input.GetKey (KeyCode.LeftControl) && !Input.GetKey (KeyCode.RightControl))){
			//Substract from Vector 3 z value the scrollSpeed value
			movement.z -= ResourceManager.ScrollSpeed;
		}
		//Same, on the bottom of  the screen
		else if( ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScrollWidth  || ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !Input.GetKey (KeyCode.LeftControl) && !Input.GetKey (KeyCode.RightControl))){
			movement.z += ResourceManager.ScrollSpeed;
		}

		//make sure movement is in the direction the camera is pointing
		//but ignore the vertical tilt of the camera to get sensible scrolling
		movement = Camera.main.transform.TransformDirection(movement);
		movement.y = 0;

		//away from ground movement
		//Validate that CTRL key is not pressed, to not be activated while rotating
		if (!Input.GetKey (KeyCode.LeftControl) && !Input.GetKey (KeyCode.RightControl)) {/* && Input.GetAxis("MouseScrollWheel"))*/
			//movement.x -= ResourceManager.ScrollSpeed * Input.GetAxis ("Mouse ScrollWheel");
			//if (Input.GetAxis("Mouse ScrollWheel") == 0){
				if (Input.GetKey (KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus)){
					movement.y -= ResourceManager.ScrollSpeed;
				}
				else{
					if (Input.GetKey (KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus)){
						movement.y += ResourceManager.ScrollSpeed;
					}
				}
			//}
			//else{
				movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis ("Mouse ScrollWheel");
			//}
			//movement.z -= ResourceManager.ScrollSpeed * Input.GetAxis ("Mouse ScrollWheel");
		}
		else{
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)){
				movement.y -= ResourceManager.ScrollSpeed;
			}
			else{
				if(Input.GetKey (KeyCode.DownArrow)|| Input.GetKey (KeyCode.S)){
					movement.y += ResourceManager.ScrollSpeed;
				}
			}
		}
		//calculate desired camera position based on received input
		Vector3 origin = Camera.main.transform.position;
		Vector3 destination = origin;
		destination.x += movement.x;
		destination.y += movement.y;	
		destination.z += movement.z;

		//limit away from ground movement to be between a minimum and maximum distance
		if(destination.y > ResourceManager.MaxCameraHeight) {
			destination.y = ResourceManager.MaxCameraHeight;
			//destination.x = origin.x;
			//destination.z = origin.z;
		} else if(destination.y < ResourceManager.MinCameraHeight) {
			destination.y = ResourceManager.MinCameraHeight;
			//destination.x = origin.x;
			//destination.z = origin.z;
		}

		//if a change in position is detected perform the necessary update
		if(destination != origin) {
			Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
		}
	}
	private void RotateCamera(){
		Vector3 origin = Camera.main.transform.eulerAngles;
		Vector3 destination = origin;

		//detect rotation amount if CTRL is being held and the Right mouse button is down
		//if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetMouseButton(1)) {
		if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))/* && Input.GetAxis("MouseScrollWheel"))*/ {
			//destination.x -= Input.GetAxis("Mouse Y") * ResourceManager.RotateSpeed;
			//destination.y += Input.GetAxis("Mouse X") * ResourceManager.RotateSpeed;

			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
				destination.y += ResourceManager.RotateSpeed;
				print ("Y euler angle: " + destination.y);
			}
			else{
				if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)){
					destination.y -= ResourceManager.RotateSpeed;
					print ("Y euler angle: " + destination.y);
				}
				else{
					float deltaScroll = ResourceManager.RotateSpeed * Input.GetAxis("Mouse ScrollWheel") /* * 10f   * 57.29578f */;
					if (Input.GetAxis ("Mouse ScrollWheel") != 0 ){
						print ("Delta Scroll: " + deltaScroll);
						destination.y += deltaScroll;
						print ("Y euler angle: " + destination.y);
					}
				}
			}
		}

		//print ("Y euler angle: " + destination.y);

		//if a change in position is detected perform the necessary update
		if(destination != origin) {
			Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.RotateSpeed);

			//Alternative rotate stuff
			//Camera.main.transform.Rotate(destination, Space.World);
		}
	}
	/**
	 * Used to find which object the human player intended to select on-screen.
	 * It casts a ray from the point in which the user clicked the screen,
	 * and finds the first object it encounters on its trajectory.
	 */
	private GameObject FindHitObject(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			return hit.collider.gameObject;
		}
		return null;
	}
	/**
	 * 
	 */
	private Vector3 FindHitPoint(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			return hit.point;
		}
		return ResourceManager.getInvalidPosition;
	}
}
