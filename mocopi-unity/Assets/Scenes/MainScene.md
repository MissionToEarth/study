# MainScene セットアップ手順

Unity エディタで以下の階層を手動で作成してください。

## Hierarchy 構成

```
MainScene
├── [管理オブジェクト]
│   ├── MocopiManager          ← MocopiManager.cs をアタッチ
│   └── GameManager            ← GameManager.cs をアタッチ
│
├── [アバター]
│   └── Avatar (Humanoid)      ← FBX/VRM アバター
│       └── MocopiAvatarController.cs + PoseRecognizer.cs をアタッチ
│
├── [カメラ]
│   └── Main Camera
│       └── CinemachineVirtualCamera (アバターを追従)
│
└── [UI Canvas]
    ├── ConnectingPanel
    │   ├── StatusText (TMP)
    │   └── StartButton
    └── GamePanel
        └── StopButton
```

## mocopi SDK の追加

1. https://github.com/sony/mocopi-receiver-plugin-unity/releases から
   最新の `.unitypackage` をダウンロード
2. Assets → Import Package → Custom Package でインポート
3. `MocopiSimpleReceiver` プレハブを `MocopiManager` と同じ GameObject に配置
4. `MocopiManager.cs` の `#if MOCOPI_SDK` ブロックのコメントを解除

## 動作確認

- Unity エディタでは `UNITY_EDITOR` 定義によりダミーポーズが流れます
- 実機確認は mocopi アプリ → 設定 → 送信先IPアドレスを PC に設定 → port 12351
