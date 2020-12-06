using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private bool collisionRun = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Only allow this collision code to be run once
        if (!collisionRun)
        {
            //Mark this collision as run
            collisionRun = true;
            //If hit by a projectile owned by the other player
            if (collision.GetComponent<Explosion>().GetWhoFiredMe() != this.gameObject)
            {
                Invoke("KillPlayer", 0.7f);
            }
            else { collisionRun = false; } //If friendly fire, do not end the game
        }
    }

    private void KillPlayer()
    {
        TurnManager.singleton.EndGame(TurnManager.singleton.GetCurrentTurn());
    }
}
