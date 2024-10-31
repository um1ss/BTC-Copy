using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBackground : MonoBehaviour
{
    public abstract string Name { get; }
    public void Destroy()
    {

    }
}
