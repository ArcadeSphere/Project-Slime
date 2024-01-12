using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPositions;
    [SerializeField] private string[] mappedKeys;
    void Update()
    {
        MoveToPosition();
    }

    void MoveToPosition()
    {
        if (Input.anyKeyDown)
        {
            string key = Input.inputString;

            if (!string.IsNullOrEmpty(key))
            {
                if (StringInArray(mappedKeys, key))
                {
                    transform.position = playerPositions[GetIndex(mappedKeys, key)].transform.position;
                }

            }
        }
    }

     bool StringInArray(string[] array, string searchString)
    {
        foreach (string item in array)
        {
            if (item == searchString)
            {
                return true;
            }
        }
        return false;
    }

    int GetIndex(string[] array, string searchString)
    {
        for (int index = 0; index < array.Length; index++)
        {
            if (array[index] == searchString)
            {
                return index;
            }
        }
        return 0;
    }
}
