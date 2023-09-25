using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using System;

namespace Zetria
{
    public class PlayerInput
    {
        public Action Action_MoveAndFlip;
        public Action Action_Crouch;
        public Action Action_Stand;
        public Action Action_PistolAttack;
        public Action Action_EmptyAttack;
        public Action Action_ShortGunAttack;
        public Action Action_MeleeAttack;
        public Action Action_Jump;
        public Action Action_AirJump;
        public Action Action_SwitchWeapon;
        public Action Action_Reload;
        public Action Action_PutDownNPC;

        private IInputManager _inputManager;
        private PlayerDynamicInfo _playerDynamicInfo;

        private bool Condition_HorizontalMove
        {
            get =>
            _playerDynamicInfo.horizontalMove != 0
            && !_playerDynamicInfo.stateInfo.isMeleeAttack
            && !_playerDynamicInfo.stateInfo.isReload;

        }
        private bool Condition_Crouch
        {
            get =>
            _playerDynamicInfo.status != E_PlayerStatus.NPC &&
            !_playerDynamicInfo.stateInfo.isReload;
            // && !_playerDynamicInfo.stateInfo.isCrouch;
        }
        private bool Condition_Stand
        {
            get =>
            _playerDynamicInfo.stateInfo.canStand;
            // && _playerDynamicInfo.stateInfo.isCrouch;
        }
        private bool Condition_GroundJump
        {
            get =>
            _playerDynamicInfo.status != E_PlayerStatus.NPC &&
            _playerDynamicInfo.stateInfo.canStand &&
            !_playerDynamicInfo.stateInfo.isReload &&
            !_playerDynamicInfo.stateInfo.isMeleeAttack;
        }
        private bool Condition_AirJump
        {
            get =>
            _playerDynamicInfo.status != E_PlayerStatus.NPC &&
            _playerDynamicInfo.currentJumpCount > 0;
        }
        private bool Condition_MeleeAttack
        {
            get =>
            _playerDynamicInfo.status != E_PlayerStatus.NPC &&
            !_playerDynamicInfo.stateInfo.isMeleeAttack &&
            !_playerDynamicInfo.stateInfo.isCrouch;
        }
        private bool Condition_PistolAttack
        {
            get =>
            (_playerDynamicInfo.status == E_PlayerStatus.Pistol || _playerDynamicInfo.status == E_PlayerStatus.NPC) &&
            !_playerDynamicInfo.stateInfo.isPistolAttack;
        }
        private bool Condition_ShortGunAttack
        {
            get =>
            _playerDynamicInfo.status == E_PlayerStatus.ShortGun &&
            !_playerDynamicInfo.stateInfo.isShotGunAttack;
        }
        private bool Condition_SwitchWeapon
        {
            get =>
            _playerDynamicInfo.status != E_PlayerStatus.NPC &&
            _playerDynamicInfo.stateInfo.hasShortGun;
        }
        private bool Condition_Reload
        {
            get =>
            _playerDynamicInfo.status != E_PlayerStatus.NPC &&
            !_playerDynamicInfo.stateInfo.isReload &&
            !_playerDynamicInfo.stateInfo.isPistolAttack &&
            !_playerDynamicInfo.stateInfo.isShotGunAttack &&
            // CanReload() &&
            _playerDynamicInfo.stateInfo.canReload &&
            _playerDynamicInfo.stateInfo.canStand;
        }
        private bool Condition_PutDownNPC
        {
            get => _playerDynamicInfo.status == E_PlayerStatus.NPC;
        }
        private bool Condition_IsGround
        {
            get => _playerDynamicInfo.stateInfo.isGround;
        }
        private bool Condition_CanFireAttack
        {
            get => _playerDynamicInfo.stateInfo.canFireAttack;
        }

        public PlayerInput(IInputManager manager, PlayerDynamicInfo dynamicInfo)
        {
            this._inputManager = manager;
            this._playerDynamicInfo = dynamicInfo;
        }

        public void UpdatePlayerInput()
        {
            if (Condition_HorizontalMove)
                Action_MoveAndFlip?.Invoke();

            if (Condition_IsGround)
            {
                if (_inputManager.GetKey(E_InputTypes.Crouch) && Condition_Crouch)
                    Action_Crouch?.Invoke();
                else if (Condition_Stand)
                    Action_Stand?.Invoke();

                MouseInput();

                if (_inputManager.GetKeyDown(E_InputTypes.Jump) && Condition_GroundJump)
                    Action_Jump?.Invoke();
                else if (_inputManager.GetKeyDown(E_InputTypes.SwitchWeapon) && Condition_SwitchWeapon)
                    Action_SwitchWeapon?.Invoke();
                else if (_inputManager.GetKeyDown(E_InputTypes.MeleeAttack) && Condition_MeleeAttack)
                    Action_MeleeAttack?.Invoke();
                else if (_inputManager.GetKeyDown(E_InputTypes.Reload) && Condition_Reload)
                    Action_Reload?.Invoke();
                else if (_inputManager.GetKeyDown(E_InputTypes.PutDownNPC) && Condition_PutDownNPC)
                    Action_PutDownNPC?.Invoke();
            }
            else
            {
                Action_Stand?.Invoke();

                MouseInput();

                if (_inputManager.GetKeyDown(E_InputTypes.Jump) && Condition_AirJump)
                    Action_AirJump?.Invoke();
                else if (_inputManager.GetKeyDown(E_InputTypes.SwitchWeapon) && Condition_SwitchWeapon)
                    Action_SwitchWeapon?.Invoke();
            }
        }

        private void MouseInput()
        {
            if (_inputManager.GetMouseButton(0) && Condition_PistolAttack)
            {
                if (Condition_CanFireAttack)
                    Action_PistolAttack?.Invoke();
                else
                    Action_EmptyAttack?.Invoke();
            }
            if (_inputManager.GetMouseButton(0) && Condition_ShortGunAttack)
            {
                if (Condition_CanFireAttack)
                    Action_ShortGunAttack?.Invoke();
                else
                    Action_EmptyAttack?.Invoke();
            }
        }

    }

}

