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

    [Header("UI部品コントローラー")]
    [SerializeField] public DialogueWindowPartsController partsController;

    [Header("外ゲーム用ダイアログ本体のImage（背景や枠など）")]
    [SerializeField] private Image outsideDialogueImage;

    [Header("クリック待ち表示用オブジェクト（例：アイコン等）")]
    [SerializeField] private GameObject clickWaitIndicator;

    [Header("中ゲーム用ダイアログ本体のImage（背景や枠など）")]
    [SerializeField] private Image insideDialogueImage;

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

    [Header("選択肢ボタン（事前配置、最大3つなど）")]
    [SerializeField] private List<Button> choiceButtons = new List<Button>();

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
        bool isNeedClick = true,
        System.Action onComplete = null)
    {
        // ダイアログImageを再表示
        if (partsController != null)
        {
            partsController.ShowInsideDialogueImage();
        }

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

            // メッセージ送り中はインジケータ非表示
            if (partsController != null)
            {
                partsController.HideClickWaitIndicator();
            }

insideDialogueTween = insideDialogueText.DOText(text ?? "", duration).SetEase(DG.Tweening.Ease.Linear).OnComplete(() =>
{
    if (isNeedClick)
    {
        if (partsController != null)
        {
            partsController.ShowClickWaitIndicator();
        }
    }
    onComplete?.Invoke();
});
        }
    }

    /// <summary>
    /// 立ち絵のフェード演出を再生
    /// </summary>
    public void PlayStandingEffect(Sprite standingImage, StandingFadeType standingFade, float standingFadeDuration, System.Action onEffectComplete = null)
    {
        if (effectPlayer != null)
        {
            effectPlayer.PlayStandingEffect(standingImage, standingFade, standingFadeDuration, onEffectComplete);
        }
    }

    /// <summary>
    /// 画面フェード演出を再生
    /// </summary>
    public void PlayScreenFade(ScreenFadeType screenFade, Color screenFadeColor, float screenFadeDuration, System.Action onEffectComplete = null)
    {
        if (effectPlayer != null && screenFade != ScreenFadeType.None)
        {
            effectPlayer.PlayScreenFade(screenFade, screenFadeColor, screenFadeDuration, onEffectComplete);
        }
    }

    /// <summary>
    /// 外側ゲーム（コッペ）のセリフを一文字ずつ表示
    /// </summary>
    /// <param name="text">表示するテキスト</param>
    public void ShowOutsideDialogue(string text, bool isNeedClick = true, Sprite standingImage = null, System.Action onComplete = null)
    {
        // ダイアログImageを再表示
        if (partsController != null)
        {
            partsController.ShowOutsideDialogueImage();
        }

        if (outsideDialogueText != null)
        {
            if (outsideDialogueTween != null) outsideDialogueTween.Kill();
            outsideDialogueText.text = "";
            float duration = charInterval * (text?.Length ?? 0);

            // メッセージ送り中はインジケータ非表示
            if (partsController != null)
            {
                partsController.HideClickWaitIndicator();
            }

outsideDialogueTween = outsideDialogueText.DOText(text ?? "", duration).SetEase(DG.Tweening.Ease.Linear).OnComplete(() =>
{
    if (isNeedClick)
    {
        if (partsController != null)
        {
            partsController.ShowClickWaitIndicator();
        }
    }
    onComplete?.Invoke();
});
        }
        if (effectPlayer != null)
        {
            effectPlayer.PlayOutsideStandingEffect(standingImage);
        }
        // OutsideStandingImageが非表示なら再表示
        if (outsideStandingImage != null && !outsideStandingImage.gameObject.activeSelf)
        {
            outsideStandingImage.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 選択肢を表示（事前配置ボタンを使い回し）
    /// </summary>
    /// <param name="choices">選択肢テキストのリスト</param>
    /// <param name="onSelect">選択時コールバック（選択肢index）</param>
    public void ShowChoice(List<string> choices, System.Action<int> onSelect)
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            if (i < choices.Count)
            {
                var btn = choiceButtons[i];
                btn.gameObject.SetActive(true);
                var txt = btn.GetComponentInChildren<TextMeshProUGUI>();
                if (txt != null) txt.text = choices[i];
                btn.onClick.RemoveAllListeners();
                int idx = i;
                btn.onClick.AddListener(() => onSelect?.Invoke(idx));
            }
            else
            {
                if (choiceButtons[i] != null) choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 選択肢を非表示
    /// </summary>
    public void HideChoices()
    {
        foreach (var btn in choiceButtons)
        {
            if (btn != null) btn.gameObject.SetActive(false);
        }
    }

}
