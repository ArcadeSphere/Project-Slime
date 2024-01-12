using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToEdge : MonoBehaviour
{
    [SerializeField] private Vector2 previousPositionOffset = new Vector2(2, 0);
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Thorn")){
            if (PlayerData.Instance.isFacingRight)
                this.transform.position = PlayerData.Instance.previousPosition - previousPositionOffset;
            else
                this.transform.position = PlayerData.Instance.previousPosition + previousPositionOffset;
        }
    }
}
