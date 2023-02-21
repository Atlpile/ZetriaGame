using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    private Text PistolAmmoCount;
    private Text ShoutGunAmmoCount;
    private Image HealthSlider;
    private Image PistolAmmoPointer;
    private Image ShoutGunAmmoPointer;
    private Image DoorCard;


    protected override void Awake()
    {
        GetChildrenAllUIComponent<Text>();
        GetChildrenAllUIComponent<Image>();

        PistolAmmoCount = GetUIComponent<Text>("PistolAmmoCount");
        ShoutGunAmmoCount = GetUIComponent<Text>("ShoutGunAmmoCount");
        HealthSlider = GetUIComponent<Image>("HealthSlider");
        PistolAmmoPointer = GetUIComponent<Image>("PistolAmmoPointer");
        ShoutGunAmmoPointer = GetUIComponent<Image>("ShoutGunAmmoPointer");
        DoorCard = GetUIComponent<Image>("DoorCard");

    }

    private void OnEnable()
    {
        GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpNPC, UpdateAmmoPointer);
        GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpDoorCard, UpdateDoorCard);
    }

    private void OnDisable()
    {
        GameManager.Instance.m_EventManager.RemoveEventListener(E_EventType.PickUpNPC, UpdateAmmoPointer);
        GameManager.Instance.m_EventManager.RemoveEventListener(E_EventType.PickUpDoorCard, UpdateDoorCard);
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

    public void UpdateDoorCard()
    {
        DoorCard.gameObject.SetActive(true);
    }

    public void UpdateLifeBar(float currentHP, float maxHP)
    {
        HealthSlider.fillAmount = currentHP / maxHP;
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

    private void UpdateAmmoPointer()
    {
        PistolAmmoPointer.gameObject.SetActive(true);
        ShoutGunAmmoPointer.gameObject.SetActive(false);
    }

}
