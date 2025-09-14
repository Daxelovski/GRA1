using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Crystal : MonoBehaviour
{
    public event Action pickupEvent;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            pickupEvent?.Invoke();
        }
    }
}
