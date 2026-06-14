using UnityEngine;

public class LevelHazard : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private bool destroyOnHit;

    public void Configure(float newDamage, bool shouldDestroyOnHit)
    {
        damage = newDamage;
        destroyOnHit = shouldDestroyOnHit;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats player = other.GetComponent<PlayerStats>();

        if(player == null)
        {
            player = other.GetComponentInParent<PlayerStats>();
        }

        if(player == null && other.CompareTag("Player"))
        {
            player = FindObjectOfType<PlayerStats>();
        }

        if(player == null)
        {
            return;
        }

        player.UpdateHealth(-damage);

        if(destroyOnHit)
        {
            Destroy(gameObject);
        }
    }
}
