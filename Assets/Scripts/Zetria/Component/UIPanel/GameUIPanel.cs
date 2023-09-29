using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using UnityEngine.UI;

namespace Zetria
{
    public class GameUIPanel : BaseUIPanel
    {
        public override IGameStructure GameStructure => ZetriaGame.Instance;

        private Text text_PistolAmmoCount;
        private Text text_ShortGunAmmoCount;
        private Image img_HealthSlider;
        private Image img_PistolAmmoPointer;
        private Image img_ShortGunAmmoPointer;
        private Image img_DoorCard;
        private Image img_Token_Obtain;

        public override void Show()
        {
            // UpdatePistolAmmoText(0, 0);
            // UpdateShortGunAmmoText(0, 0);
            // UpdateAmmoPointer(true);

            // PlayerController player = GameManager.Instance.Player;
            // // Debug.Log("Player当前血量" + player.ZetriaInfo.currentHealth);

            // if (player != null)
            //     UpdateLifeBar(player.ZetriaInfo.currentHealth, player.ZetriaInfo.maxHealth);
            // else
            //     Debug.LogWarning("场景中不存在Player,无法更新血条");

            var playerModel = GameStructure.GetModel<IPlayerModel>();
            var ammoModel = GameStructure.GetModel<IAmmoModel>();

            if (playerModel.IsInLevel)
            {
                UpdateLifeBar(playerModel.CurrentHealth, playerModel.MaxHealth);
                UpdatePistolAmmoText(ammoModel.PistlAmmoInfo.currentCount, ammoModel.PistlAmmoInfo.maxCount);
                UpdateShortGunAmmoText(ammoModel.ShortGunAmmoInfo.currentCount, ammoModel.ShortGunAmmoInfo.maxCount);
                UpdateAmmoPointer(true);
            }
            else
            {
                Debug.LogWarning("场景中不存在Player,无法更新UI");
            }
        }

        protected override void OnGetUIComponent()
        {
            GetChildAllUIComponent<Text>();
            GetChildAllUIComponent<Image>();

            text_PistolAmmoCount = GetUIComponent<Text>(nameof(text_PistolAmmoCount));
            text_ShortGunAmmoCount = GetUIComponent<Text>(nameof(text_ShortGunAmmoCount));
            img_HealthSlider = GetUIComponent<Image>(nameof(img_HealthSlider));
            img_PistolAmmoPointer = GetUIComponent<Image>(nameof(img_PistolAmmoPointer));
            img_ShortGunAmmoPointer = GetUIComponent<Image>(nameof(img_ShortGunAmmoPointer));
            img_DoorCard = GetUIComponent<Image>(nameof(img_DoorCard));
            img_Token_Obtain = GetUIComponent<Image>(nameof(img_Token_Obtain));
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

        private void OnUpdateAmmoPointer()
        {
            img_PistolAmmoPointer.gameObject.SetActive(true);
            img_ShortGunAmmoPointer.gameObject.SetActive(false);
        }

        private void OnUpdateToken(Token token)
        {
            img_Token_Obtain.gameObject.SetActive(true);
        }

        private void OnUpdateDoorCard()
        {
            img_DoorCard.gameObject.SetActive(true);
        }
    }
}


