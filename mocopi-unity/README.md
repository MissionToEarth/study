# mocopi Unity プロジェクト

Unity 2022.3 LTS + Sony mocopi を使ったゲームのベースプロジェクト。

## 必要なもの

- Unity 2022.3.x LTS
- [mocopi Receiver Plugin for Unity](https://github.com/sony/mocopi-receiver-plugin-unity/releases)
- Sony mocopi 本体 + スマートフォンアプリ

## プロジェクト構成

```
Assets/
├── Scripts/
│   ├── Mocopi/
│   │   ├── MocopiManager.cs        # UDP受信・イベント管理
│   │   └── MocopiAvatarController.cs # アバターへのポーズ適用
│   └── Game/
│       ├── GameManager.cs          # ゲームフロー・UI管理
│       └── PoseRecognizer.cs       # ジェスチャー認識
└── Scenes/
    └── MainScene.md                # シーン構築手順
```

## セットアップ

### 1. mocopi SDK のインストール

```
Assets > Import Package > Custom Package
→ mocopi-receiver-plugin-unity-*.unitypackage を選択
```

### 2. SDK を有効化

`Assets/Scripts/Mocopi/MocopiManager.cs` を開き、
`#if MOCOPI_SDK` / `#endif` のコメントブロックを解除します。

### 3. シーンの設定

`Assets/Scenes/MainScene.md` の手順に従ってシーンを構築します。

### 4. mocopi アプリの設定

mocopi スマートフォンアプリ:
```
設定 > データ送信 > 送信先
  → PCのIPアドレス : 12351
```

## 主要スクリプト概要

### MocopiManager
- UDP 受信の開始/停止
- ポーズデータを `MocopiPoseData` として各コンポーネントに配布
- Unity Editor ではダミーポーズで動作確認可能

### MocopiAvatarController
- `MocopiManager.onPoseUpdated` を購読
- 27ボーン → `HumanBodyBones` のマッピングで Humanoid アバターに適用
- スムージング付き

### PoseRecognizer
- ジャンプ / 腕上げ / しゃがみ のジェスチャーを UnityEvent で通知
- 閾値は Inspector から調整可能

### GameManager
- mocopi 接続状態に応じて UI パネルを切り替え
- `PoseRecognizer` のイベントでゲームロジックをトリガー

## ライセンス

mocopi Receiver Plugin for Unity: Apache License 2.0  
(https://github.com/sony/mocopi-receiver-plugin-unity)
