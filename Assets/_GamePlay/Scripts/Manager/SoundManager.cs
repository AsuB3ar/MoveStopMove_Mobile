using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Manager
{
    using Utilitys;
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField]
        AudioSource globalAudioSource;       
        [SerializeField]
        private Pool soundPoolScript;
        [Range(0, 1)]
        [SerializeField]
        float volume = 1f;

        [SerializeField]
        GameObject audioSourceObject;
        private List<GameObject> currentPlayAudioSource = new List<GameObject>();
        public enum Sound
        {
            Character_Die1 = 0,
            Character_Hit = 1,
            Character_WinLevel = 2,
            Character_LoseLevel = 3,
            Character_SizeUp = 4,
            Character_Die2 = 5,
            Character_Die3 = 6,
            Character_Die4 = 7,
            Character_Die5 = 8,
            
            Weapon_Throw = 100,
            Button_Click = 1000,

        }
        protected override void Awake()
        {
            base.Awake();
            soundPoolScript.Initialize(audioSourceObject);
        }
        private void FixedUpdate()
        {
            for(int i = 0; i < currentPlayAudioSource.Count; i++)
            {
                if (!Cache.GetAudioSource(currentPlayAudioSource[i]).isPlaying)
                {
                    soundPoolScript.Push(currentPlayAudioSource[i]);
                    currentPlayAudioSource.RemoveAt(i);
                    i--;
                }
            }
        }
        public void PlaySound(Sound sound)
        {
            globalAudioSource.PlayOneShot(GameAssets.Inst.SoundAssets[sound],volume);
        }
        public void PlaySound(Sound sound, Vector3 position)
        {
            GameObject soundObj = soundPoolScript.Pop();
            soundObj.transform.position = position;

            AudioSource audioSource = Cache.GetAudioSource(soundObj);
            audioSource.clip = GameAssets.Inst.SoundAssets[sound];
            audioSource.maxDistance = 10;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            audioSource.dopplerLevel = 0;

            if(sound == Sound.Character_SizeUp)
            {
                audioSource.volume = volume * 0.2f;
            }
            else
            {
                audioSource.volume = volume;
            }
            audioSource.Play();

            currentPlayAudioSource.Add(soundObj);
        }
        public Sound GetRandomDieSound()
        {
            int value = Random.Range(0, 5);

            switch (value)
            {
                case 0: 
                    return Sound.Character_Die1;
                case 1: 
                    return Sound.Character_Die2;
                case 2:
                    return Sound.Character_Die3;
                case 3:
                    return Sound.Character_Die4;
                case 4:
                    return Sound.Character_Die5;
                default:
                    return Sound.Character_Die1;
            }
        }
        public void SetVolume(float value)
        {
            if(value >= 0 && value <= 1)
            {
                volume = value;
            }
        }
    }
}