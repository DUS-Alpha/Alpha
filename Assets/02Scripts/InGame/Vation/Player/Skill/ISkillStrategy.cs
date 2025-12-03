using UnityEngine;

public interface ISkillStrategy
{
    void Initialize(PlayerCore core, SkillDataBase data);
    public void Execute();
    public bool CanExecute { get; }
}
