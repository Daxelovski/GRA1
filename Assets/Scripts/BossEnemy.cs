using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 260f;
    [SerializeField] private float health;
    [SerializeField] private float shootCooldown = 1.35f;
    [SerializeField] private float shootTimer;
    [SerializeField] private float moveHeight = 1.2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float projectileScale = 2.2f;
    [SerializeField] private int rewardCrystals = 60;
    [SerializeField] private int rewardScore = 300;

    private Transform player;
    private Vector3 startPosition;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Sprite projectileSprite;
    [SerializeField] private GameObject projectilePrefab;

    public void Init(GameManager manager, Sprite sprite, GameObject prefab)
    {
        gameManager = manager;
        projectileSprite = sprite;
        projectilePrefab = prefab;
    }

    void Start()
    {
        health = maxHealth;
        startPosition = transform.position;

        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if(playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        Move();
        Shoot();
    }

    private void Move()
    {
        float y = Mathf.Sin(Time.time * moveSpeed) * moveHeight;
        transform.position = startPosition + Vector3.up * y;
    }

    private void Shoot()
    {
        if(player == null)
        {
            return;
        }

        shootTimer += Time.deltaTime;

        if(shootTimer < shootCooldown)
        {
            return;
        }

        shootTimer = 0f;
        Vector2 direction = (player.position - transform.position).normalized;
        CreateProjectile(direction);
    }

    private void CreateProjectile(Vector2 direction)
    {
        GameObject projectile;

        if(projectilePrefab != null)
        {
            projectile = Instantiate(projectilePrefab, transform.position, GetProjectileRotation(direction));
            projectile.name = "Boss Projectile";
            projectile.transform.localScale = new Vector3(projectileScale, projectileScale, 1f);
        }
        else
        {
            projectile = new GameObject("Boss Projectile");
            projectile.transform.position = transform.position;
            projectile.transform.rotation = GetProjectileRotation(direction);
            projectile.transform.localScale = new Vector3(projectileScale, projectileScale, 1f);

            SpriteRenderer fallbackRenderer = projectile.AddComponent<SpriteRenderer>();
            fallbackRenderer.sprite = projectileSprite;
            fallbackRenderer.color = new Color(1f, 0.35f, 0.15f, 1f);
            fallbackRenderer.sortingOrder = 8;
        }

        SpriteRenderer renderer = projectile.GetComponent<SpriteRenderer>();
        if(renderer != null)
        {
            renderer.sortingOrder = 8;
        }

        Collider2D collider = projectile.GetComponent<Collider2D>();
        if(collider == null)
        {
            collider = projectile.AddComponent<CircleCollider2D>();
        }

        collider.isTrigger = true;

        LevelHazard hazard = projectile.GetComponent<LevelHazard>();
        if(hazard == null)
        {
            hazard = projectile.AddComponent<LevelHazard>();
        }

        hazard.Configure(18f, true);

        Rigidbody2D body = projectile.GetComponent<Rigidbody2D>();
        if(body != null)
        {
            body.gravityScale = 0f;
            body.velocity = direction * 5f;
        }
        else
        {
            LevelProjectile levelProjectile = projectile.GetComponent<LevelProjectile>();
            if(levelProjectile == null)
            {
                levelProjectile = projectile.AddComponent<LevelProjectile>();
            }

            levelProjectile.Launch(direction * 5f, 4f);
        }
    }

    private Quaternion GetProjectileRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, angle + 90f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.CompareTag("Attack"))
        {
            return;
        }

        ClearObject attack = col.GetComponent<ClearObject>();
        health -= attack != null ? attack.damage : 10;
        Destroy(col.gameObject);

        if(health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Score score = FindObjectOfType<Score>();
        MoneySystem money = FindObjectOfType<MoneySystem>();

        if(score != null)
        {
            score.AddScore(rewardScore);
            score.Save();
        }

        if(money != null)
        {
            money.AddCrystals(rewardCrystals);
            money.Save();
        }

        if(gameManager != null)
        {
            gameManager.BossDefeated();
        }

        Destroy(gameObject);
    }
}
