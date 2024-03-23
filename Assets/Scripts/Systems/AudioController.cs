using System.Collections;
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
    [HideInInspector]  public AudioSource Source;
    public bool Looping;
    public bool PlayOnAwake;
}

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup BGMGroup;
    [SerializeField] private AudioMixerGroup SFXGroup;
    //[SerializeField] private AudioObject[] BGM;
    //[SerializeField] private AudioObject[] SFX;
    [SerializeField] private AudioObject[] audios;

    private AudioSource currentBGMPlaying;
    private AudioSource nextAudioPlaying;
    //private bool audioTransitioning;

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
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);

        //Debug.LogFormat("{0} {1} {2}", gameObject.active, gameObject.activeSelf, gameObject.activeInHierarchy);

        foreach (var a in audios)
        {
            AudioSource aSource = gameObject.AddComponent<AudioSource>();

            aSource.clip = a.AudioClip; 
            aSource.loop = a.Looping;
            aSource.pitch = a.AudioPitch;
            aSource.volume = a.AudioVolume;
            aSource.playOnAwake = a.PlayOnAwake;

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

    //private void Update()
    //{
    //    Debug.LogFormat("{0} {1} {2}", gameObject.active, gameObject.activeSelf, gameObject.activeInHierarchy);
    //}

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

        //if (audioTransitioning) return;

        var audio = SearchAudio(audioName, AudioType.BGM);
        if (audio == null)
            return;

        StopBGM();

        currentBGMPlaying = audio.Source;
        audio.Source.Play();
    }

    //public void SwitchBGM(string audioName)
    //{
    //    if (audioTransitioning) return;

    //    //while (!gameObject.activeInHierarchy)
    //    //    gameObject.SetActive(true);
        
    //    Debug.LogWarning(gameObject.activeInHierarchy);

    //    var audio = SearchAudio(audioName, AudioType.BGM);
    //    if (audio == null)
    //        return;

    //    nextAudioPlaying = audio.Source;
    //    StartCoroutine(CoroutineTest());
    //    StartCoroutine(FadeOut(currentBGMPlaying, 1f));
    //}

    //IEnumerator CoroutineTest()
    //{
    //    print("jasdas");

    //    yield return null;
    //}

    public void StopBGM()
    {
        //if (audioTransitioning) return;

        //foreach (var a in BGM)
        //    a.AObject.GetComponent<AudioSource>().Stop();

        foreach (var a in audios)
        {
            if (a.AudioType == AudioType.BGM)
                a.Source.Stop();
        }

        currentBGMPlaying = null;
    }

    public void StopBGM(string audioName)
    {
        //if (audioTransitioning) return;

        //var bgm = BGM.FirstOrDefault(s => s.AudioName == audioName);
        //bgm.AObject.GetComponent<AudioSource>().Stop();

        var audio = SearchAudio(audioName, AudioType.BGM);
        if (audio == null)
            return;

        audio.Source.Stop();
        currentBGMPlaying = null;
    }

    public void PlaySFX(string audioName)
    {
        //if (audioTransitioning) return;

        //var sfx = SFX.FirstOrDefault(s => s.AudioName == audioName);
        //sfx.AObject.GetComponent<AudioSource>().Play();

        var audio = SearchAudio(audioName, AudioType.SFX);
        if (audio == null)
            return;

        audio.Source.Play();
    }

    public void StopSFX()
    {
        //if (audioTransitioning) return;

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
        //if (audioTransitioning) return;

        //var sfx = SFX.FirstOrDefault(s => s.AudioName == audioName);
        //sfx.AObject.GetComponent<AudioSource>().Stop();

        var audio = SearchAudio(audioName, AudioType.SFX);
        if (audio == null)
            return;

        audio.Source.Stop();
    }

    public void SetLowpass()
    {
        bool isPaused = GameManager.Instance.CompareStatus(GameStatus.PAUSE);

        if (isPaused) mixer.SetFloat("LowpassFreq", 500f);
        else mixer.SetFloat("LowpassFreq", 22000f);
    }

    //IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    //{
    //    audioTransitioning = true;

    //    float startVolume = audioSource.volume;

    //    while (audioSource.volume > 0)
    //    {
    //        audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

    //        yield return null;
    //    }

    //    audioSource.Stop();
    //    audioSource.volume = startVolume;

    //    yield return new WaitForSeconds(0.5f);

    //    StartCoroutine(FadeIn(nextAudioPlaying, 1f));
    //    StopCoroutine("FadeOut");
    //}

    //IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    //{
    //    float startVolume = 0.2f;

    //    audioSource.volume = 0;
    //    audioSource.Play();

    //    while (audioSource.volume < 1.0f)
    //    {
    //        audioSource.volume += startVolume * Time.deltaTime / FadeTime;

    //        yield return null;
    //    }

    //    audioSource.volume = 1f;

    //    nextAudioPlaying = null;
    //    currentBGMPlaying = audioSource;
    //}

    //private void OnDisable()
    //{
    //    print("Audio Controller is deactivated, activating...");
    //    gameObject.SetActive(true);
    //}
}
