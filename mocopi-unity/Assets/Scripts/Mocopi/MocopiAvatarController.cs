using System.Collections.Generic;
using UnityEngine;
using MocopiGame.Mocopi;

namespace MocopiGame.Mocopi
{
    /// <summary>
    /// MocopiManager からのポーズデータを Humanoid アバターに適用します。
    ///
    /// 使い方:
    ///   1. Humanoid Rig が設定済みの Animator を持つアバターにアタッチ
    ///   2. BoneMappings に mocopi ボーンID と HumanBodyBones のマッピングを設定
    ///      (または SetupDefaultMapping() を呼び出す)
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class MocopiAvatarController : MonoBehaviour
    {
        [Header("ボーンマッピング")]
        [SerializeField] private List<BoneMappingEntry> boneMappings = new();

        [Header("スムージング")]
        [Range(0f, 1f)]
        [SerializeField] private float positionSmoothing = 0.1f;
        [Range(0f, 1f)]
        [SerializeField] private float rotationSmoothing = 0.1f;

        private Animator _animator;
        private readonly Dictionary<int, HumanBodyBones> _boneMap = new();
        private readonly Dictionary<HumanBodyBones, Quaternion> _targetRotations = new();

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            BuildBoneMap();
        }

        private void OnEnable()
        {
            if (MocopiManager.Instance != null)
                MocopiManager.Instance.onPoseUpdated.AddListener(ApplyPose);
        }

        private void OnDisable()
        {
            if (MocopiManager.Instance != null)
                MocopiManager.Instance.onPoseUpdated.RemoveListener(ApplyPose);
        }

        private void ApplyPose(MocopiPoseData poseData)
        {
            foreach (var (boneId, boneTransform) in poseData.Bones)
            {
                if (!_boneMap.TryGetValue(boneId, out var humanBone)) continue;

                if (!_targetRotations.ContainsKey(humanBone))
                    _targetRotations[humanBone] = boneTransform.Rotation;
                else
                    _targetRotations[humanBone] = Quaternion.Slerp(
                        _targetRotations[humanBone],
                        boneTransform.Rotation,
                        1f - rotationSmoothing
                    );
            }
        }

        private void LateUpdate()
        {
            // IK ではなく直接ボーン回転を適用 (mocopi SDK 標準方式)
            foreach (var (humanBone, rotation) in _targetRotations)
            {
                var boneTransform = _animator.GetBoneTransform(humanBone);
                if (boneTransform != null)
                    boneTransform.rotation = rotation;
            }
        }

        private void BuildBoneMap()
        {
            _boneMap.Clear();
            foreach (var entry in boneMappings)
                _boneMap[entry.mocopiBoneId] = entry.humanBodyBone;

            if (_boneMap.Count == 0)
                SetupDefaultMapping();
        }

        /// <summary>mocopi 27ボーン → HumanBodyBones のデフォルトマッピング。</summary>
        public void SetupDefaultMapping()
        {
            _boneMap.Clear();
            var defaults = new Dictionary<int, HumanBodyBones>
            {
                { 0,  HumanBodyBones.Hips },
                { 1,  HumanBodyBones.Spine },
                { 2,  HumanBodyBones.Chest },
                { 3,  HumanBodyBones.UpperChest },
                { 4,  HumanBodyBones.Neck },
                { 5,  HumanBodyBones.Head },
                { 6,  HumanBodyBones.LeftUpperArm },
                { 7,  HumanBodyBones.LeftLowerArm },
                { 8,  HumanBodyBones.LeftHand },
                { 9,  HumanBodyBones.RightUpperArm },
                { 10, HumanBodyBones.RightLowerArm },
                { 11, HumanBodyBones.RightHand },
                { 12, HumanBodyBones.LeftUpperLeg },
                { 13, HumanBodyBones.LeftLowerLeg },
                { 14, HumanBodyBones.LeftFoot },
                { 15, HumanBodyBones.LeftToes },
                { 16, HumanBodyBones.RightUpperLeg },
                { 17, HumanBodyBones.RightLowerLeg },
                { 18, HumanBodyBones.RightFoot },
                { 19, HumanBodyBones.RightToes },
            };
            foreach (var kv in defaults)
                _boneMap[kv.Key] = kv.Value;
        }

        [System.Serializable]
        public class BoneMappingEntry
        {
            [Tooltip("mocopi SDK のボーンID (0–26)")]
            public int mocopiBoneId;
            public HumanBodyBones humanBodyBone;
        }
    }
}
