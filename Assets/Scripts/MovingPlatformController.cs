using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] private List<MovingPlatformTarget> platforms = new List<MovingPlatformTarget>();

    private void Awake()
    {
        foreach(MovingPlatformTarget platform in platforms)
        {
            platform.Resolve();
        }
    }

    private void Update()
    {
        foreach(MovingPlatformTarget platform in platforms)
        {
            platform.Move(Time.time);
        }
    }
}

[System.Serializable]
public class MovingPlatformTarget
{
    public string objectName;
    public Vector2 direction = Vector2.right;
    public float distance = 1.25f;
    public float speed = 1.2f;
    public float phaseOffset;

    private Transform target;
    private Vector3 startPosition;

    public void Resolve()
    {
        GameObject foundObject = GameObject.Find(objectName);

        if(foundObject == null)
        {
            Debug.LogWarning("Moving platform not found: " + objectName);
            return;
        }

        target = foundObject.transform;
        startPosition = target.position;
    }

    public void Move(float time)
    {
        if(target == null)
        {
            return;
        }

        Vector2 moveDirection = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right;
        float offset = Mathf.Sin((time + phaseOffset) * speed) * distance;
        target.position = startPosition + (Vector3)(moveDirection * offset);
    }
}
