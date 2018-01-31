using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("HUD")]
    public Slider damageSlider;
    public Color damageSliderColorMin = Color.green;
    public Color damageSliderColorMax = Color.red;
    private Image _damageFillArea;

    public Animator anim;

    void Awake()
    {
        _damageFillArea = damageSlider.fillRect.GetComponent<Image>();
        damageSlider.maxValue = GameManager.maxLife;
    }


     
    void Start()
    {
        _damageFillArea.color = Color.Lerp(damageSliderColorMin, damageSliderColorMax, GameManager.Life / damageSlider.maxValue);

         damageSlider.value = GameManager.Life;
    }

    private void Update()
    {
        damageSlider.value = GameManager.Life;
        _damageFillArea.color = Color.Lerp(damageSliderColorMin, damageSliderColorMax, GameManager.Life / damageSlider.maxValue);   
        if(GameManager.Life<=0)
        {
            anim.SetTrigger("Death");
            
        }
    }
}
