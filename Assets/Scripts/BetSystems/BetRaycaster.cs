using UnityEngine;

public class BetRaycaster : MonoBehaviour
{
    public Camera uiCamera; 
    public BetManager betManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                hit.collider.TryGetComponent(out BetBox betBox); 
                if (betBox != null)
                {
                    int amount = betManager.CurrentBetAmount;
                    bool success = betManager.PlaceSpecialBet(betBox.betType, amount,betBox.transform);
                    if (success)
                    {
                        betBox.OnBetPlaced(amount);
                    }
                }
            }
        }
    }
}