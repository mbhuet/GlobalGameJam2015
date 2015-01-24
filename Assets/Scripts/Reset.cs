using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if (GUI.Button(new Rect(0, 0, Screen.width/10f, Screen.width/10f), "RESET")){
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
