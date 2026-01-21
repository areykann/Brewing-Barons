using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    [Header("Interaction State")]
    private bool canInteract = false;
    private bool isPouring = false;
    private bool hasFinishedPouring = false;

    [Header("Beer UI Elements")]
    public GameObject beerGlassUI;      // Ana Bira Paneli
    public Image beerLiquidImage;       // Dolan Sıvı (Filled Image)
    public GameObject foamObject;       // Köpük PNG'si
    public GameObject brewingUI;        // "Dolduruluyor..." yazısı
    public GameObject readyUI;          // "Hazır!" yazısı

    [Header("Settings")]
    public float fillSpeed = 0.33f;     // 3 saniyede dolum

    private PlayerStats stats;          // Diğer scripte erişim

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        stats = GetComponent<PlayerStats>(); // Aynı objedeki statları bul

        ResetEverything();
    }

    void Update()
    {
        HandleMovement();
        HandleAnimation();
        HandleInteraction();
        HandleBeerFill();
    }

    void HandleInteraction()
    {
        if (!canInteract) return;

        // E tuşuna basıldığında başlat
        if (Input.GetKeyDown(KeyCode.E))
        {
            PrepareNewBeer();
        }

        // E tuşu bırakıldığında hesapla
        if (Input.GetKeyUp(KeyCode.E) && isPouring)
        {
            isPouring = false;
            CalculateResult();
        }
    }

    void HandleBeerFill()
    {
        if (!isPouring || beerLiquidImage == null) return;

        beerLiquidImage.fillAmount += fillSpeed * Time.deltaTime;

        // Köpük Göstergesi (%88 - %98 arası perfect aralığı)
        if (beerLiquidImage.fillAmount >= 0.88f && beerLiquidImage.fillAmount < 0.99f)
        {
            if (!foamObject.activeSelf) foamObject.SetActive(true);
        }
        // Taşma Kontrolü
        else if (beerLiquidImage.fillAmount >= 0.99f)
        {
            HandleOverflow();
        }
    }

    void CalculateResult()
    {
        if (hasFinishedPouring) return;
        hasFinishedPouring = true;

        float fill = beerLiquidImage.fillAmount;

        if (foamObject.activeSelf) // PERFECT (Köpük varken bıraktı)
        {
            Debug.Log("<color=green>MÜKEMMEL DOLUM!</color>");
            stats.UpdateStats(50, 10);
            stats.AddBeer(); // Envantere ekle
        }
        else if (fill < 0.88f) // EKSİK
        {
            Debug.Log("<color=yellow>EKSİK DOLUM!</color>");
            stats.UpdateStats(-10, 0);
        }

        ShowFinalUI();
    }

    void HandleOverflow()
    {
        if (hasFinishedPouring) return;
        hasFinishedPouring = true;
        isPouring = false;

        Debug.Log("<color=red>BİRA TAŞTI!</color>");
        stats.UpdateStats(-20, 0);

        foamObject.SetActive(false);
        ShowFinalUI();
    }

    void PrepareNewBeer()
    {
        beerLiquidImage.fillAmount = 0f;
        beerGlassUI.SetActive(true);
        brewingUI.SetActive(true);
        readyUI.SetActive(false);
        foamObject.SetActive(false);
        isPouring = true;
        hasFinishedPouring = false;
    }

    void ShowFinalUI()
    {
        brewingUI.SetActive(false);
        readyUI.SetActive(true);
        Invoke("ResetEverything", 1.5f); // 1.5 sn sonra ekranı temizle
    }

    void ResetEverything()
    {
        isPouring = false;
        if (beerGlassUI) beerGlassUI.SetActive(false);
        if (foamObject) foamObject.SetActive(false);
        if (brewingUI) brewingUI.SetActive(false);
        if (readyUI) readyUI.SetActive(false);
    }

    // --- Karakter Kontrolleri ---
    void FixedUpdate() { rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime); }
    void HandleMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); movement.y = Input.GetAxisRaw("Vertical");
        if (movement.magnitude > 1) movement = movement.normalized;
        if (movement.x < 0) sr.flipX = true; else if (movement.x > 0) sr.flipX = false;
    }
    void HandleAnimation() { if (anim) { anim.SetBool("isMoving", movement != Vector2.zero); anim.SetBool("isBrewing", isPouring); } }
    void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("BeerTap")) canInteract = true; }
    void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("BeerTap")) { canInteract = false; ResetEverything(); } }
}