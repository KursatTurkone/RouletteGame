using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class RouletteTableRaycaster : MonoBehaviour
{
    public Transform startPosTransform;
    [SerializeField] int rows = 12;
    [SerializeField] int cols = 3;
    [SerializeField] float cellWidth = 1f;
    [SerializeField] float cellHeight = 1f;
    public LayerMask tableLayer;

    private List<IBetDetector> detectors;

    void Awake()
    {
        SetupDetectors();
    }

    void SetupDetectors()
    {
        detectors = new List<IBetDetector>
        {
            new CornerBetDetector(0.18f),
            new SplitBetDetector(0.22f),
            new StraightBetDetector(0.22f)
        };
    }

    void Update()
    {
        if (!Application.isPlaying) return;
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 10000f, tableLayer)) return;
        if (startPosTransform == null) return;

        Vector3 local = hit.point - startPosTransform.position;
        int row = Mathf.Clamp((int)(local.x / cellHeight), 0, rows - 1);
        int col = Mathf.Clamp((int)(local.z / cellWidth), 0, cols - 1);

        float xPercent = (local.x % cellHeight) / cellHeight;
        float zPercent = (local.z % cellWidth) / cellWidth;

        foreach (var detector in detectors)
        {
            if (detector.TryDetect(xPercent, zPercent, row, col, rows, cols, out int[] numbers))
            {
                Debug.Log(string.Join("-", numbers));
                return;
            }
        }
    }
}