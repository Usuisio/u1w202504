using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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

/// <summary>
/// テキスト表示＆テキスト送りを管理するクラス
/// </summary>
public class DialogueWindow : MonoBehaviour
{
    [Header("演出プレイヤー")]
    [SerializeField] private DialogueEffectPlayer effectPlayer;

    [Header("外側ゲームの立ち絵")]
    [SerializeField] private Image outsideStandingImage;

    [Header("内側ゲームのセリフ本文")]
    [SerializeField] private TextMeshProUGUI insideDialogueText;

    [Header("外側ゲームのセリフ本文")]
    [SerializeField] private TextMeshProUGUI outsideDialogueText;

    [Header("内側ゲームの話者名")]
    [SerializeField] private TextMeshProUGUI insideNameText;

    [Header("テキスト表示速度（1文字あたり秒数）")]
    [SerializeField, Tooltip("1文字あたり何秒かけて表示するか（例: 0.05）")]
    private float charInterval = 0.05f;

    private DG.Tweening.Tween insideDialogueTween;
    private DG.Tweening.Tween outsideDialogueTween;

    /// <summary>
    /// 内側ゲームのセリフと話者名を一文字ずつ表示
    /// </summary>
    /// <param name="name">話者名</param>
    /// <param name="text">表示するテキスト</param>
    public void ShowInsideDialogue(
        string name,
        string text,
        Sprite standingImage = null,
        StandingFadeType standingFade = StandingFadeType.None,
        float standingFadeDuration = 0.3f,
        ScreenFadeType screenFade = ScreenFadeType.None,
        Color? screenFadeColor = null,
        float screenFadeDuration = 0.5f,
        System.Action onComplete = null)
    {
        // 話者名
        if (insideNameText != null)
        {
            insideNameText.text = name ?? "";
        }
        // セリフ本文
        if (insideDialogueText != null)
        {
            if (insideDialogueTween != null) insideDialogueTween.Kill();
            insideDialogueText.text = "";
            float duration = charInterval * (text?.Length ?? 0);
            insideDialogueTween = insideDialogueText.DOText(text ?? "", duration).SetEase(DG.Tweening.Ease.Linear).OnComplete(() => { if (onComplete != null) onComplete(); });
        }
        // 立ち絵・画面フェード演出
        if (effectPlayer != null)
        {
            effectPlayer.PlayStandingEffect(standingImage, standingFade, standingFadeDuration);
            if (screenFade != ScreenFadeType.None)
            {
                effectPlayer.PlayScreenFade(screenFade, screenFadeColor ?? Color.black, screenFadeDuration);
            }
        }
    }

    /// <summary>
    /// 外側ゲーム（コッペ）のセリフを一文字ずつ表示
    /// </summary>
    /// <param name="text">表示するテキスト</param>
    public void ShowOutsideDialogue(string text, Sprite standingImage = null, System.Action onComplete = null)
    {
        if (outsideDialogueText != null)
        {
            if (outsideDialogueTween != null) outsideDialogueTween.Kill();
            outsideDialogueText.text = "";
            float duration = charInterval * (text?.Length ?? 0);
            outsideDialogueTween = outsideDialogueText.DOText(text ?? "", duration).SetEase(DG.Tweening.Ease.Linear).OnComplete(() => { if (onComplete != null) onComplete(); });
        }
        if (effectPlayer != null)
        {
            effectPlayer.PlayOutsideStandingEffect(standingImage);
        }
    }
}
