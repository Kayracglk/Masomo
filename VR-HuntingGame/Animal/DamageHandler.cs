using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public GameObject damage50Prefab; // Prefab for Damage50
    public GameObject kritikPrefab;   // Prefab for kritik
    public GameObject damage25Prefab; // Prefab for damage25

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision has one of the specified layers
        if (collision.gameObject.layer == LayerMask.NameToLayer("Damage50"))
        {
            Instantiate(damage50Prefab, collision.contacts[0].point, Quaternion.identity);
            Debug.Log( "50" );
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("kritik"))
        {
            Instantiate(kritikPrefab, collision.contacts[0].point, Quaternion.identity);
            Debug.Log( "50" );
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("damage25"))
        {
            Instantiate(damage25Prefab, collision.contacts[0].point, Quaternion.identity);
            Debug.Log( "50" );
        }
    }
}
