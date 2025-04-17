using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム内イベント（セリフ・演出・分岐など）をデータとして表現するクラス
/// </summary>
public enum EventType
{
    InsideGameDialogue,
    OutsideGameDialogue,
    StandingFade,    // 立ち絵フェード
    ScreenFade,      // 画面全体フェード
    // SetActive,       // オブジェクトSetActive切り替え
}

[System.Serializable]
public class EventData
{
    public string Id;
    public EventType Type; // 内側/外側どちらのセリフか
    public string InsideName; // 内側ゲームの話者名（例: ノワ、他キャラ追加時もここに）
    [TextArea]
    public string InsideDialogueText; // 内側ゲームのセリフ本文
    [TextArea]
    public string OutsideDialogueText; // 外側ゲームのセリフ本文（コッペ専用）
    public Sprite StandingImage; // 立ち絵（任意）

    public StandingFadeType StandingFade = StandingFadeType.None; // 立ち絵フェード
    public float StandingFadeDuration = 0.3f;

    public ScreenFadeType ScreenFade = ScreenFadeType.None; // 画面全体フェード
    public Color ScreenFadeColor = Color.black;
    public float ScreenFadeDuration = 0.5f;

    // public List<GameObject> SetActiveObjects = new List<GameObject>(); // ONにする対象
    // public List<GameObject> SetDeactiveObjects = new List<GameObject>(); // OFFにする対象
}

public enum StandingFadeType
{
    None,
    FadeIn,
    FadeOut
}

public enum ScreenFadeType
{
    None,
    FadeIn,
    FadeOut
}
