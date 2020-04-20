using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 100.0f;
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller =  GetComponent<CharacterController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    void movePlayer()
    {

        float xMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float yMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;


        //Diversi tipi di movimento

        Vector3 forwardMov = transform.forward * yMovement;
        Vector3 rightMov = transform.right * xMovement;

        controller.SimpleMove(forwardMov + rightMov);
    }
}
