using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour {

	public string objectName;
	public Texture2D buildImage;
	public int cost;
	//Consider selling value as a non-resource attribute
	//Could be managed, but not on a per-instance basis
	public int sellValue;
	public int currentHitPoints;
	public int maxHitPoints;

	protected Player player;
	protected string[] actions = {};
	protected bool currentlySelected = false;

	protected virtual void Awake(){}

	// Use this for initialization
	protected virtual void Start () {
		this.player = transform.root.GetComponentInChildren< Player> ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {}

	protected virtual void OnGUI(){}

	public void SetSelection(bool selected){
		this.currentlySelected = selected;
	}

	public string[] getActions(){
		return this.actions;
	}
	public virtual void PerformAction(string actionToPerform){
		//Each subclass will specify which actions to perform
	}

	public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller){
		//"only handle input if currently selected"
		//And if the "Ground" is not selected
		if (currentlySelected && hitObject && hitObject.name != "Ground") {
			WorldObject worldObject = hitObject.transform.root.GetComponent< WorldObject >();
			//"clicked on another selectable object"
			if(worldObject){
				this.ChangeSelection(worldObject, controller);
			}
		}
	}

	/**
	 * This method sets the WorldObject sent as parameter as selected 
	 * to the Player object sent as parameter.
	 * 
	 * Used to change selection from one object to another.
	 * Input should be taken into account when selecting and deselecting multiple objects.
	 * So, this code might be changed.
	 * //IMPROVE
	 * 
	 * @param	worldObject	The WorldObject to be selected by the player sent as parameter.
	 * @param	controller	The player about to select the WorldObject sent as parameter.
	 */
	private void ChangeSelection(WorldObject worldObject, Player controller){
		//"this should be called by the following line, but there is an outside chance it will not"
		//Unselect this, in case it is already selected
		this.SetSelection(false);
		//If player has a selected object already
		if(controller.selectedObject){
			//Unselect it, via the object
			controller.selectedObject.SetSelection(false);
		}
		//Set the player's selected object to the WorldObject passed as parameter
		controller.selectedObject = worldObject;
		//Bidirectional relation: Set the worldObject received as selected.
		worldObject.SetSelection (true);
	}
}
