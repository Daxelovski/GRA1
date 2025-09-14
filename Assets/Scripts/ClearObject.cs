using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearObject : MonoBehaviour
{
    [SerializeField] private float lifeTime, timer;
    public int damage;

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= lifeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
