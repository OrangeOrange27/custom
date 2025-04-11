using UnityEngine;

namespace DefaultNamespace.SoundSystem
{
    public interface ISoundManager
    {
        void Play(SoundType type);
        void Stop();
    }
    
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour, ISoundManager
    {
        public static ISoundManager Instance { get; private set; }

        [SerializeField] private SoundLibrary library;
        [SerializeField] private bool persistAcrossScenes = true;

        private AudioSource _audioSource;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _audioSource = GetComponent<AudioSource>();
            library.Init();

            if (persistAcrossScenes)
                DontDestroyOnLoad(gameObject);
        }

        public void Play(SoundType type)
        {
            var sound = library.GetClip(type);
            if (sound?.Clip == null) return;

            _audioSource.PlayOneShot(sound.Clip, sound.Volume);
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}