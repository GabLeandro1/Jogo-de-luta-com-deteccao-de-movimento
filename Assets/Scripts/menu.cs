using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class menu : MonoBehaviour
{
	float X;
	float Y;

	Rigidbody2D CorpoRigido;

	void Start()
	{

		CorpoRigido = GetComponent<Rigidbody2D>();
		CorpoRigido.freezeRotation = true;
	}


	void Update()
	{
		if (Input.GetMouseButtonDown(0) == true)
		{
			SceneManager.LoadScene("jogo");
		}
	}
}