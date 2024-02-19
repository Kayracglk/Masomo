using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/MissionManager", order = 1)]
public class MissionList : ScriptableObject
{
    public List<Missions> levels;
}


[Serializable]
public struct Missions
{
    public Transform spawnPointParent;
    public List<Mission> missionsList;
}

[Serializable]
public struct Mission
{
    public int count;
    public AnimalType type;
}
