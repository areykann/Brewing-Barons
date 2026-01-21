using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    [Header("Brew Settings")]
    public float brewTime = 3f; // Kahve hazýrlanma süresi
    private bool isBrewing = false;
    private float timer = 0f;

    [Header("UI")]
    public GameObject readyUI; // Kahve hazýr UI

    public void StartBrewing()
    {
        if (!isBrewing)
        {
            isBrewing = true;
            timer = 0f;

            if (readyUI != null)
                readyUI.SetActive(false);
        }
    }

    public void StopBrewing()
    {
        isBrewing = false;
        timer = 0f;
    }

    void Update()
    {
        if (!isBrewing) return;

        timer += Time.deltaTime;
        if (timer >= brewTime)
        {
            isBrewing = false;
            timer = 0f;

            if (readyUI != null)
                readyUI.SetActive(true);

            Debug.Log("Kahve hazýr!");
        }
    }
}