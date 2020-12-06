using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Configuration")] [SerializeField] private float timeUntilDestroy = 1f;
    private GameObject owner;
   
    private void Awake()
    {
        //Start the destroy timer
        Invoke("DestroySelf", timeUntilDestroy); 
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public void SetWhoFiredMe(GameObject player)
    {
        owner = player;
    }

    public GameObject GetWhoFiredMe()
    {
        return owner;
    }
}
