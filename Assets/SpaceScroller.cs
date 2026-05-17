using UnityEngine;
using UnityEngine.UI; // RawImage kullanmak için bu kütüphane þart

public class SpaceScroller : MonoBehaviour
{
    private RawImage backgroundImage;

    // Uzayýn kayma hýzý. X saða/sola, Y yukarý/aþaðý kaydýrýr.
    public float scrollSpeedX = 0.02f;
    public float scrollSpeedY = 0.02f;

    void Start()
    {
        backgroundImage = GetComponent<RawImage>();
    }

    void Update()
    {
        // Görselin doku koordinatlarýný (UV) zamanla kaydýrýyoruz
        Rect currentRect = backgroundImage.uvRect;
        currentRect.x += scrollSpeedX * Time.deltaTime;
        currentRect.y += scrollSpeedY * Time.deltaTime;
        backgroundImage.uvRect = currentRect;
    }
}