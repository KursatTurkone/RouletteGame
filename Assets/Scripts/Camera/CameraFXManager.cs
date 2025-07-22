using UnityEngine;

public class CameraFXManager : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] private ParticleSystem winParticle;
    [SerializeField] private ParticleSystem loseParticle;
    [Header("Shake")]
    [SerializeField] private float shakeDuration = 0.25f;
    [SerializeField] private float shakeAmount = 0.5f;

    private Vector3 _originalPos;
    private float _shakeTime = 0f;

    void Awake()
    {
        _originalPos = transform.position;
    }

    void OnEnable()
    {
        GameEvents.OnWin += PlayWinFX;
        GameEvents.OnLose += PlayLoseFX;
    }

    void OnDisable()
    {
        GameEvents.OnWin -= PlayWinFX;
        GameEvents.OnLose -= PlayLoseFX;
    }

    void Update()
    {
        if (_shakeTime > 0)
        {
            transform.position = _originalPos + Random.insideUnitSphere * shakeAmount;
            _shakeTime -= Time.deltaTime;
            if (_shakeTime <= 0)
                transform.position = _originalPos;
        }
    }

    void PlayWinFX()
    {
        if (winParticle != null)
            winParticle.Play();
        _shakeTime = shakeDuration; 
    }

    void PlayLoseFX()
    {
        if (loseParticle != null)
            loseParticle.Play();
        _shakeTime = shakeDuration;
    }
}