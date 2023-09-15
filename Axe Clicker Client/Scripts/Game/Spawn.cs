using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnables = new List<GameObject>();
    [SerializeField] Vector3 defaultPos;
    Rigidbody rb;
    public bool gameRunning { get; set; }
    int elementNumber;
    float sideForce;
    float spin;

    // Start is called before the first frame update
    void Start()
    {
        gameRunning = true;
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (true)
        {
            if (gameRunning)
            {
                yield return new WaitForSeconds(1);
                elementNumber = Random.Range(0, 5);
                TossAxe(elementNumber);
            }
        }
    }

    void TossAxe(int number)
    {
        GameObject currentAxe = AxePool.axePool.Dequeue();
        currentAxe.transform.position = spawnables[number].transform.position;
        currentAxe.SetActive(true);
        rb = currentAxe.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        switch (number)
        {
            case 0:
                sideForce = Random.Range(0, 100);
                break;
            case 1:
                sideForce = Random.Range(-50, 50);
                break;
            case 2:
                sideForce = Random.Range(-100, 100);
                break;
            case 3:
                sideForce = Random.Range(-100, 100);
                break;
            case 4:
                sideForce = Random.Range(-50, 50);
                break;
            case 5:
                sideForce = Random.Range(-100, 0);
                break;
        }

        if (sideForce < 0)
        {
            spin = 5000;
            currentAxe.transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
        }
        else
        {
            spin = -5000;
            currentAxe.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }
        rb.AddForce(new Vector3(sideForce, 300, 0));
        rb.AddTorque(Vector3.forward * spin);
    }
}
