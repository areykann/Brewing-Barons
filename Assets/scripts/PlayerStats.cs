using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Karakter Verileri")]
    public int totalScore = 0;
    public int totalMoney = 0;
    public int inventoryBeerCount = 0;

    [Header("Durumlar")]
    public float energy = 100f;

    [Header("UI Panelleri")]
    public GameObject inventoryPanel; // I tuþu ile açýlacak panel
    public GameObject statsPanel;     // C tuþu ile açýlacak panel

    [Header("HUD Metinleri")]
    public Text scoreText;
    public Text moneyText;
    public Text beerCountText;

    void Start()
    {
        // Oyun baþýnda panellerin kapalý olduðundan emin oluyoruz
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (statsPanel != null) statsPanel.SetActive(false);

        UpdateUI();
    }

    void Update()
    {
        // C tuþu: Stat Paneli aç/kapat
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (statsPanel != null)
            {
                bool currentState = statsPanel.activeSelf;
                statsPanel.SetActive(!currentState);
                if (!currentState) UpdateUI(); // Panel açýlýyorsa verileri güncelle
            }
        }

        // I tuþu: Envanter Paneli aç/kapat
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryPanel != null)
            {
                bool currentState = inventoryPanel.activeSelf;
                inventoryPanel.SetActive(!currentState);
                if (!currentState) UpdateUI(); // Panel açýlýyorsa verileri güncelle
            }
        }
    }

    public void AddBeer()
    {
        inventoryBeerCount++;
        UpdateUI();
        Debug.Log("<color=orange>Envantere 1 Bira Eklendi!</color>");
    }

    public void UpdateStats(int score, int money)
    {
        totalScore += score;
        totalMoney += money;
        UpdateUI();
        Debug.Log($"<color=cyan>Puan: {totalScore} | Para: {totalMoney}</color>");
    }

    public void UpdateUI()
    {
        // Panellerin içindeki yazýlarýn güncellenmesi
        if (scoreText != null) scoreText.text = "Puan: " + totalScore;
        if (moneyText != null) moneyText.text = "Para: " + totalMoney + "$";
        if (beerCountText != null) beerCountText.text = "Bira Sayýsý: " + inventoryBeerCount;
    }
}