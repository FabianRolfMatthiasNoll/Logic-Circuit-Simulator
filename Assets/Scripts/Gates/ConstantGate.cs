using System;
using UnityEngine;

public class ConstantGate : Gate
{
    public override bool GetOutput()
    {
        return logicLevel;
    }
}
