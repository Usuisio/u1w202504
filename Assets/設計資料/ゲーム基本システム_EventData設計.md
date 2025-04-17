# ゲーム基本システム EventData設計方針

---

## 1. EventDataとは？

- ストーリー進行・演出・分岐など、ゲーム内で発生する「イベント」をデータとして記述・管理するための設計
- セリフ表示、立ち絵変更、シーン切り替え、フラグ操作、選択肢分岐など、すべてEventDataで表現できる

---

## 2. EventDataの主なパラメータ例

| パラメータ名   | 内容例・用途                                   |
|----------------|-----------------------------------------------|
| type           | イベントの種類（Dialogue, ChangeSprite, ...） |
| character      | セリフや立ち絵の対象キャラ名                   |
| text           | 表示するセリフ                                 |
| sprite         | 変更する立ち絵の種類                           |
| scene          | 切り替えるシーン名                             |
| choices        | 選択肢リスト                                   |
| next           | 次のイベントID（または分岐先リスト）           |
| flag           | 操作・判定するフラグ名                         |
| value          | フラグの値（true/false）                       |
| flagCondition  | 分岐条件となるフラグ名                         |

---

## 3. EventData記述例

```json
[
    {
        "id": "event001",
        "type": "Dialogue",
        "character": "コッペ",
        "text": "こんにちは！",
        "next": "event002"
    },
    {
        "id": "event002",
        "type": "ChangeSprite",
        "character": "ノワ",
        "sprite": "smile",
        "next": "event003"
    },
    {
        "id": "event003",
        "type": "SceneChange",
        "scene": "教室",
        "next": "event010"
    },
    {
        "id": "event010",
        "type": "Choice",
        "choices": ["Aを選ぶ", "Bを選ぶ"],
        "next": ["eventA", "eventB"]
    },
    {
        "id": "eventA",
        "type": "FlagSet",
        "flag": "ノワルート",
        "value": true,
        "next": "event100"
    }
]
```

---

## 4. 拡張性・運用ポイント

- 必要な演出や分岐が増えたら、typeやパラメータを追加して柔軟に拡張できる
- シナリオ進行・演出・分岐の“設計図”としてEventDataを活用
- 複雑な演出や特殊処理は、EventController側でtypeごとに処理を追加
- スクリプトを書き換えずに、EventData編集だけでストーリーや演出を追加・修正できる

---

## 5. 注意点

- EventDataの型やパラメータは、最初はシンプルに、必要に応じて拡張していく
- データ量が増えても管理しやすいよう、IDや分岐先の命名規則を決めておくと良い

---

## 6. まとめ

- ゲーム内のセリフ・演出・分岐は、基本的にEventDataに記述して管理する
- シナリオ進行・演出・分岐の柔軟な制御が可能
- 設計・運用のベースとしてEventDataを活用しよう！
