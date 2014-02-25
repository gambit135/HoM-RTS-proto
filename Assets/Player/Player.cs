using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public string userName;
	public bool isHuman;
	public HUD hud;

	/**
	 * 
	 */
	public WorldObject selectedObject { get; set; }	

	// Use this for initialization
	void Start () {
		this.hud = GetComponentInChildren <HUD> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
