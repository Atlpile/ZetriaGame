using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPanel : BasePanel
{
    private GameObject changeKeyTip;
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

    private bool canChangeKey;

    protected override void Awake()
    {
        base.Awake();

        GetChildrenAllUIComponent<Button>();
        GetChildrenAllUIComponent<Text>();

        changeKeyTip = this.transform.GetChild(0).gameObject;

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
        base.ShowSelf();

        LoadInputKey();
    }

    private void LoadInputKey()
    {
        //OPTIMIZE：使用循环来同步按键
        var InputDic = GameManager.Instance.m_InputController.CustomInputDic;
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
                ChangeKey(E_InputType.SwitchWeapon);
                break;
            case "btnCrouchKey":
                ChangeKey(E_InputType.Crouch);
                break;
            case "btnGunAttackKey":
                ChangeKey(E_InputType.GunAttack);
                break;
            case "btnInteractiveKey":
                ChangeKey(E_InputType.Interacitve);
                break;
            case "btnJumpKey":
                ChangeKey(E_InputType.Jump);
                break;
        }
    }

    public void ChangeKey(E_InputType inputType)
    {
        //先调出提示面板，再更新按钮文本
        changeKeyTip.gameObject.SetActive(true);
        GameManager.Instance.StartCoroutine(IE_ChangeKey(inputType));
    }

    private IEnumerator IE_ChangeKey(E_InputType inputType)
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                GameManager.Instance.m_InputController.ChangeKey(inputType);
                //TODO:保存按键配置数据
                changeKeyTip.SetActive(false);
                LoadInputKey();
                break;
            }
            else
            {
                yield return null;
            }
        }
    }


}
