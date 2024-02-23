using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Display : MonoBehaviour
{
    [FormerlySerializedAs("Cell")] [SerializeField] private GameObject cell;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(cell);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
