using System.Collections.Generic;

/// <summary>
/// ゲーム内イベント（セリフ・演出・分岐など）をデータとして表現するクラス
/// </summary>
public class EventData
{
    public string Id { get; set; }
    public string Type { get; set; } // 例: "Dialogue", "ChangeSprite", "Choice" など
    public string Character { get; set; } // セリフや演出の対象キャラ
    public string Text { get; set; } // セリフ本文
    public List<string> Choices { get; set; } // 選択肢
    public List<string> Next { get; set; } // 次のイベントID（分岐対応）
    public string NextEventId { get; set; } // 単一遷移用
    public string Flag { get; set; } // 操作・判定するフラグ名
    public bool? Value { get; set; } // フラグ値
    public string FlagCondition { get; set; } // 分岐条件

    public EventData()
    {
        Choices = new List<string>();
        Next = new List<string>();
    }
}
