using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour, IGameUIManager
{
    [Header("Spin & Bet Controls")] [SerializeField]
    private Button spinButton;

    [SerializeField] private Button increaseBetButton;
    [SerializeField] private Button decreaseBetButton;
    [SerializeField] private Button getMoneyButton;
    [SerializeField] private TMP_Dropdown spinNumberDropdown;
    [SerializeField] private TextMeshProUGUI betAmountText;
    [SerializeField] private TextMeshProUGUI chipsText;

    [Header("Result Screens")] [SerializeField]
    private GameObject winScreen;

    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TextMeshProUGUI loseText;


    [Header("Winning Chip Display")] [SerializeField]
    private Image winningChipColorImage;
    [SerializeField] private TextMeshProUGUI winningChipText;
    
    private BetManager _betManager;
    private GameManager _gameManager;

    private void Awake()
    {
        _betManager = FindObjectOfType<BetManager>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        increaseBetButton.onClick.AddListener(OnIncreaseBetClicked);
        decreaseBetButton.onClick.AddListener(OnDecreaseBetClicked);
        getMoneyButton.onClick.AddListener(() => _betManager.AdjustChips(10000));
        spinButton.onClick.AddListener(OnSpinClicked);
        SetupDropdown();
        UpdateBetAmountUI();
        spinNumberDropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void OnIncreaseBetClicked()
    {
        _betManager.IncreaseBet();
        UpdateBetAmountUI();
    }

    private void OnDecreaseBetClicked()
    {
        _betManager.DecreaseBet();
        UpdateBetAmountUI();
    }

    private void OnSpinClicked()
    {
        _gameManager.OnSpinButtonPressed();
    }

    public void UpdateBetAmountUI()
    {
        betAmountText.SetText(_betManager.CurrentBetAmount.ToString());
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
        _gameManager.SetCurrentSpinNumber(-1);
    }

    private void OnDropdownChanged(int index)
    {
        if (index == 0)
            _gameManager.SetCurrentSpinNumber(-1);
        else
            _gameManager.SetCurrentSpinNumber(index - 1);
    }

    public void ShowResultNotification(bool isWin, int amount, int number, Color color)
    {
        GameObject screen = isWin ? winScreen : loseScreen;
        TextMeshProUGUI text = isWin ? winText : loseText;
        string prefix = isWin ? "You Win" : "You Lose";
        text.SetText($"{prefix}: {amount}");
        screen.SetActive(true);
        MyTween.ScaleTo(this, screen.transform, Vector3.one, 0.5f,
            () =>
            {
                winningChipColorImage.gameObject.SetActive(true);
                winningChipText.gameObject.SetActive(true);
                winningChipColorImage.color = color;
                winningChipText.SetText(number.ToString());
                DisableResultScreen(isWin);
            }, EaseType.OutCubic);
    }

    private void DisableResultScreen(bool isWin)
    {
        GameObject screen = isWin ? winScreen : loseScreen;
        MyTween.Delay(this, 2f, () =>
        {
            winningChipColorImage.gameObject.SetActive(false);
            winningChipText.gameObject.SetActive(false);
            screen.SetActive(false);
            screen.transform.localScale = Vector3.zero;
        });
    }

    public void UpdateChipsText(int playerChips)
    {
        chipsText.text = playerChips.ToString("N0");
    }
}