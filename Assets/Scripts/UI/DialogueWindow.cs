using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// テキスト表示＆テキスト送りを管理するクラス
/// </summary>
public class DialogueWindow : MonoBehaviour
{
    [Header("ノワのテキストUI")]
    [SerializeField] private Text nowaText;

    [Header("コッペのテキストUI")]
    [SerializeField] private Text koppeText;

    // 仮のテキストデータ
    private readonly string[] nowaLines = new string[]
    {
        "こんにちは、プレイヤーさん！",
        "今日はどんな一日だった？",
        "次の選択肢を選んでね。"
    };

    private readonly string[] koppeLines = new string[]
    {
        "やっほー！",
        "ノワ、今日も元気そうだね。",
        "どれ選ぶ？"
    };

    private int nowaIndex = 0;
    private int koppeIndex = 0;

    private void Start()
    {
        ShowNowaLine();
        ShowKoppeLine();
    }

    private void Update()
    {
        // クリック or スペースキーでテキスト送り
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceNowaLine();
            AdvanceKoppeLine();
        }
    }

    private void ShowNowaLine()
    {
        if (nowaText != null && nowaIndex < nowaLines.Length)
        {
            nowaText.text = nowaLines[nowaIndex];
        }
    }

    private void ShowKoppeLine()
    {
        if (koppeText != null && koppeIndex < koppeLines.Length)
        {
            koppeText.text = koppeLines[koppeIndex];
        }
    }

    private void AdvanceNowaLine()
    {
        if (nowaIndex < nowaLines.Length - 1)
        {
            nowaIndex++;
            ShowNowaLine();
        }
    }

    private void AdvanceKoppeLine()
    {
        if (koppeIndex < koppeLines.Length - 1)
        {
            koppeIndex++;
            ShowKoppeLine();
        }
    }
}
