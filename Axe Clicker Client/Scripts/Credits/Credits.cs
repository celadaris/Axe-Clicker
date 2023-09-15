using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] List<Rigidbody2D> credits;
    [SerializeField] float speed;

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            speed *= 10;
        }

        if (Input.GetMouseButtonUp(0))
        {
            speed /= 10;
        }
    }

    private void FixedUpdate()
    {
        credits.ForEach(x => 
        {
            x.transform.Translate(Vector3.up * speed);
            x.GetComponent<Rigidbody2D>().MovePosition(Vector3.up * speed);
        });
    }
}
