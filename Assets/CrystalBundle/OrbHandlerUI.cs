using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHandler : MonoBehaviour
{
    public List<orbsUI> orbBar = new List<orbsUI>();
    public enum orbsUI
    {
        none = 0,
        echo = 1,
        slice = 2,
        crystal = 3
    }
}
