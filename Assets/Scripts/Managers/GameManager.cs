using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RouletteAnimator rouletteAnimator; 
    private IRouletteAnimator animator;

    private void Awake()
    {
        animator = rouletteAnimator;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int randomNumber = RandomNumber();
            Debug.Log($"[TEST] Space basıldı, Spin başlatılıyor: {randomNumber}");
            animator.AnimateSpinToNumber(randomNumber, OnSpinCompleted);
        }
    }

    private int RandomNumber()
    {
        // European: 0-36
        int[] numberOrder = rouletteAnimator.NumberOrder;
        int index = Random.Range(0, numberOrder.Length);
        return numberOrder[index];
    }

    private void OnSpinCompleted(int resultNumber)
    {
        Debug.Log($"Spin ended, the number: {resultNumber}");
    }
}