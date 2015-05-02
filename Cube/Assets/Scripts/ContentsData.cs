using UnityEngine;

public class ContentsData
{
    //private string strFilePath = Application.dataPath + "/StreamingAssets" + "/SkillData.xml";
    public readonly  SkillCollection SkillCollection = new SkillCollection();

    public void LoadData()
    {
        LoadSkill();
    }

    private void LoadSkill()
    {
        SkillCollection.LoadFile();
    }
}