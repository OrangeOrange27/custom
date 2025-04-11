using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.SoundSystem
{
    public enum SoundType
    {
        Flip,
        Match,
        Click,
        Error,
        Victory
    }


    [Serializable]
    public class SoundClip
    {
        public SoundType Type;
        public AudioClip Clip;
        [Range(0f, 1f)] public float Volume = 1f;
    }


    [CreateAssetMenu(menuName = "Audio/Sound Library")]
    public class SoundLibrary : ScriptableObject
    {
        public List<SoundClip> Clips;

        private Dictionary<SoundType, SoundClip> _lookup;

        public void Init()
        {
            _lookup = Clips.ToDictionary(c => c.Type, c => c);
        }

        public SoundClip GetClip(SoundType type)
        {
            if (_lookup == null)
                Init();

            return _lookup.GetValueOrDefault(type);
        }
    }
}