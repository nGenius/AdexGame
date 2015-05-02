using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class SkillCollection
{
    private string strFilePath;
    public readonly List<Skill> skills = new List<Skill>();
    
    public void LoadFile()
    {
        strFilePath = Application.dataPath + "/StreamingAssets" + "/SkillData.xml";
        skills.Clear();
        XmlDocument doc = new XmlDocument();
        doc.Load(strFilePath);

        XmlNode rootNode = doc.DocumentElement;
        XmlNodeList skillNodes = rootNode.ChildNodes;

        foreach (XmlNode node in skillNodes)
        {
            Skill skill = new Skill();
            foreach (XmlAttribute attribute in node.Attributes)
            {
                if (attribute.Name.Equals("ID"))
                {
                    skill.id = System.Convert.ToInt16(attribute.Value);
                }

                else if (attribute.Name.Equals("AttackableRange"))
                {
                    skill.attackableRange = System.Convert.ToInt16(attribute.Value);
                }

                else if (attribute.Name.Equals("AttackRange"))
                {
                    skill.attackRange = System.Convert.ToInt16(attribute.Value);
                }

                else if (attribute.Name.Equals("AnimationName"))
                {
                    skill.animationName = attribute.Value;
                }

                else if (attribute.Name.Equals("ProjectileResource"))
                {
                    skill.projectileResource = attribute.Value;
                }
            }

            skills.Add(skill);
        }
    }

    public void SaveFile()
    {
        strFilePath = Application.dataPath + "/StreamingAssets" + "/SkillData.xml";
        XmlDocument doc = new XmlDocument();

        XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        doc.AppendChild(docNode);

        XmlNode rootNode = doc.CreateElement("SkillData");
        doc.AppendChild(rootNode);

        foreach (Skill skill in skills)
        {
            XmlNode node = doc.CreateElement("Skill");

            XmlAttribute attr = doc.CreateAttribute("ID");
            attr.Value = skill.id.ToString();
            node.Attributes.Append(attr);

            attr = doc.CreateAttribute("ID");
            attr.Value = skill.id.ToString();
            node.Attributes.Append(attr);

            attr = doc.CreateAttribute("AttackableRange");
            attr.Value = skill.attackableRange.ToString();
            node.Attributes.Append(attr);

            attr = doc.CreateAttribute("AttackRange");
            attr.Value = skill.attackRange.ToString();
            node.Attributes.Append(attr);

            attr = doc.CreateAttribute("AnimationName");
            attr.Value = skill.animationName;
            node.Attributes.Append(attr);

            attr = doc.CreateAttribute("ProjectileResource");
            attr.Value = skill.projectileResource;
            node.Attributes.Append(attr);

            rootNode.AppendChild(node);
        }

        doc.Save(strFilePath);
    }
}

public class Skill
{
    public int id;
    public int attackableRange;
    public int attackRange;
    public string animationName;
    public string projectileResource;
}