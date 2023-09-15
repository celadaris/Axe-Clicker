using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxePool : MonoBehaviour
{
    [SerializeField] List<GameObject> axePrefab;
    public static Queue<GameObject> axePool { get; set; }

    private void Start()
    {
        axePool = new Queue<GameObject>();
        for(int i = 0; i < 20; i++)
        {
            axePool.Enqueue(axePrefab[i]);
        }
        
    }

    private void OnDestroy()
    {
        axePool.Clear();
    }
}