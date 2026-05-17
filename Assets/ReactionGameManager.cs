using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReactionGameManager : MonoBehaviour
{
    [Header("UI Referanslarý")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI statusText;
    public Image buttonImage;

    // YENÝ: Unity'den ekleyeceðimiz Bitir butonunu buraya baðlayacaðýz
    public GameObject quitButton;

    [Header("Çerçeve (Glow/Outline) Renkleri")]
    public Color maviCerceve = new Color(0f, 0.2f, 0.8f, 1f);
    public Color kirmiziCerceve = Color.red;

    [Header("Buton Durum Renkleri")]
    public Color colorWait = Color.yellow;
    public Color colorSignal = Color.green;
    public Color colorFinished = Color.red;
    public Color colorEnd = new Color(0.2f, 0.2f, 0.2f, 1f);

    private enum GameState { Idle, WaitingForSignal, WaitingForClick, Finished, ProjectEnd }
    private GameState currentState = GameState.Idle;

    private float randomWaitTime;
    private float signalDisplayTime;

    void Start()
    {
        StartRound();
    }

    void Update()
    {
        if (currentState == GameState.WaitingForSignal)
        {
            randomWaitTime -= Time.deltaTime;
            if (randomWaitTime <= 0)
            {
                ShowSignal();
            }
        }
    }

    public void OnScreenClicked()
    {
        switch (currentState)
        {
            case GameState.Idle:
                StartRound();
                break;
            case GameState.WaitingForSignal:
                FalseStart();
                break;
            case GameState.WaitingForClick:
                SuccessfulClick();
                break;
            case GameState.Finished:
                ShowEndScreen();
                break;
            case GameState.ProjectEnd:
                // Sadece ekrana týklarsa yeniden baþlar (Bitir'e týklamazsa)
                StartRound();
                break;
        }
    }

    void StartRound()
    {
        currentState = GameState.WaitingForSignal;

        // Oyun baþlarken Çýkýþ butonunu gizle
        if (quitButton != null) quitButton.SetActive(false);

        statusText.text = "Hazýr Ol...";
        SetTextStyling(statusText, Color.white, new Color(0f, 0.2f, 1f, 1f), 0.08f);

        resultText.text = "";

        if (buttonImage != null) buttonImage.color = colorWait;

        randomWaitTime = Random.Range(2f, 5f);
    }

    void ShowSignal()
    {
        currentState = GameState.WaitingForClick;

        statusText.text = "TIKLA!";
        SetTextStyling(statusText, Color.white, new Color(0f, 0.2f, 1f, 1f), 0.08f);

        resultText.text = "";

        if (buttonImage != null) buttonImage.color = colorSignal;

        signalDisplayTime = Time.time;
    }

    void FalseStart()
    {
        currentState = GameState.Finished;

        statusText.text = "Çok Erken!\n<color=white><size=60%>(Devam etmek için týkla)</size></color>";
        SetTextStyling(statusText, new Color(0.15f, 0.15f, 0.15f, 1f), kirmiziCerceve, 0.08f);

        resultText.text = "";

        if (buttonImage != null) buttonImage.color = colorFinished;
    }

    void SuccessfulClick()
    {
        currentState = GameState.Finished;
        float reactionTime = (Time.time - signalDisplayTime) * 1000f;
        int ms = Mathf.RoundToInt(reactionTime);

        string unvan = "";
        if (ms < 180) unvan = "<color=#FFFFFF>SÝBER NÝNJA!</color>";
        else if (ms < 250) unvan = "<color=#FFFFFF>YAZILIMCI REFLEKSÝ</color>";
        else if (ms < 350) unvan = "<color=#FFFFFF>NORMAL KÝÞÝ</color>";
        else unvan = "<color=#FFFFFF>KAPLUMBAÐA</color>";

        statusText.text = $"Tepki Süren:\nDeðerlendirme:\n{unvan}\n<color=white><size=55%>(Devam etmek için týkla)</size></color>";
        SetTextStyling(statusText, new Color(0.15f, 0.15f, 0.15f, 1f), kirmiziCerceve, 0.08f);

        resultText.text = $"{ms} ms";
        SetTextStyling(resultText, new Color(0.15f, 0.15f, 0.15f, 1f), kirmiziCerceve, 0.08f);

        if (buttonImage != null) buttonImage.color = colorFinished;
    }

    void ShowEndScreen()
    {
        currentState = GameState.ProjectEnd;

        // Bitiþ ekraný geldiðinde Çýkýþ butonunu görünür yap!
        if (quitButton != null) quitButton.SetActive(true);

        statusText.text = "Oynadýðýnýz Ýçin\nTeþekkürler!";

        // Ekrana týklayan yeniden oynar, butona týklayan çýkar.
        resultText.text = "<size=60%>Yeniden oynamak için gri butona,\nçýkmak için aþaðýdaki kýrmýzý butona týkla</size>";

        SetTextStyling(statusText, Color.white, new Color(0f, 0.2f, 1f, 1f), 0.08f);
        SetTextStyling(resultText, Color.white, new Color(0f, 0.2f, 1f, 1f), 0.08f);

        if (buttonImage != null) buttonImage.color = colorEnd;
    }

    // YENÝ METOT: Oyundan çýkýþ iþlemi
    // YENÝ METOT: Oyundan çýkýþ iþlemi (Editörde de çalýþýr!)
    public void QuitGame()
    {
        Debug.Log("Oyun Kapatýlýyor...");

#if UNITY_EDITOR
        // Eðer Unity Editörü içindeysek, Play modunu durdurur
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Eðer Build alýnmýþ gerçek oyundaysak, komple uygulamayý kapatýr
            Application.Quit(); 
#endif
    }

    void SetTextStyling(TextMeshProUGUI textObj, Color icRenk, Color cerceveRenki, float kalinlik)
    {
        if (textObj == null) return;

        textObj.color = icRenk;
        textObj.fontStyle = FontStyles.Bold;

        if (textObj.fontMaterial != null)
        {
            textObj.fontMaterial.SetColor("_OutlineColor", cerceveRenki);
            textObj.fontMaterial.SetColor("_GlowColor", cerceveRenki);
            textObj.fontMaterial.SetFloat("_OutlineWidth", kalinlik);
        }
    }
}