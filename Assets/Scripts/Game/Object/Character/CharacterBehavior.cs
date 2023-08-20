using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior
{
    public void MoveAndFlip(Transform transform, bool isRight, float moveSpeed, float horizontalMove)
    {
        if (horizontalMove > 0)
        {
            transform.Translate(horizontalMove * moveSpeed * Time.deltaTime * Vector2.right);
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            isRight = true;
        }
        else if (horizontalMove < 0)
        {
            transform.Translate(horizontalMove * moveSpeed * Time.deltaTime * Vector2.left);
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            isRight = false;
        }
    }
}
