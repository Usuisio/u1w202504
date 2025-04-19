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

    // 入力受付フラグ
    [SerializeField] private bool isInputEnabled = true;

    // 直前の立ち絵・話者名
    private Sprite lastStandingSprite = null;
    private string lastSpeakerName = "";

    // BGM再生用AudioSource
    [SerializeField] private AudioSource bgmAudioSource;

    private void Start()
    {
        // Resources/StoryEvent.csv を読み込む（Resources/StoryEvent というパスになる想定）
        storyEvents = StoryEventCsvLoader.Load("StoryEvent");
        currentEventIndex = 0;
        if (storyEvents == null || storyEvents.Count == 0)
        {
            Debug.LogError("StoryEvent.csvの読み込みに失敗、またはイベントが0件です。パス・カラム数・内容を確認してください。");
        }
        // BGM AudioSourceが未設定なら自動生成
        if (bgmAudioSource == null)
        {
            var go = new GameObject("BGM_AudioSource");
            go.transform.SetParent(this.transform);
            bgmAudioSource = go.AddComponent<AudioSource>();
            bgmAudioSource.loop = true;
        }
        PlayCurrentEvent();
    }

    private void Update()
    {
        if (!isInputEnabled) return;
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
                Sprite standingSpriteSay = null;
                if (!string.IsNullOrEmpty(ev.args[0]))
                {
                    var s = Resources.Load<Sprite>(ev.args[0]);
                    if (s != null)
                    {
                        standingSpriteSay = s;
                        lastStandingSprite = s;
                    }
                }
                // arg1が空の場合はstandingSpriteSay=null, lastStandingSpriteも更新しない
                // 話者名
                string speakerName = lastSpeakerName;
                if (!string.IsNullOrEmpty(ev.args[1]))
                {
                    lastSpeakerName = ev.args[1];
                    speakerName = ev.args[1];
                }
                else
                {
                    lastSpeakerName = "";
                    speakerName = "";
                }
                dialogueWindow.ShowInsideDialogue(
                    speakerName,
                    ev.content.Replace("\\n", "\n"),
                    ev.isNeedClick,
                    OnEventFinished
                );
                // 立ち絵・画面フェード演出
                dialogueWindow.PlayStandingEffect(standingSpriteSay, StandingFadeType.None, 0.3f);
                dialogueWindow.PlayScreenFade(ScreenFadeType.None, UnityEngine.Color.black, 0.5f);
                break;
            case "outsideSay":
                // arg1: 立ち絵ファイル名
                // content: セリフ
                Sprite standingSpriteOut = null;
                if (!string.IsNullOrEmpty(ev.args[0]))
                {
                    var s = Resources.Load<Sprite>(ev.args[0]);
                    if (s != null)
                    {
                        standingSpriteOut = s;
                        lastStandingSprite = s;
                    }
                }
                // arg1が空の場合はstandingSpriteOut=null, lastStandingSpriteも更新しない
                dialogueWindow.ShowOutsideDialogue(
                    ev.content.Replace("\\n", "\n"),
                    ev.isNeedClick,
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
                    null, null, ev.isNeedClick,
                    !ev.isNeedClick ? OnEventFinished : null
                );
                dialogueWindow.PlayStandingEffect(null, StandingFadeType.None, 0.3f);
                dialogueWindow.PlayScreenFade(fadeType, UnityEngine.Color.black, fadeDuration);
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
                    null, null, ev.isNeedClick,
                    !ev.isNeedClick ? OnEventFinished : null
                );
                dialogueWindow.PlayStandingEffect(standingSprite, standingFade, standingFadeDuration);
                dialogueWindow.PlayScreenFade(ScreenFadeType.None, UnityEngine.Color.black, 0.5f);
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
                    isInputEnabled = false; // 選択肢表示中は入力無効
                    dialogueWindow.ShowChoice(choices, (selectedIdx) =>
                    {
                        dialogueWindow.HideChoices();
                        isInputEnabled = true; // 選択肢を閉じたら入力再開
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
            case "bgmPlay":
                // content: BGMファイル名（Resources/に配置）, arg1: ボリューム(0-1, 省略可)
                if (!string.IsNullOrEmpty(ev.content))
                {
                    var clip = Resources.Load<AudioClip>(ev.content);
                    if (clip != null)
                    {
                        bgmAudioSource.clip = clip;
                        float vol = 1.0f;
                        if (ev.args.Length > 0) float.TryParse(ev.args[0], out vol);
                        bgmAudioSource.volume = Mathf.Clamp01(vol);
                        bgmAudioSource.Play();
                    }
                    else
                    {
                        Debug.LogWarning($"bgmPlay: BGMファイルが見つかりません: {ev.content}");
                    }
                }
                else
                {
                    Debug.LogWarning("bgmPlay: content(BGMファイル名)が空です");
                }
                OnEventFinished();
                break;
            case "bgmFadeOut":
                // arg1: フェードアウト秒数
                float fadeSec = 1.0f;
                if (ev.args.Length > 0) float.TryParse(ev.args[0], out fadeSec);
                StartCoroutine(BgmFadeOutCoroutine(fadeSec));
                break;
            case "wait":
                // arg1: 待機秒数
                float waitSec = 1.0f;
                float.TryParse(ev.args[0], out waitSec);
                StartCoroutine(WaitAndFinish(waitSec));
                break;
            // 他typeも必要に応じて追加
            case "OutsideHide":
                if (dialogueWindow != null)
                {
                    dialogueWindow.partsController.HideOutsideDialogueImage();
                }
                OnEventFinished();
                break;
            case "InsideHide":
                if (dialogueWindow != null)
                {
                    dialogueWindow.partsController.HideInsideDialogueImage();
                }
                OnEventFinished();
                break;
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

    private System.Collections.IEnumerator BgmFadeOutCoroutine(float fadeSec)
    {
        if (bgmAudioSource == null || !bgmAudioSource.isPlaying)
        {
            OnEventFinished();
            yield break;
        }
        float startVol = bgmAudioSource.volume;
        float t = 0f;
        while (t < fadeSec)
        {
            t += Time.deltaTime;
            bgmAudioSource.volume = Mathf.Lerp(startVol, 0f, t / fadeSec);
            yield return null;
        }
        bgmAudioSource.Stop();
        bgmAudioSource.volume = startVol;
        OnEventFinished();
    }
}
