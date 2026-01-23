using UnityEngine;
using FirstGearGames.SmoothCameraShaker;
using System.Collections.Generic;

public enum ShakeType
{
    None,
    FootStep,
    Shooting,
    Explosion,
    Blow
}

[System.Serializable]
public class ShakeMapping
{
    public ShakeType Type;
    public ShakeData Data;
}

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance;

    [SerializeField]
    private ShakeMapping[] m_shakeMappings;
    private Dictionary<ShakeType, ShakeData> m_shakeDic;

    private void Awake()
    {
        Instance = this;

        m_shakeDic = new Dictionary<ShakeType, ShakeData>();
        foreach (var map in m_shakeMappings)
        {
            if (!m_shakeDic.ContainsKey(map.Type))
                m_shakeDic.Add(map.Type, map.Data);
        }
    }

    public ShakeData GetShkeData(ShakeType type) => m_shakeDic.TryGetValue(type, out ShakeData data) ? data : null;

    public void Shake(ShakeType type)
    {
        ShakeData _shakeData = GetShkeData(type);
        if (_shakeData == null) return;
        CameraShakerHandler.Shake(_shakeData);
    }
}
