using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveAndFlip
{
    bool IsRight { get; set; }

    void MoveAndFlip();
}
