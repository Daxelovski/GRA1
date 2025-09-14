using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour 
{
    [SerializeField] private float timer;
    [SerializeField] private float cooldown;
    [SerializeField] private bool onCoolDown;
    [SerializeField] private GameObject fireball;
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private PlayerStats player;
    [SerializeField] private AudioSystem audioSystem;

    private void Start() 
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyUp("f"))
        {
            if(onCoolDown == false)
            {
                if(player.mana >= 10f)
                PlayerAttack();
            } 
        }

        Cooldown();
    }

    private void Cooldown()
    {
        if(onCoolDown == true)
        {
            timer += Time.deltaTime;
            if(timer >= cooldown)
            {
                onCoolDown = false;
                timer = 0;
            }
        }
    }

    private void PlayerAttack()
    {
        player.mana -= 10f;
        int direction;

        if (rend.flipX == true) 
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
            
        GameObject fireball = Instantiate(this.fireball, transform.position, Quaternion.identity);
        fireball.GetComponent<Rigidbody2D>().AddForce(Vector3.right * 500f * direction); 
        fireball.GetComponent<ClearObject>().damage = player.damage;

        audioSystem.PlayAudio(0, 1f);

        if(direction == -1)
        {
            fireball.transform.Rotate(0f, 0f, -90f);
        }
        else
        {
            fireball.transform.Rotate(0f, 0f, 90f);
        }

        onCoolDown = true;
    }
}
