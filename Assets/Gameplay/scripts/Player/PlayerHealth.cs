using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float maximumHealth = 3f;
    [SerializeField]
    private RespawnHandler respawnHandler;
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maximumHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        respawnHandler.RespawnPlayer(this.gameObject);
    }
}
