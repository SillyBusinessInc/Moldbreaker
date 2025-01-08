using UnityEngine;

public class AudioManager : Reference
{

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

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

    public AudioClip jumpSound;

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
}