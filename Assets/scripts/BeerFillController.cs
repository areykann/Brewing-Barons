using UnityEngine;
using UnityEngine.UI;

public class BeerFillController : MonoBehaviour
{
    [Header("References")]
    public RectTransform beerLiquid;

    [Header("Fill Settings")]
    public float fillSpeed = 0.4f;
    public float maxFill = 1.2f;

    private float fillAmount = 0f;
    private bool isPouring = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isPouring = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isPouring = false;
            CheckScore();
        }

        if (isPouring)
        {
            fillAmount += fillSpeed * Time.deltaTime;
            fillAmount = Mathf.Clamp(fillAmount, 0, maxFill);

            beerLiquid.localScale = new Vector3(1, fillAmount, 1);
        }
    }

    void CheckScore()
    {
        if (fillAmount >= 0.95f && fillAmount <= 1.05f)
            Debug.Log("PERFECT +10");
        else if (fillAmount >= 0.75f)
            Debug.Log("OK +2");
        else if (fillAmount > 1.05f)
            Debug.Log("OVERFLOW -10");
        else
            Debug.Log("BAD -5");
    }
}