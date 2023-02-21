using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPanel : BasePanel
{
    private Button btnSwitchWeaponKey;
    private Button btnCrouchKey;
    private Button btnGunAttackKey;
    private Button btnInteractiveKey;
    private Button btnJumpKey;
    private Text txtSwitchWeaponKey;
    private Text txtCrouchKey;
    private Text txtGunAttackKey;
    private Text txtInteractiveKey;
    private Text txtJumpKey;

    protected override void Awake()
    {
        GetChildrenAllUIComponent<Button>();
        GetChildrenAllUIComponent<Text>();

        btnSwitchWeaponKey = GetUIComponent<Button>("btnSwitchWeaponKey");
        btnCrouchKey = GetUIComponent<Button>("btnCrouchKey");
        btnGunAttackKey = GetUIComponent<Button>("btnGunAttackKey");
        btnInteractiveKey = GetUIComponent<Button>("btnInteractiveKey");
        btnJumpKey = GetUIComponent<Button>("btnJumpKey");

        txtSwitchWeaponKey = GetUIComponent<Text>("txtSwitchWeaponKey");
        txtCrouchKey = GetUIComponent<Text>("txtCrouchKey");
        txtGunAttackKey = GetUIComponent<Text>("txtGunAttackKey");
        txtInteractiveKey = GetUIComponent<Text>("txtInteractiveKey");
        txtJumpKey = GetUIComponent<Text>("txtJumpKey");
    }

    public override void ShowSelf()
    {
        LoadInputKey();
    }

    private void LoadInputKey()
    {
        var InputDic = GameManager.Instance.m_InputController.CustomInputDic2;
        txtSwitchWeaponKey.text = InputDic[E_InputType.SwitchWeapon].ToString();
        txtCrouchKey.text = InputDic[E_InputType.Crouch].ToString();
        txtGunAttackKey.text = InputDic[E_InputType.GunAttack].ToString();
        txtInteractiveKey.text = InputDic[E_InputType.Interacitve].ToString();
        txtJumpKey.text = InputDic[E_InputType.Jump].ToString();
    }

    protected override void OnClick(string buttonName)
    {
        switch (buttonName)
        {
            case "btnSwitchWeaponKey":
                // GameManager.Instance.m_InputController.ChangeInput(E_InputType.SwitchWeapon);
                break;
            case "btnCrouchKey":
                break;
            case "btnGunAttackKey":
                break;
            case "btnInteractiveKey":
                break;
            case "btnJumpKey":
                break;
        }
    }

    public void ChangeKey()
    {
        //先调出“请按下任意键”提示面板，再将当前按钮的文本，切换为按下按键的文本
    }

    private void UpdateKeyText()
    {

    }
}
