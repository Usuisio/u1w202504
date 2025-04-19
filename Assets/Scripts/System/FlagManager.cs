using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フラグ（string→bool）を動的に管理するクラス
/// </summary>
[System.Serializable]
public class FlagEntry
{
    public string key;
    public bool value;
}

public class FlagManager : MonoBehaviour
{
    private Dictionary<string, bool> flags = new Dictionary<string, bool>();

    [SerializeField]
    private List<FlagEntry> debugFlags = new List<FlagEntry>();

    /// <summary>
    /// フラグをtrueにする
    /// </summary>
    public void SetFlag(string flagName)
    {
        if (!string.IsNullOrEmpty(flagName))
        {
            flags[flagName] = true;
            UpdateDebugFlags();
        }
    }

    /// <summary>
    /// フラグをfalseにする
    /// </summary>
    public void ResetFlag(string flagName)
    {
        if (!string.IsNullOrEmpty(flagName))
        {
            flags[flagName] = false;
            UpdateDebugFlags();
        }
    }

    /// <summary>
    /// フラグがtrueかどうか
    /// </summary>
    public bool HasFlag(string flagName)
    {
        if (string.IsNullOrEmpty(flagName)) return false;
        return flags.ContainsKey(flagName) && flags[flagName];
    }

    /// <summary>
    /// フラグの値を取得（未登録ならfalse）
    /// </summary>
    public bool GetFlag(string flagName)
    {
        if (string.IsNullOrEmpty(flagName)) return false;
        return flags.TryGetValue(flagName, out var value) && value;
    }

    /// <summary>
    /// 全フラグをリセット
    /// </summary>
    public void ResetAll()
    {
        flags.Clear();
        UpdateDebugFlags();
    }

    /// <summary>
    /// flags辞書の内容をdebugFlagsリストに同期
    /// </summary>
    private void UpdateDebugFlags()
    {
        debugFlags.Clear();
        foreach (var kv in flags)
        {
            debugFlags.Add(new FlagEntry { key = kv.Key, value = kv.Value });
        }
    }
}
