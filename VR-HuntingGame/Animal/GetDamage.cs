using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDamage : MonoBehaviour, IDamagable
{
    public HealthManager healthManager;
    public float customDamage;
    public int CustomPoint; 

    private float displayDuration = 0.32f;

    private string damage50PrefabName = "Damage(50)";
    private string kritikPrefabName = "Kritik(100)";
    private string damage25PrefabName = "Damage(25)";

    
    private ScoreManager scoreManager;

    private void Start()
    {
       
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void GiveDamage(Vector3 hitPosition, float damage)
    {
        healthManager.GiveDamage(customDamage, hitPosition);

        // Puan ver
        if (scoreManager != null)
        {
            scoreManager.AddScore(CustomPoint);
        }

        // Damage gösterimi
        if (gameObject.layer == LayerMask.NameToLayer("Damage50"))
        {
            ShowDamagePrefab(damage50PrefabName, hitPosition);
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Kritik"))
        {
            ShowDamagePrefab(kritikPrefabName, hitPosition);
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Damage25"))
        {
            ShowDamagePrefab(damage25PrefabName, hitPosition);
        }
    }

    private void ShowDamagePrefab(string prefabName, Vector3 hitPosition)
    {
        GameObject damageObj = Instantiate(Resources.Load(prefabName) as GameObject, hitPosition, Quaternion.identity);
        Destroy(damageObj, displayDuration);
    }
}