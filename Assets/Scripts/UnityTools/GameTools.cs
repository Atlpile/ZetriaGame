using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTools
{
    public static RaycastHit2D ShowRay(Vector2 rayPos, Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPos + offset, rayDirection, length, layer);
        Color rayColor = hitInfo ? Color.red : Color.green;
        Debug.DrawRay(rayPos + offset, rayDirection * length, rayColor);

        return hitInfo;
    }
}
