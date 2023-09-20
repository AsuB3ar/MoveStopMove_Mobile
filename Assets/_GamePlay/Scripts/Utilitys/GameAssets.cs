using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys
{
    using MoveStopMove.Manager;
    public class GameAssets : Singleton<GameAssets>
    {
        [SerializeField]
        private SoundAudioClip[] soundAssets;
        public Dictionary<SoundManager.Sound, AudioClip> SoundAssets = new Dictionary<SoundManager.Sound, AudioClip>();
        protected override void Awake()
        {
            base.Awake();
            for(int i = 0; i < soundAssets.Length; i++)
            {
                SoundAssets.Add(soundAssets[i].sound, soundAssets[i].audioClip);
            }
        }

        
        [System.Serializable]
        public class SoundAudioClip
        {
            public SoundManager.Sound sound;
            public AudioClip audioClip;
        }
    }
}