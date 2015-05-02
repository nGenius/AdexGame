using Hunter.Tool;
using UnityEditor;
using UnityEngine;

public class SkillWindow : EditorWindow
{
    public SkillCollection skillCollection = new SkillCollection();
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
            skillCollection.LoadFile();
        }

        if (GUILayout.Button("Save", CustomGUI.GetFixedLayoutSize(100, 25)))
        {
            skillCollection.SaveFile();
        }

        if (GUILayout.Button("Add New Skill", CustomGUI.GetFixedLayoutSize(100, 25)))
        {
            AddNewSkill();
        }
        EditorGUILayout.EndHorizontal();
    }

  

    private void AddNewSkill()
    {
        Skill skill = new Skill();
        int checkID = 1;
        while (skillCollection.skills.Find(x => x.id == checkID) != null)
        {
            ++checkID;
        }

        skill.id = checkID;

        skillCollection.skills.Add(skill);
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
        foreach (Skill skill in skillCollection.skills)
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