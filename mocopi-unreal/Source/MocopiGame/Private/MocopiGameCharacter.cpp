#include "MocopiGameCharacter.h"
#include "ILiveLinkClient.h"
#include "Roles/LiveLinkAnimationRole.h"
#include "Roles/LiveLinkAnimationTypes.h"
#include "Features/IModularFeatures.h"
#include "Components/SkeletalMeshComponent.h"

AMocopiGameCharacter::AMocopiGameCharacter()
{
	PrimaryActorTick.bCanEverTick = true;
}

void AMocopiGameCharacter::BeginPlay()
{
	Super::BeginPlay();
}

void AMocopiGameCharacter::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

	// 定期的に LiveLink 接続確認
	ConnectionCheckTimer += DeltaTime;
	if (ConnectionCheckTimer >= ConnectionCheckInterval)
	{
		ConnectionCheckTimer = 0.f;
		CheckLiveLinkConnection();
	}

	if (bIsMocopiConnected)
	{
		UpdateGestureDetection(DeltaTime);
	}
}

void AMocopiGameCharacter::CheckLiveLinkConnection()
{
	IModularFeatures& ModularFeatures = IModularFeatures::Get();
	if (!ModularFeatures.IsModularFeatureAvailable(ILiveLinkClient::ModularFeatureName))
	{
		return;
	}

	ILiveLinkClient& LiveLinkClient =
		ModularFeatures.GetModularFeature<ILiveLinkClient>(ILiveLinkClient::ModularFeatureName);

	// mocopi サブジェクトが存在するか確認
	TArray<FLiveLinkSubjectKey> Subjects = LiveLinkClient.GetSubjects(true, true);
	bool bFound = false;
	for (const FLiveLinkSubjectKey& SubjectKey : Subjects)
	{
		if (SubjectKey.SubjectName == LiveLinkSubjectName)
		{
			bFound = true;
			break;
		}
	}

	if (bFound && !bIsMocopiConnected)
	{
		bIsMocopiConnected = true;
		UE_LOG(LogTemp, Log, TEXT("[MocopiGame] mocopi 接続を検出: %s"), *LiveLinkSubjectName.ToString());
		OnMocopiConnected();
	}
	else if (!bFound && bIsMocopiConnected)
	{
		bIsMocopiConnected = false;
		UE_LOG(LogTemp, Warning, TEXT("[MocopiGame] mocopi 切断: %s"), *LiveLinkSubjectName.ToString());
		OnMocopiDisconnected();
	}
}

void AMocopiGameCharacter::UpdateGestureDetection(float DeltaTime)
{
	// 腰ボーン (Hips) の位置を Skeletal Mesh から取得
	USkeletalMeshComponent* Mesh = GetMesh();
	if (!Mesh) return;

	int32 HipBoneIndex = Mesh->GetBoneIndex(TEXT("Hips"));
	if (HipBoneIndex == INDEX_NONE)
	{
		// VRM / Metahuman など別の命名規則に対応
		HipBoneIndex = Mesh->GetBoneIndex(TEXT("pelvis"));
	}
	if (HipBoneIndex == INDEX_NONE) return;

	FVector HipPos = Mesh->GetBoneLocation(
		Mesh->GetBoneName(HipBoneIndex),
		EBoneSpaces::WorldSpace
	);
	HipWorldPosition = HipPos;

	// 基準高さを初期化
	if (BaseHipHeight < 0.f)
	{
		BaseHipHeight = HipPos.Z;
		PrevHipY = HipPos.Z;
		return;
	}

	// ジャンプ判定 (Z 速度)
	const float VerticalVelocity = (HipPos.Z - PrevHipY) / DeltaTime;
	if (VerticalVelocity > JumpVelocityThreshold)
	{
		UE_LOG(LogTemp, Log, TEXT("[MocopiGame] ジャンプ検出 velocity=%.1f"), VerticalVelocity);
		OnJumpGestureDetected();
	}

	// しゃがみ判定
	const float RelativeHeight = HipPos.Z / BaseHipHeight;
	const bool bShouldCrouch = RelativeHeight < CrouchHeightRatio;
	if (bShouldCrouch && !bIsCrouching)
	{
		bIsCrouching = true;
		UE_LOG(LogTemp, Log, TEXT("[MocopiGame] しゃがみ検出 height=%.2f"), RelativeHeight);
		OnCrouchGestureDetected();
	}
	else if (!bShouldCrouch && bIsCrouching)
	{
		bIsCrouching = false;
	}

	PrevHipY = HipPos.Z;
}

void AMocopiGameCharacter::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);
	// Enhanced Input は BP_MocopiGameCharacter で設定してください
}
