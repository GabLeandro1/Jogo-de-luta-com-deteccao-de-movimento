using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mouse : MonoBehaviour
{
    Rigidbody2D CorpoRigido;
    // Use this for initialization
    void Start()
    {
        CorpoRigido = GetComponent<Rigidbody2D>(); 
        CorpoRigido.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position from screen space to world space
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Update the object's position to the mouse position
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    void OnTriggerStay2D(Collider2D objetoTriggerPermaneceTocado)
    {
        string tagPermaneceTrigger = objetoTriggerPermaneceTocado.gameObject.tag;
        if (tagPermaneceTrigger == "botao")
        {
            print("trigger stay correto");
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("jogo");
            }
        }
    }
}
