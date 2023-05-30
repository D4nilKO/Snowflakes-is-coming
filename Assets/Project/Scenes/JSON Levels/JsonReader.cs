using UnityEngine;
using System;

public class JsonReader : MonoBehaviour
{
    public TextAsset textJSON;

    [Serializable]
    public class SkillUpgrade
    {
        public int lvl;
        public float timeBtwSpawns;
        public int countOfStrikes;
        public float damage;
        public string upgradeText;
    }

    [Serializable]
    public class SkillUpgradeList

    {
        public SkillUpgrade[] skillUpgrade;
    }

    public SkillUpgradeList mySkillUpgradeList = new SkillUpgradeList();

    public void ReadJson()
    {
        mySkillUpgradeList = JsonUtility.FromJson<SkillUpgradeList>(textJSON.text);
    }
}