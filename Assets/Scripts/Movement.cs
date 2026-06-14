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
    [SerializeField] private float jumpPower = 4.5f;
    [SerializeField] private int maxJumpPresses = 2;

    private float baseSpeed;
    private float baseMaxSpeed;
    private bool baseMovementCaptured;

    void Awake()
    {
        CaptureBaseMovement();
    }

    void Start()
    {
        RefreshUpgrades();
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
        if(rig == null)
        {
            return;
        }

        velocity.Set(InputBindings.GetHorizontalRaw() * currentSpeed, rig.velocity.y);
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
        float horizontal = InputBindings.GetHorizontalRaw();

        if(horizontal < 0)
        {
            rend.flipX = true;
        }
        else if(horizontal > 0)
        {
            rend.flipX = false;
        }
    }

    private void Jump()
    {
        if(InputBindings.GetKeyDown(GameInputAction.Jump))
        {
            if(canJump == true)
            rig.velocity += Vector2.up * jumpPower;
        }         
    }

    private void Visualization()
    {
        timer += Time.deltaTime;
        if(timer > 0.25f)
        {
            if(sprites == null || sprites.Length == 0 || rend == null)
            {
                timer = 0;
                return;
            }

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
        if(InputBindings.GetKeyDown(GameInputAction.Jump))
        {
            keyCount++;

            if(keyCount >= maxJumpPresses)
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

    public void RefreshUpgrades()
    {
        CaptureBaseMovement();
        speed = baseSpeed + GameSave.GetSpeedBonus();
        maxSpeed = baseMaxSpeed + GameSave.GetSprintBonus();
    }

    private void CaptureBaseMovement()
    {
        if(baseMovementCaptured)
        {
            return;
        }

        baseSpeed = speed;
        baseMaxSpeed = maxSpeed;
        baseMovementCaptured = true;
    }
}
