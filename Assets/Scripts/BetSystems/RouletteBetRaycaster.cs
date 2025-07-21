using UnityEngine;
using System.Collections.Generic;

public class RouletteBetRaycaster : MonoBehaviour
{
    [SerializeField] private BetManager betManager;
    [SerializeField] private Transform startPosTransform;
    [SerializeField] private int rows = 12;
    [SerializeField] private int cols = 3;
    [SerializeField] private float cellWidth = 1f;
    [SerializeField] private float cellHeight = 1f;
    [SerializeField] private LayerMask tableLayer;

    private List<IBetDetector> _detectors;

    void Awake()
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

        // first check if the hit object is a BetBox
        if (hit.collider.TryGetComponent(out BetBox betBox))
        {
            int amount = betManager.CurrentBetAmount;
            betManager.PlaceSpecialBet(betBox.betType, amount, betBox.transform);
            return;
        }

        // Is Grid Space?
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
                int amount = betManager.CurrentBetAmount;
                if (numbers.Length == 1)
                    betManager.PlaceNumberBet(numbers[0], amount);
                else
                {
                    var payout = RoulettePayouts.GetPayoutForNumberCount(numbers.Length);
                    betManager.PlaceGroupBet(numbers, amount, payout);
                }

                return;
            }
        }
    }

// Returns the center position of a cell in the grid based on its number (1-indexed)
    public Vector3 GetCellCenter(int number)
    {
        int row = (number - 1) / cols;
        int col = (number - 1) % cols;
        return startPosTransform.position
               + Vector3.forward * (col * cellWidth + cellWidth / 2f)
               + Vector3.right * (row * cellHeight + cellHeight / 2f);
    }

// Returns the center position of multiple cells in the grid based on their numbers (1-indexed)
    public Vector3 GetCellsCenter(int[] numbers)
    {
        Vector3 sum = Vector3.zero;
        foreach (var num in numbers)
            sum += GetCellCenter(num);
        return sum / numbers.Length;
    }
}