using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerStats player;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Crystal"))
        {
            Destroy(col.gameObject);
        }
        else if(col.CompareTag("EnemyAttack"))
        {
            player.UpdateHealth(-15);
            Destroy(col.gameObject);
        }
        else if(col.CompareTag("Portal"))
        {
            gameManager.nearPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.CompareTag("Portal"))
        {
            gameManager.nearPortal = false;
        }
    }
}
