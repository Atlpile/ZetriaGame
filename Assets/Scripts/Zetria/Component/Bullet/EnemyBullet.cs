using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class EnemyBullet : BaseBullet
    {
        public E_EnemyBulletType type;
        [SerializeField] private Transform _playerPos;
        private readonly float _chaseDistance = 3f;

        public override void OnInit()
        {
            base.OnInit();

            moveSpeed = 10f;
            currentMoveSpeed = moveSpeed;

            _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            anim.Play("Run");
        }

        private void Update()
        {
            Move(type);
        }


        private void Move(E_EnemyBulletType type)
        {
            switch (type)
            {
                case E_EnemyBulletType.Horizontal:
                    HorizontalMove();
                    break;
                case E_EnemyBulletType.Down:
                    DownMove();
                    break;
                case E_EnemyBulletType.Chase:
                    ChaseMove();
                    break;
            }
        }

        private void HorizontalMove()
        {
            transform.Translate(currentMoveSpeed * Time.deltaTime * Vector2.right);
        }

        private void DownMove()
        {
            transform.Translate(currentMoveSpeed * Time.deltaTime * Vector2.down);
        }

        private void ChaseMove()
        {
            //开始时追击Player
            this.transform.position = _playerPos.position;

            //到达一定距离时不进行追击
            if (Vector2.Distance(this.transform.position, _playerPos.position) < _chaseDistance)
            {
                //利用向量使其移动
            }
        }


    }
}


