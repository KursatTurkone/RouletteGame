using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class RouletteTableRaycaster : MonoBehaviour
{
    [SerializeField] private Transform startPosTransform;
    [SerializeField] int rows = 12;
    [SerializeField] int cols = 3;
    [SerializeField] float cellWidth = 1f;
    [SerializeField] float cellHeight = 1f;
    [SerializeField] private LayerMask tableLayer;
    [SerializeField] private BetManager betManager;

    private List<IBetDetector> _detectors;

    void Awake()
    {
        SetupDetectors();
    }

    void SetupDetectors()
    {
        _detectors = new List<IBetDetector>
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

        foreach (var detector in _detectors)
        {
            if (detector.TryDetect(xPercent, zPercent, row, col, rows, cols, out int[] numbers))
            {
                if (numbers.Length == 1)
                {
                    betManager.PlaceNumberBet(numbers[0], betManager.CurrentBetAmount);
                }
                else
                {
                    var payout = RoulettePayouts.GetPayoutForNumberCount(numbers.Length);
                    betManager.PlaceGroupBet(numbers, betManager.CurrentBetAmount, payout);
                }
                return;
            }
        }
    }

    public Vector3 GetCellCenter(int number)
    {
        int row = (number - 1) / cols;
        int col = (number - 1) % cols;
        return startPosTransform.position
               + Vector3.forward * (col * cellWidth + cellWidth / 2f)
               + Vector3.right * (row * cellHeight + cellHeight / 2f);
    }

    public Vector3 GetCellsCenter(int[] numbers)
    {
        Vector3 sum = Vector3.zero;
        foreach (var num in numbers)
            sum += GetCellCenter(num);
        return sum / numbers.Length;
    }
}