using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public Transform enemiesParent;
    public List<AIManager> enemyList = new List<AIManager>();
    private bool isWorked = false;

    private void Awake()
    {
        instance = this;

        GetAllEnemies();
    }

    private void GetAllEnemies()
    {
        for (int i = 0; i < enemiesParent.childCount; i++)
        {
            enemyList.Add(enemiesParent.GetChild(i).gameObject.GetComponent<AIManager>());
        }
    }

    public void AttackAllPlayer(AIManager agent)
    {
        if(!isWorked)
        {
            int index = enemyList.IndexOf(agent);
            enemyList[index].stateMachine.ChangeState(AIStateID.AttackPlayerState);
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (index != i)
                {
                    enemyList[i].playerTransform = agent.playerTransform;
                    enemyList[i].stateMachine.ChangeState(AIStateID.CoverState);
                }
            }
            isWorked = true;
        }
        
    }
}
