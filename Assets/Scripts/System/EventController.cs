using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベント進行・切り替え・セリフ表示指示を担当するコントローラー
/// </summary>
public class EventController : MonoBehaviour
{
    [SerializeField] private DialogueWindow dialogueWindow;

    // ScriptableObjectで管理するイベントグループリスト
    [SerializeField]
    private List<EventGroup> eventGroups = new List<EventGroup>();
    private int currentGroupIndex = 0;

    private void Start()
    {
        PlayCurrentGroup();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceGroup();
        }
    }

    private void PlayCurrentGroup()
    {
        if (currentGroupIndex < 0 || currentGroupIndex >= eventGroups.Count) return;
        var groupAsset = eventGroups[currentGroupIndex];
        if (groupAsset == null || groupAsset.events == null || groupAsset.events.Count == 0) return;
        PlayEventSequence(groupAsset.events, 0);
    }

    // group内のeventListを順番に再生
    private void PlayEventSequence(List<EventData> group, int idx)
    {
        if (idx >= group.Count) return;
        var ev = group[idx];
        switch (ev.Type)
        {
            case EventType.InsideGameDialogue:
                dialogueWindow.ShowInsideDialogue(
                    ev.InsideName,
                    ev.InsideDialogueText,
                    ev.StandingImage,
                    StandingFadeType.None,
                    0.3f,
                    ScreenFadeType.None,
                    Color.black,
                    0.5f,
                    () => PlayEventSequence(group, idx + 1)
                );
                break;
            case EventType.OutsideGameDialogue:
                dialogueWindow.ShowOutsideDialogue(ev.OutsideDialogueText, ev.StandingImage, () => PlayEventSequence(group, idx + 1));
                break;
            case EventType.StandingFade:
                dialogueWindow.ShowInsideDialogue(
                    null, null, ev.StandingImage,
                    ev.StandingFade,
                    ev.StandingFadeDuration,
                    ScreenFadeType.None,
                    Color.black,
                    0.5f,
                    () => PlayEventSequence(group, idx + 1)
                );
                break;
            case EventType.ScreenFade:
                dialogueWindow.ShowInsideDialogue(
                    null, null, null,
                    StandingFadeType.None,
                    0.3f,
                    ev.ScreenFade,
                    ev.ScreenFadeColor,
                    ev.ScreenFadeDuration,
                    () => PlayEventSequence(group, idx + 1)
                );
                break;
            // SetActiveイベントは削除
            default:
                PlayEventSequence(group, idx + 1);
                break;
        }
    }

    private void AdvanceGroup()
    {
        currentGroupIndex++;
        if (currentGroupIndex < eventGroups.Count)
        {
            PlayCurrentGroup();
        }
        // 末尾なら何もしない
    }
}
