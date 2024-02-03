using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepNoFlee : Sheep
{
    protected override void Start()
    {
        base.Start();
        canScare = false;
    }
    protected override void Update()
    {
        isFlee = false;
        base.Update();
    }
}
