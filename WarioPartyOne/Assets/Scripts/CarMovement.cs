using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        speed = Random.Range(1.5f, 3.0f);
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.up * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NoCarZone"))
        {
            Destroy(gameObject);
        }
    }
}
