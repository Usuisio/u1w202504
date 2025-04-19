using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベント進行・切り替え・セリフ表示指示を担当するコントローラー
/// </summary>
public class EventController : MonoBehaviour
{
    [SerializeField] private DialogueWindow dialogueWindow;
    [SerializeField] private SetActiveManager setActiveManager;

    // CSVから読み込んだストーリーイベントリスト
    private List<StoryEventCsvLoader.StoryEventRow> storyEvents = new List<StoryEventCsvLoader.StoryEventRow>();
    private int currentEventIndex = 0;

    private void Start()
    {
        // Resources/StoryEvent.csv を読み込む（Resources/StoryEvent というパスになる想定）
        storyEvents = StoryEventCsvLoader.Load("StoryEvent");
        currentEventIndex = 0;
        if (storyEvents == null || storyEvents.Count == 0)
        {
            Debug.LogError("StoryEvent.csvの読み込みに失敗、またはイベントが0件です。パス・カラム数・内容を確認してください。");
        }
        PlayCurrentEvent();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceEvent();
        }
    }

    /// <summary>
    /// 現在のイベントを再生
    /// </summary>
    private void PlayCurrentEvent()
    {
        if (currentEventIndex < 0 || currentEventIndex >= storyEvents.Count) return;
        var ev = storyEvents[currentEventIndex];

        // typeごとに分岐
        switch (ev.type)
        {
            case "insideSay":
                // arg1: 立ち絵ファイル名, arg2: 話者名
                // content: セリフ
                // TODO: SpriteManagerからスプライト取得
                dialogueWindow.ShowInsideDialogue(
                    ev.args[1], // 話者名
                    ev.content.Replace("\\n", "\n"),
                    null, // 立ち絵スプライト（仮）
                    StandingFadeType.None,
                    0.3f,
                    ScreenFadeType.None,
                    (Color?)UnityEngine.Color.black,
                    0.5f,
                    OnEventFinished
                );
                break;
            case "outsideSay":
                // arg1: 立ち絵ファイル名
                // content: セリフ
                dialogueWindow.ShowOutsideDialogue(
                    ev.content.Replace("\\n", "\n"),
                    null, // 立ち絵スプライト（仮）
                    OnEventFinished
                );
                break;
            case "screenfade":
                // content: 対象スクリーン名（未使用）, arg1: in/out, arg2: フェード時間
                var fadeType = ev.args[0] == "in" ? ScreenFadeType.FadeIn : ScreenFadeType.FadeOut;
                float fadeDuration = 0.5f;
                float.TryParse(ev.args[1], out fadeDuration);
                dialogueWindow.ShowInsideDialogue(
                    null, null, null,
                    StandingFadeType.None,
                    0.3f,
                    fadeType,
                    (Color?)UnityEngine.Color.black,
                    fadeDuration,
                    OnEventFinished
                );
                break;
            case "insideCharaFade":
                // content: 立ち絵ファイル名, arg1: in/out, arg2: フェード時間
                var standingFade = ev.args[0] == "in" ? StandingFadeType.FadeIn : StandingFadeType.FadeOut;
                float standingFadeDuration = 0.3f;
                float.TryParse(ev.args[1], out standingFadeDuration);
                Sprite standingSprite = null;
                if (!string.IsNullOrEmpty(ev.content))
                {
                    // 例: content="nowa1" → Resources/nowa1 からロード
                    standingSprite = Resources.Load<Sprite>(ev.content);
                    if (standingSprite == null)
                    {
                        Debug.LogWarning($"立ち絵スプライトが見つかりません: {ev.content}");
                    }
                }
                dialogueWindow.ShowInsideDialogue(
                    null, null, standingSprite,
                    standingFade,
                    standingFadeDuration,
                    ScreenFadeType.None,
                    (Color?)UnityEngine.Color.black,
                    0.5f,
                    OnEventFinished
                );
                break;
            case "setActive":
                // content: オブジェクトキー名, arg1: "true"/"false"
                bool active = false;
                bool.TryParse(ev.args[0], out active);
                if (setActiveManager != null && !string.IsNullOrEmpty(ev.content))
                {
                    setActiveManager.SetActiveByKey(ev.content, active);
                }
                else
                {
                    Debug.LogWarning($"setActive: content(キー名)が空、またはSetActiveManager未設定");
                }
                OnEventFinished();
                break;
            // 他typeも必要に応じて追加
            default:
                OnEventFinished();
                break;
        }
    }

    /// <summary>
    /// 次のイベントへ進む
    /// </summary>
    private void AdvanceEvent()
    {
        currentEventIndex++;
        if (currentEventIndex < storyEvents.Count)
        {
            PlayCurrentEvent();
        }
        // 末尾なら何もしない
    }

    /// <summary>
    /// イベント終了時コールバック
    /// </summary>
    private void OnEventFinished()
    {
        // isNeedClickがfalseなら自動でAdvanceEvent
        if (currentEventIndex < storyEvents.Count)
        {
            var ev = storyEvents[currentEventIndex];
            if (!ev.isNeedClick)
            {
                AdvanceEvent();
            }
        }
    }
}
