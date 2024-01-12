using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public bool PlayerDetected { get; set; }
    public Vector2 DirectionToPlayer => target.transform.position - detectorOrigin.position;

    [Header("OverlapBox parameters")]
    [SerializeField]
    private Transform detectorOrigin;

    [SerializeField]
    private Vector2 detectorSize = Vector2.one;
    public Vector2 detectorOriginOffset = Vector2.zero;

    [SerializeField]
    private float detectionDelay = 0.3f;

    [SerializeField]
    private LayerMask detectorLayerMask;

    [Header("Gizmo parameters")]
    [SerializeField]
    private Color gizmoIdleColor = new Color(0f, 1f, 0f, 0.4f);

    [SerializeField]
    private Color gizmoDetectedColor = new Color(1f, 0f, 0f, 0.4f);

    [SerializeField]
    private bool showGizmo = true;

    private GameObject target;

    public GameObject Target
    {
        get => target;
        private set
        {
            target = value;
            PlayerDetected = target != null;
        }
    }

    private void Start()
    {
        StartCoroutine(DetectionCoroutine());
    }

    IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionDelay);
        PerformDetection();
        StartCoroutine(DetectionCoroutine());
    }

    public void PerformDetection()
    {
        Collider2D collider = Physics2D.OverlapBox(
            (Vector2)detectorOrigin.position + detectorOriginOffset,
            detectorSize,
            0,
            detectorLayerMask
        );
        if (collider != null)
        {
            Target = collider.gameObject;
        }
        else
        {
            Target = null;
        }
    }

    public void FlipDetector()
    {
        detectorOriginOffset.x *= -1;
    }

    private void OnDrawGizmos()
    {
        if (showGizmo && detectorOrigin != null)
        {
            Gizmos.color = gizmoIdleColor;
            if (PlayerDetected)
            {
                Gizmos.color = gizmoDetectedColor;
            }
            Gizmos.DrawCube((Vector2)detectorOrigin.position + detectorOriginOffset, detectorSize);
        }
    }
}
