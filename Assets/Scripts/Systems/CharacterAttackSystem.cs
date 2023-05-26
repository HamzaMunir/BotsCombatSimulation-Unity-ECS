using ECSBattleSimulation.Authoring;
using ECSBattleSimulation.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECSBattleSimulation.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(CharacterMoveSystem))]
    public partial class CharacterAttackSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<BulletPrefabComponent>();
            World.Unmanaged.GetExistingSystemState<GameResultSystem>().Enabled = true;
        }
        
        protected override void OnUpdate()
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged).AsParallelWriter();
            var transformLookup = GetComponentLookup<LocalTransform>(true);
            var bulletPrefab =SystemAPI.GetSingleton<BulletPrefabComponent>().Bullet;
            
            Entities.WithReadOnly(transformLookup).ForEach(
                (Entity entity, int entityInQueryIndex, ref CharacterAttackTime attackTime, in CharacterTarget target,
                    in CharacterAttackDamage attackDamage, in CharacterAttackRange attackRange) =>
                {
                    if (!transformLookup.HasComponent(target.Target) || !transformLookup.HasComponent(entity))
                    {
                        return;
                    }

                    
                    var targetTransform = transformLookup.GetRefRO(target.Target);
                    var entityTransform = transformLookup.GetRefRO(entity);
                    var distance = math.distance(entityTransform.ValueRO.Position, targetTransform.ValueRO.Position);
                    if (distance <= attackRange.AttackRange)
                    {
                        attackTime.Cooldown -= deltaTime;
                        if (attackTime.Cooldown <= 0)
                        {
                            attackTime.Cooldown = attackTime.AttackSpeed;
                            //Spawn a bullet
                            var newBullet = ecb.Instantiate(entityInQueryIndex, bulletPrefab);
                            ecb.AddComponent(entityInQueryIndex, newBullet, new LocalTransform()
                            {
                                Position = entityTransform.ValueRO.Position,
                                Scale = 0.25f
                            });
                            ecb.AddComponent(entityInQueryIndex, newBullet, new BulletProperties()
                            {
                                Damage = attackDamage.AttackDamage,
                                OriginEntity = entity,
                                TargetEntity = target.Target,
                                TargetPosition = targetTransform.ValueRO.Position
                            });
                        }
                    }
                }).ScheduleParallel();
        }
    }
}