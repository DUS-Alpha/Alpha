using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public abstract class ItemData : ScriptableObject
{
    [Header("[ Item Info ]"), Space(10)]
    public string Description;
    public GameObject ItemPrefab;
    public string ID;
    public string Name;
    public Sprite Icon;
    public string Rating;
}
