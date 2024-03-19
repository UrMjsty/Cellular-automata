//using System.Collections;

using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private GameObject _cell;
    [SerializeField]private int size = 100;
    [SerializeField]private float cellSize = .1f;
    [SerializeField]private int iterations = 100;
    private List<int> _nextGeneration = new List<int>();
    private List<int> _nextGenerationTemplate = new List<int>();
    private List<int> _generation = new List<int>();
    [SerializeField] private GameObject cell;
    void Start()
    {
        for (int i = 0; i < size; i++)
        {
            _nextGenerationTemplate.Add(0);
        }
        cell.transform.localScale = new Vector3(cellSize, cellSize);
        _nextGeneration = new List<int>(_nextGenerationTemplate);
        CreateTriangle(ref _generation);
            for (int i = 0; i < iterations; i++)
            {
                Generate(ref _generation, ref _nextGeneration);
                _generation = new List<int>(_nextGeneration);
                _nextGeneration = new List<int>(_nextGenerationTemplate);
                for (int j = 0; j < size; j++)
                {
                    if (_generation[j] == 1)
                        Instantiate(cell, (new Vector3(- size + 8 + j,  - i) * cellSize),quaternion.identity, transform);
                }
                //  Debug.Log(String.Join(" ", _generation));
            }
    }
    
    void Generate(ref List<int> gen,ref List<int> nextgen)
    {
        gen.Insert(0, 0);
        gen.Add(0);
        var parents = "";
        for (int i = 0; i < size; i++)
        {
            parents += (gen[i]).ToString() + (gen[i + 1]) + (gen[i + 2]);
            //Debug.Log($"parents ={parents}");
            if (parents is "111" or "100" or "000")
                nextgen[i] = 0;
            else
                nextgen[i] = 1;
            parents = "";
        }
    }

    void CreateTriangle(ref List<int> gen)
    {
        for (int i = 0; i < size - 1; i++)
        {
            gen.Add(0);
        }
        gen.Add(1);
    }
}