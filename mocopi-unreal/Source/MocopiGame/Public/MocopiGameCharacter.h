#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "LiveLinkTypes.h"
#include "MocopiGameCharacter.generated.h"

/**
 * mocopi の Live Link データを受け取り、アニメーションに適用するキャラクター。
 *
 * セットアップ手順:
 *   1. このクラスを親にした Blueprint キャラクターを作成
 *   2. Skeletal Mesh に Humanoid スケルトンを設定
 *   3. Animation Blueprint で LiveLinkPose ノードを追加し
 *      SubjectName に mocopi の Live Link サブジェクト名を設定
 *   4. LiveLinkSubjectName プロパティに同じ名前を設定
 */
UCLASS(Blueprintable)
class MOCOPIGAME_API AMocopiGameCharacter : public ACharacter
{
	GENERATED_BODY()

public:
	AMocopiGameCharacter();

protected:
	virtual void BeginPlay() override;
	virtual void Tick(float DeltaTime) override;
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

public:
	/** mocopi Live Link サブジェクト名 (デフォルト: "mocopi") */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "mocopi")
	FName LiveLinkSubjectName = TEXT("mocopi");

	/** Live Link 接続中かどうか */
	UPROPERTY(BlueprintReadOnly, Category = "mocopi")
	bool bIsMocopiConnected = false;

	/** 腰ボーンの位置 (ゲームロジック用) */
	UPROPERTY(BlueprintReadOnly, Category = "mocopi|Pose")
	FVector HipWorldPosition = FVector::ZeroVector;

	/** ジャンプ速度閾値 (cm/s) */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "mocopi|Gesture")
	float JumpVelocityThreshold = 150.f;

	/** しゃがみ高さ閾値 (基準高さからの比率) */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "mocopi|Gesture")
	float CrouchHeightRatio = 0.8f;

	// ---- Blueprint イベント ----

	/** mocopi 接続時に呼ばれる */
	UFUNCTION(BlueprintImplementableEvent, Category = "mocopi")
	void OnMocopiConnected();

	/** mocopi 切断時に呼ばれる */
	UFUNCTION(BlueprintImplementableEvent, Category = "mocopi")
	void OnMocopiDisconnected();

	/** ジャンプジェスチャー検出時に呼ばれる */
	UFUNCTION(BlueprintImplementableEvent, Category = "mocopi|Gesture")
	void OnJumpGestureDetected();

	/** しゃがみジェスチャー検出時に呼ばれる */
	UFUNCTION(BlueprintImplementableEvent, Category = "mocopi|Gesture")
	void OnCrouchGestureDetected();

private:
	void CheckLiveLinkConnection();
	void UpdateGestureDetection(float DeltaTime);

	float PrevHipY = 0.f;
	float BaseHipHeight = -1.f;
	bool bIsCrouching = false;
	float ConnectionCheckTimer = 0.f;
	static constexpr float ConnectionCheckInterval = 1.0f;
};
