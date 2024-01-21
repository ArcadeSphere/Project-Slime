using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venus : MonoBehaviour
{
    private enum venusState {idle, closeMouth, openMouth};
    private Animator anim;
    private venusState currentState = 0;

    private void Start() {
        anim = this.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        currentState = venusState.closeMouth;
        if (collision.CompareTag("Player")) {
            anim.SetInteger("state", (int)currentState);
        }
    }

    void changeStateToOpenMouth() {
        currentState = venusState.openMouth;
        anim.SetInteger("state", (int)currentState);
    }
}
