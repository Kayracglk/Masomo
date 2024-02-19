using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{
    private AIManager agent;

    private void Awake()
    {
        agent = GetComponent<AIManager>();
    }

    private void Update()
    {
        
    }

}
