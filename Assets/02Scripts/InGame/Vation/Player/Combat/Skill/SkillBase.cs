using UnityEngine;

public abstract class SkillBase : ScriptableObject, ISkillStrategy
{
    protected SkillDataBase m_Data;
    protected PlayerCore m_Core;
    protected float m_CoolDownTimer;

    public bool CanExecute
    {
        get { return Time.time >= m_CoolDownTimer; }
    }

    public void Initialize(PlayerCore core, SkillDataBase data)
    {
        m_Core = core;
        m_Data = data;
    }

    public virtual void Execute()
    {
        if(!CanExecute) return;
        m_CoolDownTimer = Time.time + m_Data.CoolDown;

    }
}
