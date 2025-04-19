using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectEntry
{
    public string key;
    public GameObject target;
}

/// <summary>
/// Inspectorで紐づけたキー名→GameObjectの辞書でSetActiveを制御するマネージャ
/// </summary>
public class SetActiveManager : MonoBehaviour
{
    [SerializeField]
    private List<ObjectEntry> objectEntries = new List<ObjectEntry>();

    private Dictionary<string, GameObject> objectDict;

    private void Awake()
    {
        objectDict = new Dictionary<string, GameObject>();
        foreach (var entry in objectEntries)
        {
            if (!string.IsNullOrEmpty(entry.key) && entry.target != null)
            {
                objectDict[entry.key] = entry.target;
            }
        }
    }

    public void SetActiveByKey(string key, bool active)
    {
        if (objectDict.TryGetValue(key, out var obj) && obj != null)
        {
            obj.SetActive(active);
        }
        else
        {
            Debug.LogWarning($"SetActiveManager: key '{key}' のオブジェクトが見つかりません");
        }
    }
}
