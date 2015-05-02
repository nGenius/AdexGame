using System.Collections.Generic;

public class Skill
{
    public int id;
    public int attackableRange;
    public int attackRange;
    public string animationName;
    public string projectileResource;
}

public enum ElementType
{

}

public enum SkillType
{

}

public class JobClass
{
    private string name;
    private ElementType elementType;
    private SkillType skillType;
    private int jobPoint;
    private List<Skill> skills = new List<Skill>(); 
}