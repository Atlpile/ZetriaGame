using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public enum E_FXType { JumpFX, KickFX }

    public class FXSpawner : BaseComponent, IPoolObject
    {
        public override IGameStructure GameStructure => ZetriaGame.Instance;

        public E_FXType type;
        [SerializeField] private float _destroyTime = 0.5f;
        private Animator _animator;
        private Vector3 leftOffset;
        private Vector3 rightOffset;

        public void OnInit()
        {
            _animator = this.GetComponent<Animator>();
        }

        public void OnCreate()
        {
            _animator.Play("Create");
            SetOffset();
            OnReturn();
        }

        public void OnReturn()
        {
            StartCoroutine(IE_Hide());
        }

        public void OnRelease()
        {
            StopCoroutine(IE_Hide());
        }

        public void SetFXTransform(bool isRight, Transform playerTranfrom)
        {
            if (isRight)
                this.transform.position = playerTranfrom.position + rightOffset;
            else
                this.transform.position = playerTranfrom.position + leftOffset;

            this.transform.rotation = playerTranfrom.rotation;
        }

        private IEnumerator IE_Hide()
        {
            yield return new WaitForSeconds(_destroyTime);
            Manager.GetManager<IObjectPoolManager>().ReturnObject(this.gameObject);
        }

        private void SetOffset()
        {
            switch (type)
            {
                case E_FXType.JumpFX:
                    break;
                case E_FXType.KickFX:
                    leftOffset = new Vector2(-0.75f, 1f);
                    rightOffset = new Vector2(0.75f, 1f);
                    break;
            }
        }
    }
}

