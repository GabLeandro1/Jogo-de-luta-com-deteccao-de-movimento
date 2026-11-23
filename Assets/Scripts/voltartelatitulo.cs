using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class voltartelatitulo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool apertou = Input.GetKeyDown(KeyCode.Backspace);
		if (apertou == true)
        {
			SceneManager.LoadScene("tela de titulo");
        }
	}
}
