using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 간단한 오브젝트 풀 매니저 (싱글턴)
/// - 프리팹별로 List를 관리합니다.
/// - Spawn(prefab, pos, rot) / Despawn(go)를 제공합니다.
/// </summary>
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private readonly Dictionary<GameObject, List<GameObject>> _pools = new();
    private readonly Dictionary<GameObject, GameObject> _instanceToPrefab = new();
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return null;

        if (!_pools.TryGetValue(prefab, out var list))
        {
            list = new List<GameObject>();
            _pools[prefab] = list;
        }

        GameObject go = null;

        // 리스트에서 재사용 가능한 오브젝트 찾기
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].activeSelf)
            {
                go = list[i];
                go.transform.SetPositionAndRotation(position, rotation);
                go.SetActive(true);
                break;
            }
        }

        // 못 찾으면 새로 생성
        if (go == null)
        {
            go = Instantiate(prefab, position, rotation,transform);
            _instanceToPrefab[go] = prefab;
            list.Add(go);
        }

        return go;
    }

    public void Despawn(GameObject go)
    {
        if (go == null) return;

        if (!_instanceToPrefab.ContainsKey(go))
        {
            Destroy(go); // 풀 관리 외부 생성물
            return;
        }

        go.SetActive(false);
    }
}