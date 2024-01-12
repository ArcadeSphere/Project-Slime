using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestoreCheckpoint : MonoBehaviour
{
    void Start()
    {
        if (PlayerData.Instance.currentCheckpoint != null)
            transform.position = PlayerData.Instance.currentCheckpoint.position + new Vector3(0, 2, 0);
    }
}
