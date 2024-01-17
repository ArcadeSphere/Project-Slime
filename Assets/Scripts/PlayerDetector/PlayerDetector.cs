using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerDetector : MonoBehaviour
{
    public bool PlayerDetected { get; set; }
    public Vector2 DirectionToPlayer => target.transform.position - detectorOrigin.position;

    public SerializedEnum.DetectorShape DetectorShape;

    // Detector Params
    [SerializeField]
    private Transform detectorOrigin;

    [SerializeField]
    private Vector2 detectorSize = Vector2.one;

    [SerializeField]
    private float detectorRadius = 0f;
    public Vector2 detectorOriginOffset = Vector2.zero;

    [SerializeField]
    private float detectionDelay = 0.3f;

    private LayerMask detectorLayerMask;

    // Gizmo params
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

    private void Awake()
    {
        detectorLayerMask = LayerMask.GetMask("Player");
        detectorOrigin = transform;
    }

    private void Start()
    {
        StartCoroutine(DetectionCoroutine());
    }

    IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionDelay);
        switch (DetectorShape)
        {
            case SerializedEnum.DetectorShape.Square:
                PerformDetectionSquare();
                break;
            case SerializedEnum.DetectorShape.Circle:
                PerformDetectionCircle();
                break;
        }
        StartCoroutine(DetectionCoroutine());
    }

    public void PerformDetectionSquare()
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

    public void PerformDetectionCircle()
    {
        Collider2D collider = Physics2D.OverlapCircle(
            (Vector2)detectorOrigin.position + detectorOriginOffset,
            detectorRadius,
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
            switch (DetectorShape)
            {
                case SerializedEnum.DetectorShape.Square:
                    Gizmos.color = gizmoIdleColor;
                    if (PlayerDetected)
                    {
                        Gizmos.color = gizmoDetectedColor;
                    }
                    Gizmos.DrawCube(
                        (Vector2)detectorOrigin.position + detectorOriginOffset,
                        detectorSize
                    );
                    break;

                case SerializedEnum.DetectorShape.Circle:
                    Gizmos.color = gizmoIdleColor;
                    if (PlayerDetected)
                    {
                        Gizmos.color = gizmoDetectedColor;
                    }
                    Gizmos.DrawWireSphere(
                        (Vector2)detectorOrigin.position + detectorOriginOffset,
                        detectorRadius
                    );
                    break;
            }
        }
    }
}

#region CustomUI
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerDetector))]
class PlayerDetectorEditor : Editor
{
    // dectector properties
    SerializedProperty detectorOrigin;
    SerializedProperty detectorOriginOffset;
    SerializedProperty detectionDelay;
    SerializedProperty detectorSize;
    SerializedProperty detectorRadius;
    SerializedProperty gizmoIdleColor;
    SerializedProperty gizmoDetectedColor;
    SerializedProperty showGizmo;

    private void OnEnable()
    {
        detectorOrigin = serializedObject.FindProperty("detectorOrigin");
        detectorOriginOffset = serializedObject.FindProperty("detectorOriginOffset");
        detectionDelay = serializedObject.FindProperty("detectionDelay");
        detectorSize = serializedObject.FindProperty("detectorSize");
        detectorRadius = serializedObject.FindProperty("detectorRadius");
        gizmoIdleColor = serializedObject.FindProperty("gizmoIdleColor");
        gizmoDetectedColor = serializedObject.FindProperty("gizmoDetectedColor");
        showGizmo = serializedObject.FindProperty("showGizmo");
    }

    public override void OnInspectorGUI()
    {
        var playerDetector = (PlayerDetector)target;
        if (playerDetector == null)
            return;
        serializedObject.Update();

        ShowDetectorShape(playerDetector);
        EditorGUILayout.Space();
        ShowPhysicsOverlapParams(playerDetector);
        EditorGUILayout.Space();
        ShowGizmoParams();
        serializedObject.ApplyModifiedProperties();
    }

    private void ShowDetectorShape(PlayerDetector playerDetector)
    {
        EditorGUILayout.LabelField("Detector", EditorStyles.boldLabel);

        playerDetector.DetectorShape = (SerializedEnum.DetectorShape)
            EditorGUILayout.EnumPopup("Detector Shape", playerDetector.DetectorShape);
        SceneView.RepaintAll();
    }

    private void ShowPhysicsOverlapParams(PlayerDetector playerDetector)
    {
        EditorGUILayout.LabelField("Physics Overlap parameters", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(detectorOrigin);
        EditorGUILayout.PropertyField(detectorOriginOffset);
        EditorGUILayout.PropertyField(detectionDelay);
        ShowSize(playerDetector);
    }

    private void ShowGizmoParams()
    {
        EditorGUILayout.LabelField("Gizmo parameters", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(gizmoIdleColor);
        EditorGUILayout.PropertyField(gizmoDetectedColor);
        EditorGUILayout.PropertyField(showGizmo);
    }

    private void ShowSize(PlayerDetector playerDetector)
    {
        if (playerDetector.DetectorShape == SerializedEnum.DetectorShape.Circle)
        {
            EditorGUILayout.PropertyField(detectorRadius);
        }
        else
        {
            EditorGUILayout.PropertyField(detectorSize);
        }
    }
}

#endif
#endregion
