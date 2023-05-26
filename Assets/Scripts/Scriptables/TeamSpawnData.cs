using System;
using System.Collections.Generic;
using ECSBattleSimulation.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECSBattleSimulation.Scriptable
{
    [CreateAssetMenu(fileName = "ScriptableData", menuName = "Spawn Properties/Team Spawn Data")]
    public class TeamSpawnData : ScriptableObject
    {
        private static readonly Dictionary<string, BlobAssetReference<SpawnableTeamBlobAsset>> ConvertedAssetReference =
            new();

        [SerializeField] public uint Rows;
        [SerializeField] public uint Columns;
        [SerializeField] public CharacterProperties[] Units;

        public BlobAssetReference<SpawnableTeamBlobAsset> Convert()
        {
            if (ConvertedAssetReference.ContainsKey(this.name))
            {
                return ConvertedAssetReference[this.name];
            }
            BlobAssetReference<SpawnableTeamBlobAsset> team;
            {
                var builder = new BlobBuilder( Allocator.Temp );
                ref var root = ref builder.ConstructRoot<SpawnableTeamBlobAsset>();
                {
                    root.Rows = Rows;
                }
                {
                    root.Columns = Columns;
                }
                {
                    int length = Units.Length;
                    var arr = builder.Allocate( ref root.UnitsData , length );
                    for (int i = 0; i < length; i++)
                        arr[i] = new CharacterProperties
                        {
                            AttackDamage = Units[i].AttackDamage,
                            AttackRange = Units[i].AttackRange,
                            HealthPoints = Units[i].HealthPoints,
                            MovementSpeed = Units[i].MovementSpeed,
                            AttackSpeed = Units[i].AttackSpeed
                        };
                }
                team = builder.CreateBlobAssetReference<SpawnableTeamBlobAsset>( Allocator.Persistent );
                builder.Dispose();
            }
 
            ConvertedAssetReference.Add( this.name , team );
 
            return team;
        }

        private void OnDestroy()
        {
            ConvertedAssetReference.Clear();
        }
    }

    [System.Serializable]
    public struct CharacterProperties : IComponentData
    {
        public int AttackRange;
        public int HealthPoints;
        public int AttackDamage;
        public float AttackSpeed;
        public int MovementSpeed;
    }

    public struct SpawnableTeamBlobAsset
    {
        public uint Rows;
        public uint Columns;
        public BlobArray<CharacterProperties> UnitsData;
    }

    public struct SpawnPointsBlob
    {
        public BlobArray<float3> Value;
    }
    
}