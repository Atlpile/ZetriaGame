using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    private Text text_PistolAmmoCount;
    private Text text_ShortGunAmmoCount;
    private Image img_HealthSlider;
    private Image img_PistolAmmoPointer;
    private Image img_ShortGunAmmoPointer;
    private Image img_DoorCard;
    private Image img_Token_Obtain;

    protected override void Awake()
    {
        base.Awake();

        GetChildrenAllUIComponent<Text>();
        GetChildrenAllUIComponent<Image>();

        text_PistolAmmoCount = GetUIComponent<Text>(nameof(text_PistolAmmoCount));
        text_ShortGunAmmoCount = GetUIComponent<Text>(nameof(text_ShortGunAmmoCount));
        img_HealthSlider = GetUIComponent<Image>(nameof(img_HealthSlider));
        img_PistolAmmoPointer = GetUIComponent<Image>(nameof(img_PistolAmmoPointer));
        img_ShortGunAmmoPointer = GetUIComponent<Image>(nameof(img_ShortGunAmmoPointer));
        img_DoorCard = GetUIComponent<Image>(nameof(img_DoorCard));
        img_Token_Obtain = GetUIComponent<Image>(nameof(img_Token_Obtain));

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

    public override void Hide(TweenCallback callback)
    {
        base.Hide(callback);

        GameManager.Instance.AudioManager.BGMSetting(E_AudioSettingType.Stop);
    }

    public void UpdateDoorCard()
    {
        img_DoorCard.gameObject.SetActive(true);
    }

    public void UpdateLifeBar(float currentHP, float maxHP)
    {
        img_HealthSlider.fillAmount = currentHP / maxHP;
    }

    public void UpdatePistolAmmoText(int currentCount, int maxCount)
    {
        text_PistolAmmoCount.text = currentCount + "/" + maxCount;
    }

    public void UpdateShortGunAmmoText(int currentCount, int maxCount)
    {
        text_ShortGunAmmoCount.text = currentCount + "/" + maxCount;
    }

    public void UpdateAmmoPointer(bool isAcitve)
    {
        img_PistolAmmoPointer.gameObject.SetActive(isAcitve);
        img_ShortGunAmmoPointer.gameObject.SetActive(!isAcitve);
    }

    private void UpdateAmmoPointer()
    {
        img_PistolAmmoPointer.gameObject.SetActive(true);
        img_ShortGunAmmoPointer.gameObject.SetActive(false);
    }

    private void UpdateToken(Token token)
    {
        img_Token_Obtain.gameObject.SetActive(true);
    }
}
