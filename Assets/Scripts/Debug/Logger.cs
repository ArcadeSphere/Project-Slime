using System;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static bool showLogs = true;

    public static void Log(String message)
    {
        if (showLogs)
            Debug.Log(message);
    }
}
