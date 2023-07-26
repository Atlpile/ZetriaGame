using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    private Image Token_Obtain;


    protected override void Awake()
    {
        base.Awake();

        GetChildrenAllUIComponent<Text>();
        GetChildrenAllUIComponent<Image>();

        PistolAmmoCount = GetUIComponent<Text>("PistolAmmoCount");
        ShoutGunAmmoCount = GetUIComponent<Text>("ShoutGunAmmoCount");
        HealthSlider = GetUIComponent<Image>("HealthSlider");
        PistolAmmoPointer = GetUIComponent<Image>("PistolAmmoPointer");
        ShoutGunAmmoPointer = GetUIComponent<Image>("ShoutGunAmmoPointer");
        DoorCard = GetUIComponent<Image>("DoorCard");
        Token_Obtain = GetUIComponent<Image>("Token_Obtain");

        GameManager.Instance.EventManager.AddEventListener(E_EventType.PickUpNPC, UpdateAmmoPointer);
        GameManager.Instance.EventManager.AddEventListener(E_EventType.PickUpDoorCard, UpdateDoorCard);
        GameManager.Instance.EventManager.AddEventListener<Token>(E_EventType.PickUpToken, UpdateToken);

    }

    private void OnDestroy()
    {
        GameManager.Instance.EventManager.RemoveEventListener(E_EventType.PickUpNPC, UpdateAmmoPointer);
        GameManager.Instance.EventManager.RemoveEventListener(E_EventType.PickUpDoorCard, UpdateDoorCard);
        GameManager.Instance.EventManager.RemoveEventListener<Token>(E_EventType.PickUpToken, UpdateToken);
    }

    public override void Show(TweenCallback ShowCallBack)
    {
        base.Show(ShowCallBack);

        UpdatePistolAmmoText(0, 0);
        UpdateShortGunAmmoText(0, 0);
        UpdateAmmoPointer(true);

        PlayerController player = GameManager.Instance.Player;
        // Debug.Log("Player当前血量" + player.ZetriaInfo.currentHealth);

        if (player != null)
            UpdateLifeBar(player.ZetriaInfo.currentHealth, player.ZetriaInfo.maxHealth);
        else
            Debug.LogWarning("场景中不存在Player,无法更新血条");
    }

    public override void Show()
    {
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.BGM, "bgm_02", true);
        UpdatePistolAmmoText(0, 0);
        UpdateShortGunAmmoText(0, 0);
        UpdateAmmoPointer(true);

        PlayerController player = GameManager.Instance.Player;
        // Debug.Log("Player当前血量" + player.ZetriaInfo.currentHealth);

        if (player != null)
            UpdateLifeBar(player.ZetriaInfo.currentHealth, player.ZetriaInfo.maxHealth);
        else
            Debug.LogWarning("场景中不存在Player,无法更新血条");
    }

    public override void Hide(TweenCallback callback)
    {
        base.Hide(callback);

        GameManager.Instance.AudioManager.BGMSetting(E_AudioSettingType.Stop);
    }

    public override void Hide()
    {
        GameManager.Instance.AudioManager.BGMSetting(E_AudioSettingType.Stop);
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

    private void UpdateToken(Token token)
    {
        Token_Obtain.gameObject.SetActive(true);
    }
}
