using ECSBattleSimulation.Components;
using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Authoring
{
    public class FriendlyAuthoring : MonoBehaviour
    {
        public int AttackRange;
        public int AttackDamage;
        public int HealthPoints;
        public float AttackSpeed;
        public int MovementSpeed;

        public GameObject HealthPrefab;
        
        public class FriendlyBaker : Baker<FriendlyAuthoring>
        {
            public override void Bake(FriendlyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new FriendlyTag());
                AddComponent(entity, new CharacterAttackDamage()
                {
                    AttackDamage = authoring.AttackDamage
                });
                AddComponent(entity, new CharacterHealthPoints()
                {
                    HealthPoints = authoring.HealthPoints,
                    CurrentHealth = authoring.HealthPoints
                });
                AddComponent(entity, new CharacterMovementSpeed()
                {
                    MovementSpeed = authoring.MovementSpeed
                });
                AddComponent(entity, new CharacterAttackRange()
                {
                    AttackRange = authoring.AttackRange
                });
                AddComponent(entity, new CharacterAttackTime()
                {
                    AttackSpeed = authoring.AttackSpeed,
                    Cooldown = authoring.AttackSpeed
                });
                
                HealthComponent healthComponent = new HealthComponent();
                healthComponent.HealthUI = authoring.HealthPrefab;
                AddComponentObject(entity, healthComponent);

            }
        }
    }
}