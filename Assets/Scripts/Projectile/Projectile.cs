using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Inspector variables
    [SerializeField] [Header("Required")] private Transform projectileSprite;
    [SerializeField] private GameObject explosionZone;
    [SerializeField] [Header("Configuration")] private LayerMask collisionLayers;
    //Non inspector variables
    private Rigidbody2D thisRB;
    private GameObject playerWhoFiredMe;

    private void Awake()
    {
        //Get this rb
        thisRB = GetComponent<Rigidbody2D>();
        //Unparent the projectile sprite as we dont want it to be affected by this rigidbody rotation
        projectileSprite.SetParent(null);
    }

    private void FixedUpdate()
    {
        //Rotate the sprite indepedently of the rigidbody to look natural
        ProjectileVisualRotation();
        //Use a raycast for collision detection
        CollisionDetection();
    }

    private void ProjectileVisualRotation()
    {
        //Make sure the position is the same as the projectile
        projectileSprite.position = transform.position;
        //Rotate this sprite based on the velocity indepently
        projectileSprite.up = thisRB.velocity.normalized;
    }

    private void CollisionDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, thisRB.velocity, 0.6f, collisionLayers);
        if (hit.collider != null)
        {
            //Unparent the explosion zone and set it as active
            explosionZone.transform.SetParent(null);
            explosionZone.SetActive(true);
            explosionZone.GetComponent<Explosion>().SetWhoFiredMe(playerWhoFiredMe);
            //Destroy this project
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        //Ensure the visuals for this object are destroyed
        if (projectileSprite != null)
        {
            //Make sure the projectile sprite has also been destroyed
            Destroy(projectileSprite.gameObject);
        }
    }

    public void SetWhoFiredMe(GameObject player) //Set the player who shot me
    {
        playerWhoFiredMe = player;
    }
}
