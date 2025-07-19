using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("UI References")]
   [SerializeField] private Button spinButton;
   [SerializeField] private Button increaseBetButton;
   [SerializeField] private Button decreaseBetButton;
   [SerializeField] private TextMeshProUGUI betAmountText;
   [SerializeField] private TMP_Dropdown spinNumberDropdown;
   [SerializeField] private GameObject winScreen;
   [SerializeField] private TextMeshProUGUI winText;
   [SerializeField] private GameObject loseScreen;
   [SerializeField] private TextMeshProUGUI loseText;
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
    public void ShowWinNotification(int winAmount)
    {
        winText.SetText($"You Win: {winAmount}");
        winScreen.SetActive(true);
        MyTween.ScaleTo(this, winScreen.transform, Vector3.one, 0.5f, DisableWinScreen, EaseType.OutCubic);
    }

    private void DisableWinScreen()
    {
        MyTween.Delay(this, 1f, () =>
        {
            winScreen.SetActive(false);
            winScreen.transform.localScale = Vector3.zero;
        });
    }

    private void DisableLoseScreen()
    {
        MyTween.Delay(this, 1f, () =>
        {
            loseScreen.SetActive(false);
            loseScreen.transform.localScale = Vector3.zero;
        });
    }

    public void ShowLoseNotification(int loseAmount)
    {
        loseText.SetText($"You Lose: {loseAmount}");
        loseScreen.SetActive(true);
        MyTween.ScaleTo(this, loseScreen.transform, Vector3.one, 0.5f, DisableLoseScreen, EaseType.OutCubic);
    }
}
