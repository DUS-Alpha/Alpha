using UnityEngine;
using UnityEngine.EventSystems;

public class Listener : MonoBehaviour
{
    [SerializeField]private Animator _animator;
    
    public void Settrigger()
    {

        string clicked = EventSystem.current.currentSelectedGameObject.name;
        _animator.SetTrigger(clicked);
    }
}
