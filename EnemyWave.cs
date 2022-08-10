using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyWave : ScriptableObject
{
    [SerializeField] private EnemySpawnSyquence[] spawnSyquences;
    public State Begin() => new State(this);
    public struct State
    {
        private EnemyWave wave;
        private int index;
        private EnemySpawnSyquence.State sequence;

        public State(EnemyWave wave)
        {
            this.wave = wave;
            index = 0;
            sequence = this.wave.spawnSyquences[0].Begin;
            Game.Instance.currentWave++;
            if (Game.Instance.isUnderGround) ProfileAssistant.main.userProfile.underGroundWavesCount++;
            WaveNatification.main.ShowWaveInfo(Game.Instance.currentWave);
        }

        public float Progress(float deltaTime)
        {
            deltaTime = sequence.Progress(deltaTime);
            while(deltaTime >= 0)
            {
                if(index++ >= wave.spawnSyquences.Length - 1)
                {
                    return deltaTime;
                }
                sequence = wave.spawnSyquences[index].Begin;
                deltaTime = sequence.Progress(deltaTime);
            }
            return -1;
        }
    }
}
