

using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {


	//Private reference to plaer
	private Player player;

	//Referemce to GUI Skins
	public GUISkin ResourcesSkin, OrdersSkin;

	//Constant values: Width of orders and resources bar
	private const int ORDERS_BAR_WIDTH = 150;
	private const int RESOURCES_BAR_HEIGHT = 40;

	/**
	 * Height for selection name text
	 */
	private const int SELECTION_NAME_HEIGHT = 15;

	// Use this for initialization
	void Start () {
		player = transform.root.GetComponent<Player>();
	
	}
	
	// Update is called once per frame
	/*
	void Update () {
	
	}*/

	void OnGUI(){
		if(player.isHuman){
			DrawOrdersBar();
			DrawResourcesBar();
		}
	}
	void DrawOrdersBar(){
		GUI.skin = OrdersSkin;
		GUI.BeginGroup(
			new Rect(
				Screen.width-HUD.ORDERS_BAR_WIDTH,
				RESOURCES_BAR_HEIGHT,
				ORDERS_BAR_WIDTH,
				Screen.height-RESOURCES_BAR_HEIGHT));
		GUI.Box(
			new Rect(
			0,
			0,
			ORDERS_BAR_WIDTH,
			Screen.height-RESOURCES_BAR_HEIGHT),"");

		//Display selected object
		string selectionName = "";
		if (player.selectedObject) {
			selectionName = player.selectedObject.objectName;
		}
		if(!selectionName.Equals("")){
			GUI.Label(
				new Rect(
				0,
				10,
				ORDERS_BAR_WIDTH,
				SELECTION_NAME_HEIGHT), selectionName);
		}

		GUI.EndGroup();
	}
	private void DrawResourcesBar() {
		GUI.skin = ResourcesSkin;
		GUI.BeginGroup(
			new Rect(
				0,
				0,
				Screen.width,
				RESOURCES_BAR_HEIGHT));
		GUI.Box(
				new Rect(
					0,
					0,
					Screen.width,
					RESOURCES_BAR_HEIGHT),"");
		GUI.EndGroup();
	}
	/**
	 * "This method simply finds the current position of the mouse and determines
	 * whether it is inside the playing area or if it is over part of the HUD."
	 */
	public bool MouseInBounds(){
		//"Screen coordinates start in the lower-left corner of the screen"
		//"not the top-left corner of the screen like drawing coordinates do"
		
		Vector3 mousePos = Input.mousePosition;
		//If the position is actually on the inside of the screen width available for displaying game objects
		//a.k.a., not outside of the screen and not on the orders' bar
		bool insideWidth = mousePos.x >= 0 && mousePos.x <= (Screen.width - ORDERS_BAR_WIDTH);
		//If the position is actually on the inside of the screen height available for displaying game objects
		//a.k.a., not outside of the screen and not on the resources' bar
		bool insideHeight = mousePos.y >= 0 && mousePos.y <= (Screen.height - RESOURCES_BAR_HEIGHT);
		
		return insideWidth && insideHeight;
	}
}
