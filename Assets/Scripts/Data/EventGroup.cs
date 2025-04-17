using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 複数のEventData（セリフやイベント）をまとめて管理するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "EventGroup", menuName = "Game/Event Group")]
public class EventGroup : ScriptableObject
{
    public List<EventData> events = new List<EventData>();
}
