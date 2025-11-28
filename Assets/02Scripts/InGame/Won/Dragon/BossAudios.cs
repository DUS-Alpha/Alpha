using UnityEngine;
using System.Collections.Generic;

public class BossAudio : MonoBehaviour
{
    [System.Serializable]
    public class BossSound
    {
        public string key;       // "Roar", "Breath", "Death" 등
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<BossSound> sounds;

    private Dictionary<string, BossSound> soundDict;

    void Awake()
    {
        soundDict = new Dictionary<string, BossSound>();
        foreach (var s in sounds)
            soundDict[s.key] = s;
    }
    

    public void Play(string key)
    {
        print(key+"실행");
        if (soundDict.TryGetValue(key, out BossSound s))
        {
            sfxSource.PlayOneShot(s.clip, s.volume);
        }
    }
    
}