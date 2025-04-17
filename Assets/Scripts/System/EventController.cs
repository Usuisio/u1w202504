using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベント進行・切り替え・セリフ表示指示を担当するコントローラー
/// </summary>
public class EventController : MonoBehaviour
{
    [SerializeField] private DialogueWindow dialogueWindow;

    // 仮のイベントグループリスト（1クリックで順に再生されるイベントのセット）
    private List<List<EventData>> eventGroups = new List<List<EventData>>();
    private int currentGroupIndex = 0;

    private void Start()
    {
        // 仮データセットアップ（1クリックで外→内の順に再生）
        eventGroups.Add(new List<EventData> {
            new EventData
            {
                Id = "event001b",
                Type = "OutsideGameDialogue",
                Character = "コッペ",
                Text = "やっほー！"
            },
            new EventData
            {
                Id = "event001a",
                Type = "InsideGameDialogue",
                Character = "ノワ",
                Text = "こんにちは、プレイヤーさん！"
            }
        });
        eventGroups.Add(new List<EventData> {
            new EventData
            {
                Id = "event002b",
                Type = "OutsideGameDialogue",
                Character = "コッペ",
                Text = "ノワ、今日も元気そうだね。"
            },
            new EventData
            {
                Id = "event002a",
                Type = "InsideGameDialogue",
                Character = "ノワ",
                Text = "今日はどんな一日だった？"
            }
        });
eventGroups.Add(new List<EventData> {
            new EventData
            {
                Id = "event002a",
                Type = "InsideGameDialogue",
                Character = "ノワ",
                Text = "楽しかった！"
            }
        });

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
        var group = eventGroups[currentGroupIndex];
        PlayEventSequence(group, 0);
    }

    // group内のeventListを順番に再生
    private void PlayEventSequence(List<EventData> group, int idx)
    {
        if (idx >= group.Count) return;
        var ev = group[idx];
        if (ev.Type == "InsideGameDialogue")
        {
            dialogueWindow.ShowNowa(ev.Text, () => PlayEventSequence(group, idx + 1));
        }
        else if (ev.Type == "OutsideGameDialogue")
        {
            dialogueWindow.ShowKoppe(ev.Text, () => PlayEventSequence(group, idx + 1));
        }
        else
        {
            PlayEventSequence(group, idx + 1);
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
