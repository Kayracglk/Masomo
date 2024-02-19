using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DestroyOK : MonoBehaviour
{
    public float time = 10f;
    void Start()
    {
        GameObject.Destroy(this.gameObject, time);
    }

}
