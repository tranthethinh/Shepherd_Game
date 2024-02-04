using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepRed : Sheep
{
    protected override void Update()
    {
        base.Update();
        isRunToGroup = true;
    }
}
