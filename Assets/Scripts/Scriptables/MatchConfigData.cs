using UnityEngine;

namespace ECSBattleSimulation.Scriptable
{
    [CreateAssetMenu(fileName = "ScriptableData", menuName = "Spawn Properties/Match Config")]
    public class MatchConfigData : ScriptableObject
    {
        public TeamSpawnData[] FriendlyTeamConfig;
        public TeamSpawnData[] EnemyTeamConfigs;
    }
}