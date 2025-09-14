using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private float currentSpeed, speed, maxSpeed, timer;
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private int current;

    [SerializeField] private int keyCount;
    [SerializeField] private bool canJump;
    [SerializeField] private float timerJump;

    void Start()
    {
        canJump = true;
    }

    void Update()
    {
        PlayerMovement();
        Sprinting();
        Rotation();
        Jump();
        Visualization();
        CountJumps();
    }

    private void PlayerMovement()
    {
        velocity.Set(Input.GetAxis("Horizontal") * currentSpeed, rig.velocity.y);
        rig.velocity = velocity;
    }

    private void Sprinting()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = maxSpeed;
        }
        else
        {
            currentSpeed = speed;
        }
    }

    private void Rotation()
    {
        if(Input.GetAxis("Horizontal") < 0)
        {
            rend.flipX = true;
        }
        else if(Input.GetAxis("Horizontal") > 0)
        {
            rend.flipX = false;
        }
    }

    private void Jump()
    {
        if(Input.GetKeyDown("space"))
        {
            if(canJump == true)
            rig.velocity += Vector2.up * 4.5f;
        }         
    }

    private void Visualization()
    {
        timer += Time.deltaTime;
        if(timer > 0.25f)
        {
            current ++;

            if(current >= sprites.Length)
            {
                current = 0;
            }

            rend.sprite = sprites[current];
            timer = 0;
        }
    }

    private void CountJumps()
    {
        if(canJump == true)
        {
        if(Input.GetKeyDown("space"))
        {
            keyCount++;

            if(keyCount >= 2)
            {
                canJump = false;
                timer = 0;
                StartCoroutine(Wait());
            }
        }

        timerJump += Time.deltaTime;
        if(timerJump >= 0.75f)
        {
            if(keyCount > 0)
            keyCount --;

            timerJump = 0;
        }
        }
    }

    private IEnumerator Wait()
    {
        canJump = false;
        yield return new WaitForSeconds(1f);
        canJump = true;
        timerJump = 0;
        keyCount = 0;
    }
}
