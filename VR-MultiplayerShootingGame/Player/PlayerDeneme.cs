using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeneme : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Sound sound = new Sound(transform.position, 30);
            Sounds.MakeSound(sound);
        }
    }
}
