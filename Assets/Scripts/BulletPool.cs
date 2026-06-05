using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    static BulletPool _instance;
    public static BulletPool Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("BulletPool");
                _instance = go.AddComponent<BulletPool>();
            }
            return _instance;
        }
    }

    // Map prefab -> queue of pooled instances
    private readonly Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return null;

        Queue<GameObject> pool;
        if (!pools.TryGetValue(prefab, out pool))
        {
            pool = new Queue<GameObject>();
            pools[prefab] = pool;
        }

        GameObject obj = null;
        while (pool.Count > 0)
        {
            var candidate = pool.Dequeue();
            if (candidate != null)
            {
                obj = candidate;
                break;
            }
        }

        if (obj == null)
        {
            obj = Instantiate(prefab, position, rotation);
            var meta = obj.GetComponent<PooledObject>();
            if (meta == null) meta = obj.AddComponent<PooledObject>();
            meta.prefab = prefab;
            obj.SetActive(true);
        }
        else
        {
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
        }

        obj.transform.SetParent(this.transform, true);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj == null) return;
        var meta = obj.GetComponent<PooledObject>();
        if (meta == null || meta.prefab == null)
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        Queue<GameObject> pool;
        if (!pools.TryGetValue(meta.prefab, out pool))
        {
            pool = new Queue<GameObject>();
            pools[meta.prefab] = pool;
        }
        pool.Enqueue(obj);
    }
}

public class PooledObject : MonoBehaviour
{
    public GameObject prefab;
}
