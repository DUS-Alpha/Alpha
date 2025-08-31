using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData m_Data { get; protected set; }

    public virtual void Initialize(ItemData data)
    {
        m_Data = data;
    }

    public virtual void Use(GameObject user) { }
}
