using UnityEngine;

public class AudioManager : Reference
{

    [Header("Audio Source")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip bradleySweepRVoice;

    public AudioClip bradleySweepLVoice;

    public AudioClip bradleyPoundVoice;

    public AudioClip crumbPickup;

    public AudioClip hitEnemy;

    public AudioClip poundAttackSFX;

    public AudioClip hitCollision;

    public AudioClip bradleyGetsHurt;

    public AudioClip powerUpPickUp;

    public AudioClip healItemPickup;

    public AudioClip dashSfx;

    public AudioClip deathSfx;

    public AudioClip gameOverScreenSFX;
    public AudioClip calorieSFX;

    public AudioClip enemyThankYousfx;

    public AudioClip walkingSound;

    private float masterVolume_value;
    private float effectsVolume_value;
    private float musicVolume_value;


    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        SFXSource.PlayOneShot(clip);
    }

    public void PlaySFXOnRepeat(AudioClip clip)
    {
        if (clip == null) return;
        SFXSource.Stop();
        SFXSource.clip = clip;
        SFXSource.loop = true;
        SFXSource.Play();
    }

    public void StopSFXLoop()
    {
        SFXSource.loop = false;
        SFXSource.Stop();
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        CalculateMusicSourceVolume();
        CalculateSPXSourceVolume();
    }
    public void CalculateMusicSourceVolume() {
        masterVolume_value = GlobalReference.Settings.Get<float>("master_volume");
        musicVolume_value = GlobalReference.Settings.Get<float>("music_volume");
        musicSource.volume = masterVolume_value * musicVolume_value;
    }
    public void CalculateSPXSourceVolume() {
        masterVolume_value = GlobalReference.Settings.Get<float>("master_volume");
        effectsVolume_value = GlobalReference.Settings.Get<float>("effects_volume");
        SFXSource.volume = masterVolume_value * effectsVolume_value;
    }

}