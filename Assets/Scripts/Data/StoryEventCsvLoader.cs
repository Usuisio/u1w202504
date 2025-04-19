using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

/// <summary>
/// StoryEvents.csvを読み込んでストーリーイベントリストを生成するクラス
/// </summary>
public class StoryEventCsvLoader
{
    public class StoryEventRow
    {
        public int id;
        public string type;
        public string content;
        public string[] args = new string[6];
        public bool isNeedClick;
        public string memo;
    }

    /// <summary>
    /// CSVファイルからストーリーイベントリストを読み込む
    /// </summary>
    /// <param name="csvPath">Resources以下のパス（拡張子なし）</param>
    public static List<StoryEventRow> Load(string csvPath)
    {
        var result = new List<StoryEventRow>();
        TextAsset csvAsset = Resources.Load<TextAsset>(csvPath);
        if (csvAsset == null)
        {
            Debug.LogError($"[StoryEventCsvLoader] Resources.Load<TextAsset>でnullが返りました。パス: {csvPath} (例: 'StoryEvent'ならAssets/Resources/StoryEvent.csv)");
            return result;
        }
        else
        {
            Debug.Log($"[StoryEventCsvLoader] Resources.Load<TextAsset>成功: {csvPath} (bytes: {csvAsset.bytes.Length})");
        }

        using (StringReader reader = new StringReader(csvAsset.text))
        {
            string header = reader.ReadLine(); // ヘッダー行をスキップ
            string line;
            int lineNum = 1;
            while ((line = reader.ReadLine()) != null)
            {
                lineNum++;
                if (string.IsNullOrWhiteSpace(line)) continue;
                var columns = ParseCsvLine(line);
                if (columns.Count < 11)
                {
                    Debug.LogWarning($"[StoryEventCsvLoader] {lineNum}行目のカラム数が不足: {columns.Count}列 内容: {line}");
                    continue;
                }

                var row = new StoryEventRow();
                int.TryParse(columns[0], out row.id);
                row.type = columns[1];
                row.isNeedClick = columns[2].Trim().ToLower() == "true";
                row.content = columns[3];
                for (int i = 0; i < 6; i++)
                {
                    row.args[i] = columns[4 + i];
                }
                row.memo = columns[10];
                result.Add(row);
            }
            Debug.Log($"[StoryEventCsvLoader] 読み込んだイベント件数: {result.Count}");
        }
        return result;
    }

    // シンプルなCSVパーサー（カンマ区切り、ダブルクォート対応）
    private static List<string> ParseCsvLine(string line)
    {
        var result = new List<string>();
        bool inQuotes = false;
        string value = "";
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(value);
                value = "";
            }
            else
            {
                value += c;
            }
        }
        result.Add(value);
        return result;
    }
}
