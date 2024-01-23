using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newPlayerCore", menuName ="Core/Player Data/Base Data")]
public class PlayerCore : ScriptableObject
{
    [Header("Movement State")]
    public float MovementSpeed = 10f;
}
