using System.Collections.Generic;
using System.Xml;
using Hunter.Tool;
using UnityEditor;
using UnityEngine;

public class SkillWindow : EditorWindow
{
    private string strFilePath = Application.dataPath + "/StreamingAssets" + "/SkillData.xml";
    private List<Skill> skills = new List<Skill>();

    [MenuItem("Tactics/Skill")]
    public static void ShowWindow()
    {
        SkillWindow window = (SkillWindow) EditorWindow.GetWindow(typeof (SkillWindow));
        window.position = new Rect(100, 100, 1000, 500);
    }

    private void OnGUI()
    {
        DrawTopButtons();
        DrawCategory();
        DrawSkills();
    }

    private void DrawTopButtons()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Load", CustomGUI.GetFixedLayoutSize(100, 25)))
        {
            LoadFile();
        }

        if (GUILayout.Button("Save", CustomGUI.GetFixedLayoutSize(100, 25)))
        {
            SaveFile();
        }

        if (GUILayout.Button("Add New Skill", CustomGUI.GetFixedLayoutSize(100, 25)))
        {
            AddNewSkill();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void LoadFile()
    {
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

    private void SaveFile()
    {
        
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

    private void AddNewSkill()
    {
        Skill skill = new Skill();
        int checkID = 1;
        while (skills.Find(x => x.id == checkID) != null)
        {
            ++checkID;
        }

        skill.id = checkID;

        skills.Add(skill);
    }

    private void DrawCategory()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("ID", CustomGUI.GetFixedLayoutSize(100, 20));
        GUILayout.Label("AttackableRange", CustomGUI.GetFixedLayoutSize(100, 20));
        GUILayout.Label("AttackRange", CustomGUI.GetFixedLayoutSize(100, 20));
        GUILayout.Label("AnimationName", CustomGUI.GetFixedLayoutSize(150, 20));
        GUILayout.Label("ProjectileResource", CustomGUI.GetFixedLayoutSize(150, 20));
        EditorGUILayout.EndHorizontal();
    }

    private void DrawSkills()
    {
        EditorGUILayout.BeginVertical();
        foreach (Skill skill in skills)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(skill.id.ToString(), CustomGUI.GetFixedLayoutSize(100, 20));

            skill.attackableRange 
                = EditorGUILayout.IntField(skill.attackableRange, CustomGUI.GetFixedLayoutSize(100, 20));

            skill.attackRange 
                = EditorGUILayout.IntField(skill.attackRange, CustomGUI.GetFixedLayoutSize(100, 20));

            skill.animationName =
                EditorGUILayout.TextField(skill.animationName, CustomGUI.GetFixedLayoutSize(150, 20));

            skill.projectileResource =
                EditorGUILayout.TextField(skill.projectileResource, CustomGUI.GetFixedLayoutSize(150, 20));

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

}