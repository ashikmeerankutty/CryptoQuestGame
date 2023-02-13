using UnityEngine;
using UnityEngine.Rendering.Universal; // keep this namespace in for when 2D lights become non experimental;
using UnityEngine.Experimental.Rendering.Universal;

[DisallowMultipleComponent]
public class LightFlicker : MonoBehaviour
{
    private Light2D light2D;
    [SerializeField] private float lightIntensityMin;
    [SerializeField] private float lightIntensityMax;
    [SerializeField] private float lightFlickerTimeMin;
    [SerializeField] private float lightFlickerTimeMax;
    private float lightFlickerTimer;

    private void Awake()
    {
        // Load components
        light2D = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        lightFlickerTimer = Random.Range(lightFlickerTimeMin, lightFlickerTimeMax);
    }

    private void Update()
    {
        if (light2D == null) return;

        lightFlickerTimer -= Time.deltaTime;

        if (lightFlickerTimer < 0f)
        {
            lightFlickerTimer = Random.Range(lightFlickerTimeMin, lightFlickerTimeMax);

            RandomiseLightIntensity();
        }
    }

    private void RandomiseLightIntensity()
    {
        light2D.intensity = Random.Range(lightIntensityMin, lightIntensityMax);
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(lightIntensityMin), lightIntensityMin, nameof(lightIntensityMax), lightIntensityMax, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(lightFlickerTimeMin), lightFlickerTimeMin, nameof(lightFlickerTimeMax), lightFlickerTimeMax, false);
    }
#endif
    #endregion
}
