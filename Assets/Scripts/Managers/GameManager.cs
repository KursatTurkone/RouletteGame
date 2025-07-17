using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RouletteAnimator rouletteAnimator;
    private IRouletteAnimator animator;
    private int currentNumber;
    private int SelectedBetNumber;
    public BetManager betManager;
    private void Awake()
    {
        animator = rouletteAnimator;
    }


    private void RandomNumber()
    {
        int[] numberOrder = rouletteAnimator.NumberOrder;
        int index = Random.Range(0, numberOrder.Length);
        currentNumber = numberOrder[index];
    }

    private void OnSpinCompleted(int resultNumber)
    {
        betManager.EvaluateBets(resultNumber);
        Debug.Log($"Spin ended, the number: {resultNumber}");
    }

    public void OnSpinButtonPressed()
    {
        currentNumber = SelectedBetNumber == -1
            ? rouletteAnimator.NumberOrder[Random.Range(0, rouletteAnimator.NumberOrder.Length)]
            : SelectedBetNumber;
        animator.AnimateSpinToNumber(currentNumber, OnSpinCompleted);
    }

    public void SetCurrentSpinNumber(int number)
    {
        SelectedBetNumber = number;
    }
}