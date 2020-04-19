using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float patrolDist = 2f;
    float sqrPatrolDist;
    public Vector3 direction = Vector3.left;
    public float speed = 1f;

    SpriteRenderer sr;
    Vector3 startPos;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startPos = transform.position;
        sqrPatrolDist = Mathf.Pow(patrolDist, 2);
        direction.Normalize();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Vector3.Dot(direction, Vector3.left) < 0f)
        {
            sr.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        if (Vector3.SqrMagnitude(transform.position - startPos) > sqrPatrolDist)
        {
            direction *= -1f;
            startPos = transform.position;
            sr.flipX = !sr.flipX;
        }
            
    }
}
