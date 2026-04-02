# mocopi Unreal Engine プロジェクト

Unreal Engine 5.3 + Sony mocopi Live Link を使ったゲームのベースプロジェクト。

## 必要なもの

- Unreal Engine 5.3 以降
- [mocopi Receiver Plugin for Unreal Engine](https://github.com/sony/mocopi-receiver-plugin-unrealengine)
- Sony mocopi 本体 + スマートフォンアプリ

## プロジェクト構成

```
MocopiGame.uproject
Source/MocopiGame/
├── Public/
│   ├── MocopiGameCharacter.h   # キャラクター基底クラス
│   └── MocopiGameMode.h
├── Private/
│   ├── MocopiGameCharacter.cpp # Live Link 接続確認 + ジェスチャー検出
│   └── MocopiGameMode.cpp
└── MocopiGame.Build.cs
Config/
├── DefaultGame.ini
└── DefaultEngine.ini
Content/Blueprints/
└── README.md                   # BP・ABP のセットアップ手順
```

## セットアップ

### 1. mocopi Live Link プラグインのインストール

```
GitHub から Plugins/ フォルダをダウンロード:
https://github.com/sony/mocopi-receiver-plugin-unrealengine

→ プロジェクトの Plugins/ フォルダに配置
→ UE エディタで Edit > Plugins > "mocopi" を有効化
```

### 2. プロジェクトのビルド

```
MocopiGame.uproject を右クリック
→ "Generate Visual Studio project files"
→ MocopiGame.sln を開いてビルド
```

### 3. Live Link ソースの追加

```
Window > Virtual Production > Live Link
→ + Source > MocopiLiveLink Source
→ Port: 12351
```

### 4. Blueprint のセットアップ

`Content/Blueprints/README.md` の手順に従って
BP_MocopiCharacter と ABP_MocopiCharacter を作成します。

### 5. mocopi アプリの設定

mocopi スマートフォンアプリ:
```
設定 > データ送信 > 送信先
  → PCのIPアドレス : 12351
```

## 主要クラス概要

### AMocopiGameCharacter

| プロパティ | 説明 |
|---|---|
| `LiveLinkSubjectName` | Live Link のサブジェクト名 (デフォルト: "mocopi") |
| `JumpVelocityThreshold` | ジャンプ判定の垂直速度閾値 (cm/s) |
| `CrouchHeightRatio` | しゃがみ判定の高さ比率 |

| Blueprint イベント | トリガー条件 |
|---|---|
| `OnMocopiConnected` | Live Link にサブジェクトが現れたとき |
| `OnMocopiDisconnected` | サブジェクトが消えたとき |
| `OnJumpGestureDetected` | 腰ボーンの垂直速度が閾値超過 |
| `OnCrouchGestureDetected` | 腰の高さが基準より低下 |

## ライセンス

mocopi Receiver Plugin for Unreal Engine: Apache License 2.0  
(https://github.com/sony/mocopi-receiver-plugin-unrealengine)
