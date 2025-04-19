using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベント進行・切り替え・セリフ表示指示を担当するコントローラー
/// </summary>
public class EventController : MonoBehaviour
{
    [SerializeField] private DialogueWindow dialogueWindow;
    [SerializeField] private SetActiveManager setActiveManager;
    [SerializeField] private FlagManager flagManager;

    // CSVから読み込んだストーリーイベントリスト
    private List<StoryEventCsvLoader.StoryEventRow> storyEvents = new List<StoryEventCsvLoader.StoryEventRow>();
    private int currentEventIndex = 0;

    // 直前の立ち絵・話者名
    private Sprite lastStandingSprite = null;
    private string lastSpeakerName = "";

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
                // 立ち絵
                Sprite standingSpriteSay = lastStandingSprite;
                if (!string.IsNullOrEmpty(ev.args[0]))
                {
                    var s = Resources.Load<Sprite>(ev.args[0]);
                    if (s != null) lastStandingSprite = s;
                    standingSpriteSay = s ?? lastStandingSprite;
                }
                // 話者名
                string speakerName = lastSpeakerName;
                if (!string.IsNullOrEmpty(ev.args[1]))
                {
                    lastSpeakerName = ev.args[1];
                    speakerName = ev.args[1];
                }
                dialogueWindow.ShowInsideDialogue(
                    speakerName,
                    ev.content.Replace("\\n", "\n"),
                    standingSpriteSay,
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
                Sprite standingSpriteOut = lastStandingSprite;
                if (!string.IsNullOrEmpty(ev.args[0]))
                {
                    var s = Resources.Load<Sprite>(ev.args[0]);
                    if (s != null) lastStandingSprite = s;
                    standingSpriteOut = s ?? lastStandingSprite;
                }
                dialogueWindow.ShowOutsideDialogue(
                    ev.content.Replace("\\n", "\n"),
                    standingSpriteOut,
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
            case "flag":
                // content: フラグ名
                if (flagManager != null && !string.IsNullOrEmpty(ev.content))
                {
                    flagManager.SetFlag(ev.content);
                }
                else
                {
                    Debug.LogWarning($"flag: content(フラグ名)が空、またはFlagManager未設定");
                }
                OnEventFinished();
                break;
            case "ifgoto":
                // content: ジャンプ先id, arg1: フラグ名
                if (flagManager != null && !string.IsNullOrEmpty(ev.content) && !string.IsNullOrEmpty(ev.args[0]))
                {
                    if (flagManager.HasFlag(ev.args[0]))
                    {
                        // ジャンプ先idを探す
                        int jumpId = 0;
                        int.TryParse(ev.content, out jumpId);
                        int idx = storyEvents.FindIndex(e => e.id == jumpId);
                        if (idx >= 0)
                        {
                            currentEventIndex = idx;
                            PlayCurrentEvent();
                            return;
                        }
                        else
                        {
                            Debug.LogWarning($"ifgoto: ジャンプ先id {jumpId} が見つかりません");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"ifgoto: content(ジャンプ先id)またはarg1(フラグ名)が空、またはFlagManager未設定");
                }
                OnEventFinished();
                break;
            case "choice":
                // arg1,2,3,4,5,6: テキスト,ジャンプ先idペア
                var choices = new List<string>();
                var jumpIds = new List<int>();
                for (int i = 0; i < 6; i += 2)
                {
                    string text = ev.args[i];
                    string idStr = (i + 1 < ev.args.Length) ? ev.args[i + 1] : "";
                    if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int jumpId))
                    {
                        choices.Add(text);
                        jumpIds.Add(jumpId);
                    }
                }
                if (choices.Count > 0)
                {
                    dialogueWindow.ShowChoice(choices, (selectedIdx) =>
                    {
                        dialogueWindow.HideChoices();
                        if (selectedIdx >= 0 && selectedIdx < jumpIds.Count)
                        {
                            int idx = storyEvents.FindIndex(e => e.id == jumpIds[selectedIdx]);
                            if (idx >= 0)
                            {
                                currentEventIndex = idx;
                                PlayCurrentEvent();
                                return;
                            }
                        }
                        OnEventFinished();
                    });
                }
                else
                {
                    Debug.LogWarning("choice: 有効な選択肢がありません");
                    OnEventFinished();
                }
                break;
            case "wait":
                // arg1: 待機秒数
                float waitSec = 1.0f;
                float.TryParse(ev.args[0], out waitSec);
                StartCoroutine(WaitAndFinish(waitSec));
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

    private System.Collections.IEnumerator WaitAndFinish(float sec)
    {
        yield return new WaitForSeconds(sec);
        OnEventFinished();
    }
}
