using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class FXSpawner : BaseComponent, IPoolObject
    {
        public override IGameStructure GameStructure => ZetriaGame.Instance;

        [SerializeField] private float _destroyTime = 0.5f;
        private Animator _animator;

        private void Awake()
        {
            _animator = this.GetComponent<Animator>();
        }

        public void OnInit()
        {

        }

        public void OnCreate()
        {
            _animator.Play("Create");
            StartCoroutine(IE_Hide());
        }

        public void OnReturn()
        {

        }

        public void OnRelease()
        {
            StopCoroutine(IE_Hide());
        }

        private IEnumerator IE_Hide()
        {
            yield return new WaitForSeconds(_destroyTime);
            Manager.GetManager<IObjectPoolManager>().ReturnObject(this.gameObject);
        }
    }
}

