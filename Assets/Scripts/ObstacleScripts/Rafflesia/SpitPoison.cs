using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpitPoison : MonoBehaviour
{
    [SerializeField] GameObject poisonPrefab;
    private Animator poisonAnim;

    private void Start() {
        poisonAnim = poisonPrefab.GetComponent<Animator>();
    }

    void StartPoisonAnim() {
        poisonAnim.SetBool("shoot", true);
    }

    void StopPoisonAnim() {
        poisonAnim.SetBool("shoot", false);
    }

}
