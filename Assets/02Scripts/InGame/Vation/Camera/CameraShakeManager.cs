using UnityEngine;
using FirstGearGames.SmoothCameraShaker;
using System.Collections.Generic;

public enum ShakeType
{
    None,
    Shooting,
    FootStep,
    Explosion
}
[System.Serializable]
public class ShakeMapping
{
    public ShakeType type;
    public ShakeData data;
}

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance;

    [SerializeField]
    private ShakeMapping[] m_shakeMappings;
    public Dictionary<ShakeType, ShakeData> m_shakeDic;

    private void Awake()
    {
        Instance = this;

        m_shakeDic = new Dictionary<ShakeType, ShakeData>();
        foreach (var map in m_shakeMappings)
        {
            if (!m_shakeDic.ContainsKey(map.type))
                m_shakeDic.Add(map.type, map.data);
        }
    }

    public ShakeData GetShkeData(ShakeType type) => m_shakeDic.TryGetValue(type, out ShakeData data) ? data : null;

    public void Shake(ShakeType type)
    {
        if(GetShkeData(type) == null) return;
        CameraShakerHandler.Shake(GetShkeData(type));
    }
}
