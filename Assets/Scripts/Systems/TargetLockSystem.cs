using ECSBattleSimulation.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace ECSBattleSimulation.Systems
{
    [UpdateAfter(typeof(TeamSpawnSystem))]
    public partial class TargetLockSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<GroundData>();
            RequireForUpdate<RandomComponent>();
            Enabled = false;
        }
        
        protected override void OnUpdate()
        {
            
            //Create entity queries to get spawned entities;
            EntityQuery entitiesQuery = EntityManager.CreateEntityQuery(typeof(TeamComponent));
            EntityQuery friendlyUnitsQuery = EntityManager.CreateEntityQuery(typeof(FriendlyTag));
            EntityQuery enemyUnitsQuery = EntityManager.CreateEntityQuery(typeof(EnemyTag));
            
            //Run All Queries
            NativeArray<Entity> entities = entitiesQuery.ToEntityArray(Allocator.TempJob);
            NativeArray<Entity> friendlyUnits = friendlyUnitsQuery.ToEntityArray(Allocator.TempJob);
            NativeArray<Entity> enemyUnits = enemyUnitsQuery.ToEntityArray(Allocator.TempJob);
            //Debug.Log($"Friendly Units {friendlyUnits.Length} Enemy Units {enemyUnits.Length}");
            
            var randomComponent = SystemAPI.GetSingleton<RandomComponent>();
            
            Entities.WithAll<CharacterTarget>().WithAll<TeamComponent>().WithReadOnly(entities).WithReadOnly(enemyUnits)
                .WithReadOnly(friendlyUnits).ForEach((
                    ref CharacterTarget characterTarget,
                    in TeamComponent team) =>
                {
                    if (entities.Contains(characterTarget.Target))
                    {
                        return;
                    }

                    if (team.Team == Team.Friendly)
                    {
                        if (enemyUnits.Length <= 0)
                        { 
                            return;
                        }
                        characterTarget.Target = enemyUnits[randomComponent.Value.NextInt(0, enemyUnits.Length)];
                        characterTarget.IsTargetLocked = true;
                    }
                    else
                    {
                        if (friendlyUnits.Length <= 0)
                        {
                            return;
                        }
                        characterTarget.Target = friendlyUnits[randomComponent.Value.NextInt(0, friendlyUnits.Length)];
                        characterTarget.IsTargetLocked = true;
                    }
                }).ScheduleParallel();
            
            EntityManager.WorldUnmanaged.GetExistingSystemState<GameResultSystem>().Enabled = true;
        }
    }
}