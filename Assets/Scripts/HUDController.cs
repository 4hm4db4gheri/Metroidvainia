using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;
    [SerializeField] Image[] images;
    [SerializeField] Sprite fullHeart, brokenHeart;
    [SerializeField] Transform HealthParent;
    
    private void Awake()
    {
        instance = this;
    }

    public void Repaint(int health)
    {
        
        for (int i = 0; i < images.Length; i++)
            images[i].sprite = brokenHeart;

        for (int i = 0; i < health; i++)
            images[i].sprite = fullHeart;

    }
    
}
