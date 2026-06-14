using UnityEngine;

public class LevelProjectile : MonoBehaviour
{
    [SerializeField] private Vector2 velocity;
    [SerializeField] private float lifeTime = 4f;
    [SerializeField] private float timer;

    public void Launch(Vector2 newVelocity, float newLifeTime)
    {
        velocity = newVelocity;
        lifeTime = newLifeTime;
    }

    void Update()
    {
        transform.position += (Vector3)(velocity * Time.deltaTime);
        timer += Time.deltaTime;

        if(timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
