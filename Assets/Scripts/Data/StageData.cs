using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageInfo
{
    public int stageKey;
    public WaveData[] waves;

    public StageInfo(int stageKey, WaveData[] waves)
    {
        this.stageKey = stageKey;
        this.waves = waves;
    }
}

[System.Serializable]
public class WaveData
{
    public MonsterSpawnData[] monsters;
    public bool hasBoss;
    public string bossType;
    public WaveData(MonsterSpawnData[] monsters, bool hasBoss, string bossType)
    {
        this.monsters = monsters;
        this.hasBoss = hasBoss;
        this.bossType = bossType;
    }
}

[System.Serializable]
public class MonsterSpawnData
{
    public string monsterType;
    public int spawnCount;
    
    public MonsterSpawnData(string monsterType, int spawnCount)
    {
        this.monsterType = monsterType;
        this.spawnCount = spawnCount;
    }
}

public static class StageData
{
    public static readonly StageInfo[] stages = new StageInfo[]
    {
        new StageInfo(0, new WaveData[]
        {
            new WaveData(new MonsterSpawnData[]
            {
                new MonsterSpawnData("GoblinWithBow", 1),
            }, false, ""),
            new WaveData(new MonsterSpawnData[]
            {
                new MonsterSpawnData("GoblinWithKnife", 3),
            }, false, ""),
            new WaveData(new MonsterSpawnData[]
            {
                new MonsterSpawnData("GoblinWithBow", 2),
                new MonsterSpawnData("GoblinWithKnife", 2),
                new MonsterSpawnData("GoblinWithBow", 2),
            }, true, "Orc_Shaman")
        }),
        new StageInfo(1, new WaveData[]
        {
            new WaveData(new MonsterSpawnData[]
            {
                new MonsterSpawnData("GoblinWithBow", 5),
            }, false, ""),
            new WaveData(new MonsterSpawnData[]
            {
                new MonsterSpawnData("GoblinWithKnife", 10),
            }, false, ""),
            new WaveData(new MonsterSpawnData[]
            {
                new MonsterSpawnData("GoblinWithBow", 10),
                new MonsterSpawnData("GoblinWithKnife", 10),
                new MonsterSpawnData("GoblinWithBow", 10),
            }, true, "Orc_Shaman")
        })
    };
}