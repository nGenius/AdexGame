using System.Collections.Generic;

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