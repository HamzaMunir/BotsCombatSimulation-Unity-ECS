using ECSBattleSimulation.Scriptable;
using UnityEngine;

namespace ECSBattleSimulation.Mono
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; private set; }
        
        public MatchConfigData Config;
        
        public TeamSpawnData CurrentSelectedEnemyConfig;
        public TeamSpawnData CurrentSelectedFriendlyConfig;

        void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            } 
        }
    }
}