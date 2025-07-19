using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("UI References")]
    public Button spinButton;
    public Button increaseBetButton;
    public Button decreaseBetButton;
    public TextMeshProUGUI betAmountText;
    public TMP_Dropdown spinNumberDropdown;
    private BetManager betManager;
    private GameManager gameManager;

    private void Awake()
    {
        betManager = FindObjectOfType<BetManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        increaseBetButton.onClick.AddListener(OnIncreaseBetClicked);
        decreaseBetButton.onClick.AddListener(OnDecreaseBetClicked);
        spinButton.onClick.AddListener(OnSpinClicked);
        SetupDropdown();
        UpdateBetAmountUI();
        spinNumberDropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void OnIncreaseBetClicked()
    {
        betManager.IncreaseBet();
        UpdateBetAmountUI();
    }

    private void OnDecreaseBetClicked()
    {
        betManager.DecreaseBet();
        UpdateBetAmountUI();
    }

    private void OnSpinClicked()
    {
        gameManager.OnSpinButtonPressed();
    }

    public void UpdateBetAmountUI()
    {
        betAmountText.SetText(betManager.CurrentBetAmount.ToString());
    }
    private void SetupDropdown()
    {
        spinNumberDropdown.ClearOptions();
        var options = new System.Collections.Generic.List<string>
        {
            "Random"
        };
        for (int i = 0; i <= 36; i++)
            options.Add(i.ToString());
        spinNumberDropdown.AddOptions(options);

        spinNumberDropdown.value = 0; 
        gameManager.SetCurrentSpinNumber(-1);
    }
    private void OnDropdownChanged(int index)
    {
        if (index == 0)
            gameManager.SetCurrentSpinNumber(-1); 
        else
            gameManager.SetCurrentSpinNumber(index-1);
    }
}
