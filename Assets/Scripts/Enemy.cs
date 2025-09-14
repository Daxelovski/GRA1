using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health, maxHealth;
    [SerializeField] private float attackSpeed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private bool left;
    [SerializeField] private float speed;
    [SerializeField] private float timer;
    [SerializeField] private float duration;
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private float attackTimer;
    [SerializeField] private Score score;
    [SerializeField] private GameObject crystal;
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        score = GameObject.Find("Score").GetComponent<Score>();
        gameManager = GameObject.Find("GAME MANAGER").GetComponent<GameManager>();
    }

    void Update()
    {
        Movement();

        if(bullet != null)
        {
            AttackProcedure();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Attack"))
        {
            int damage = col.GetComponent<ClearObject>().damage;
            Destroy(col.gameObject);
            health -= damage;
            if(health <= 0)
            {
                Destroy(this.gameObject);
                score.AddScore(50);
                GameObject newCrystal = Instantiate(crystal, transform.position, Quaternion.identity);
                newCrystal.transform.localScale = new Vector3(3, 3, 1);
                gameManager.Find();
                gameManager.SetEvents();
            }
        }
    }

    private void Movement()
    {
        if(left == false)
        {
            transform.position += Vector3.right * speed * Time.smoothDeltaTime;
        }
        else
        {
            transform.position += -Vector3.right * speed * Time.smoothDeltaTime;
        }

        timer += Time.smoothDeltaTime;
        if(timer >= duration)
        {
            timer = 0;
            left = !left;
            rend.flipX = !rend.flipX;
        }
    }

    private void AttackProcedure()
    {
        attackTimer += Time.smoothDeltaTime;

        if(attackTimer >= attackSpeed)
        {
            int direction;

            if (rend.flipX == true)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }

            GameObject fireball = Instantiate(this.bullet, transform.position, Quaternion.identity);
            fireball.GetComponent<Rigidbody2D>().AddForce(Vector3.right * 500f * direction);

            if(direction == -1)
            {
                fireball.transform.Rotate(0f, 0f, -90f);
            }
            else
            {
                fireball.transform.Rotate(0f, 0f, 90f);
            }

            attackTimer = 0;
        }
        
    }
}
