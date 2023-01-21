using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    [SerializeField] private Text PistolAmmoCount;
    [SerializeField] private Text ShoutGunAmmoCount;
    [SerializeField] private Image HealthSlider;
    [SerializeField] private Image PistolAmmoPointer;
    [SerializeField] private Image ShoutGunAmmoPointer;


    protected override void Awake()
    {
        GetChildrenAllUIComponent<Text>();
        GetChildrenAllUIComponent<Image>();

        PistolAmmoCount = GetUIComponent<Text>("PistolAmmoCount");
        ShoutGunAmmoCount = GetUIComponent<Text>("ShoutGunAmmoCount");
        HealthSlider = GetUIComponent<Image>("HealthSlider");
        PistolAmmoPointer = GetUIComponent<Image>("PistolAmmoPointer");
        ShoutGunAmmoPointer = GetUIComponent<Image>("ShoutGunAmmoPointer");

    }



    public override void ShowSelf()
    {
        UpdatePistolAmmoText(0, 0);
        UpdateShortGunAmmoText(0, 0);
        UpdateAmmoPointer(true);
    }

    public override void HideSelf()
    {

    }


    public void UpdatePistolAmmoText(int currentCount, int maxCount)
    {
        PistolAmmoCount.text = currentCount + "/" + maxCount;
    }

    public void UpdateShortGunAmmoText(int currentCount, int maxCount)
    {
        ShoutGunAmmoCount.text = currentCount + "/" + maxCount;
    }

    public void UpdateAmmoPointer(bool isAcitve)
    {
        PistolAmmoPointer.gameObject.SetActive(isAcitve);
        ShoutGunAmmoPointer.gameObject.SetActive(!isAcitve);
    }

    public void UpdateLifeBar(int life)
    {
        HealthSlider.fillAmount = life;
    }
}
