using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum direction { left, right }
    //Inspector variables
    [SerializeField] [Header("Requisites")] private Transform barrel;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private string inputAxis = "Mouse Y";
    [SerializeField] [Header("Configuration")] [Range(0, 2.5f)] private float projectileStrength = 1.5f;
    [SerializeField] private float afterFiringCooldown = 3;
    [SerializeField] [Range(0.1f, 10f)] private float aimSensitivity = 1;
    [SerializeField] [Range(0, 180)] int clampAmount = 90;
    [SerializeField] private KeyCode shootButton = KeyCode.Space;
    [SerializeField] [Header("Runtime")] private direction aimDirection = direction.right;
    //Non-Inspector variables
    private Vector3 barrelRotation;
    private bool canShoot = true;
    private SpriteRenderer muzzleSprite;
    private Animator muzzleAnimator;

    private void Awake() //Get the muzzle animator
    {
        muzzleAnimator = GetComponentInChildren<Animator>();
        muzzleSprite = muzzleAnimator.gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (canShoot)
        {
            //Aim the barrel
            AimBarrel();
            //Shoot on button press
            if (Input.GetKeyUp(shootButton)) { Shoot(); }
        }
    }

    private void AimBarrel() //This method will actually move the gun barrel to the desired location
    {
        //Decide what side of the map this is on
        if (this.gameObject.name == "Player 1") { aimDirection = direction.right; }
        else { aimDirection = direction.left; }
        //Calculate the barrel rotation without appplying it
        Vector3 mouseInput = new Vector3(0, 0, Input.GetAxis(inputAxis) * aimSensitivity);
        if (aimDirection == direction.left) { mouseInput = -mouseInput; } //If facing left, invert the look vector
        barrelRotation += mouseInput;
        //Clamp the barrel rotation to ensure it stays within the desired angles
        float minClamp = 0;
        float maxClamp = clampAmount;
        if (aimDirection == direction.right) //Inverse the clamp
        {
            minClamp = -clampAmount;
            maxClamp = 0;
        }
        barrelRotation = new Vector3(0, 0, Mathf.Clamp(barrelRotation.z, minClamp, maxClamp));
        //Apply the clamped barrel rotation
        barrel.eulerAngles = barrelRotation;
    }

    private void Shoot() //This method will shoot
    {
        //Do not allow another shot
        canShoot = false;
        //Show the muzzle flash
        muzzleAnimator.SetTrigger("Shoot");
        muzzleSprite.enabled = true;
        //Spawn the projectile and add some force
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, barrel.rotation) as GameObject;
        projectile.GetComponent<Rigidbody2D>().AddForce(barrel.transform.up * (projectileStrength * 10000));
        projectile.GetComponent<Projectile>().SetWhoFiredMe(this.gameObject);
        //Start the next turn timer
        StartCoroutine(NextTurn());
    }

    private IEnumerator NextTurn()
    {
        yield return new WaitForSeconds(afterFiringCooldown);
        TurnManager.singleton.NextTurn();
    }

    public void Reload() //This will allow shooting again
    {
        canShoot = true;
    }

}
