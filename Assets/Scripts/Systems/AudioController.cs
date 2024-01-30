using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

enum AudioType
{
    BGM,
    SFX
}

[System.Serializable]
class AudioObject
{
    public string AudioName;
    public AudioType AudioType;
    public AudioClip AudioClip;
    [Range(-3f, 3f)] public float AudioPitch = 1f;
    [Range(0f, 1f)] public float AudioVolume = 1f;
    public AudioSource Source;
    public bool Looping;
}

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup BGMGroup;
    [SerializeField] private AudioMixerGroup SFXGroup;
    //[SerializeField] private AudioObject[] BGM;
    //[SerializeField] private AudioObject[] SFX;
    [SerializeField] private AudioObject[] audios;

    private static AudioController _instance;
    public static AudioController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Audio Controller is null");
            }
            
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        foreach(var a in audios)
        {
            AudioSource aSource = gameObject.AddComponent<AudioSource>();

            aSource.clip = a.AudioClip;
            aSource.loop = a.Looping;
            aSource.pitch = a.AudioPitch;
            aSource.volume = a.AudioVolume;

            switch (a.AudioType)
            {
                case AudioType.BGM:
                    aSource.outputAudioMixerGroup = BGMGroup;
                    break;

                case AudioType.SFX:
                    aSource.outputAudioMixerGroup = SFXGroup;
                    break;
            }

            a.Source = aSource;
        }
    }

    private AudioObject SearchAudio(string audioName, AudioType type)
    {
        var audio = audios.FirstOrDefault(s => s.AudioName == audioName);

        if (audio == null)
        {
            Debug.LogError("Audio is not available");
            return null;
        }

        if (audio.AudioType != type)
        {
            Debug.LogErrorFormat("Audio is not {0}", type);
            return null;
        }

        return audio;
    }

    public void PlayBGM(string audioName)
    {
        //foreach (var a in BGM)
        //{
        //    if (a.AudioName == audioName) a.AObject.GetComponent<AudioSource>().Play();
        //    else a.AObject.GetComponent<AudioSource>().Stop();
        //}

        var audio = SearchAudio(audioName, AudioType.BGM);
        if (audio == null)
            return;

        StopBGM();

        audio.Source.Play();
    }

    public void StopBGM()
    {
        //foreach (var a in BGM)
        //    a.AObject.GetComponent<AudioSource>().Stop();

        foreach (var a in audios)
        {
            if (a.AudioType == AudioType.BGM)
                a.Source.Stop();
        }
    }

    public void StopBGM(string audioName)
    {
        //var bgm = BGM.FirstOrDefault(s => s.AudioName == audioName);
        //bgm.AObject.GetComponent<AudioSource>().Stop();

        var audio = SearchAudio(audioName, AudioType.BGM);
        if (audio == null)
            return;

        audio.Source.Stop();
    }

    public void PlaySFX(string audioName)
    {
        //var sfx = SFX.FirstOrDefault(s => s.AudioName == audioName);
        //sfx.AObject.GetComponent<AudioSource>().Play();

        var audio = SearchAudio(audioName, AudioType.SFX);
        if (audio == null)
            return;

        audio.Source.Play();
    }

    public void StopSFX()
    {
        //foreach (var a in SFX)
        //    a.AObject.GetComponent<AudioSource>().Stop();

        foreach (var a in audios)
        {
            if (a.AudioType == AudioType.SFX)
                a.Source.Stop();
        }
    }

    public void StopSFX(string audioName)
    {
        //var sfx = SFX.FirstOrDefault(s => s.AudioName == audioName);
        //sfx.AObject.GetComponent<AudioSource>().Stop();

        var audio = SearchAudio(audioName, AudioType.SFX);
        if (audio == null)
            return;

        audio.Source.Stop();
    }
}
