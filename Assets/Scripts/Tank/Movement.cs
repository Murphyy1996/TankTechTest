using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    //Inspector variables
    [SerializeField] [Header("Configuration")] private string movementAxis = "Horizontal";
    [SerializeField] [Range(0.1f, 10f)] private float movementSpeed = 2f;
    [SerializeField] [Range(0, 2)] private float gravityAmount = 1f;
    //Non inspector variables
    private Rigidbody2D thisRB;
    private float playerInput = 0;

    private void Awake() //Get any required components on awake
    {
        thisRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Get axis in update for responsive controls
        playerInput = Input.GetAxis(movementAxis);
    }

    private void FixedUpdate() //Calculate and apply the movement code here
    {
        //Calculate the player speed here (I have multiplied the player speed by 100 so that it is simple in the inpsector)
        float calculatedSpeed = playerInput * ((movementSpeed * 100) * Time.deltaTime);
        //Set the movement vector direction and the speed
        Vector3 movementVector = transform.right * calculatedSpeed;
        //Apply the calculated velocity to this rigidbody
        thisRB.velocity = movementVector;
    }
}
