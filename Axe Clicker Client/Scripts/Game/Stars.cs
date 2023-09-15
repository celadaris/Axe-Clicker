using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] ParticleSystem stars;

    // Start is called before the first frame update
    void Start()
    {
        stars.Simulate(0.5f);
    }
}
