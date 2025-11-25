using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 간단한 오브젝트 풀 매니저
/// </summary>
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    // prefab → 인스턴스 목록
    private readonly Dictionary<GameObject, List<GameObject>> _pools = new();
    // 인스턴스 → prefab
    private readonly Dictionary<GameObject, GameObject> _instanceToPrefab = new();

    [Header("미리 풀 생성할 프리팹 목록")]
    public List<GameObject> preloadPrefabs;

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

    void Start()
    {
        foreach (var prefab in preloadPrefabs)
        {
            // prefab을 key로 하는 풀 생성
            /*
             * _pools는 프리팹 하나당 하나의 리스트만 생성됩니다.

                즉, 같은 종류의 프리팹(FireBall)이 여러 개 있어도 Dictionary의 키는 FireBall 하나입니다.

                그래서 _pools[FireBall] → 하나의 리스트에 FireBall 오브젝트들이 모두 들어갑니다.

                SmallExplosion도 마찬가지로 하나의 리스트에 6개 오브젝트가 들어갑니다.
             */
            if (!_pools.ContainsKey(prefab))
                _pools[prefab] = new List<GameObject>();
            
            var obj = Instantiate(prefab, transform);
            obj.SetActive(false);

            _pools[prefab].Add(obj);
            _instanceToPrefab[obj] = prefab;
        }
    }

    /// <summary>
    /// 오브젝트풀로 사용하고자 할때 사용
    /// </summary>
    /// <param name="prefab">소환하고자하는 프리펩(키값 맞추기용)</param>
    /// <param name="position">위치</param>
    /// <param name="rotation">각도</param>
    /// <returns></returns>
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return null;

        if (!_pools.TryGetValue(prefab, out var list))
        {
            list = new List<GameObject>();
            _pools[prefab] = list;
        }

        GameObject go = null;

        // 비활성화 오브젝트 재사용
        foreach (var obj in list)
        {
            if (!obj.activeSelf)
            {
                go = obj;
                break;
            }
        }

        // 없으면 새로 생성
        if (go == null)
        {
            go = Instantiate(prefab, transform);
            list.Add(go);
            _instanceToPrefab[go] = prefab;
        }

        go.transform.SetPositionAndRotation(position, rotation);
        go.SetActive(true);
        return go;
    }

    /// <summary>
    /// 다쓴 오브젝트들 해제하고자 할때 사용
    /// </summary>
    /// <param name="go">해제하고자하는 오브젝트</param>
    public void Despawn(GameObject go)
    {
        if (go == null) return;

        if (!_instanceToPrefab.ContainsKey(go))
        {
            Destroy(go);
            return;
        }

        go.SetActive(false);
        go.transform.SetParent(transform);
    }
}
