using UnityEngine;

public abstract class SkillBase : ScriptableObject, ISkill
{

    private float m_nextReadyTime;

    public bool IsReady
    {
        get { return Time.time >= m_nextReadyTime; }
    }
    public void ExecuteSkill()
    {
        if(!IsReady)
        {

            return;
        }
        //m_nextReadyTime = Time.time + m_coolDown;
    }

    protected abstract void OnCast();
}
