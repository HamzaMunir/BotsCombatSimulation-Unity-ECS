using ECSBattleSimulation.Components;
using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Authoring
{
    public class CharacterAssetAuthoring : MonoBehaviour
    {
        public GameObject FriendlyPrefab;
        public GameObject EnemyPrefab;
        
        public class CharacterAssetBaker : Baker<CharacterAssetAuthoring>
        {
            public override void Bake(CharacterAssetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CharacterAssetComponent()
                {
                    FriendlyEntity = GetEntity(authoring.FriendlyPrefab, TransformUsageFlags.Dynamic),
                    EnemyEntity = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}