using UnityEngine;

[DisallowMultipleComponent]
public class AmmoHitEffect : MonoBehaviour
{
    private ParticleSystem ammoHitEffectParticleSystem;

    private void Awake()
    {
        ammoHitEffectParticleSystem = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// Set Ammo Hit Effect from passed in AmmoHitEffectSO details
    /// </summary>
    public void SetHitEffect(AmmoHitEffectSO ammoHitEffect)
    {
        // Set hit effect color gradient
        SetHitEffectColorGradient(ammoHitEffect.colorGradient);

        // Set hit effect particle system starting values
        SetHitEffectParticleStartingValues(ammoHitEffect.duration, ammoHitEffect.startParticleSize, ammoHitEffect.startParticleSpeed, ammoHitEffect.startLifetime, ammoHitEffect.effectGravity, ammoHitEffect.maxParticleNumber);

        // Set hit effect particle system particle burst particle number
        SetHitEffectParticleEmission(ammoHitEffect.emissionRate, ammoHitEffect.burstParticleNumber);

        // Set hit effect particle sprite
        SetHitEffectParticleSprite(ammoHitEffect.sprite);

        // Set hit effect lifetime min and max velocities
        SetHitEffectVelocityOverLifeTime(ammoHitEffect.velocityOverLifetimeMin, ammoHitEffect.velocityOverLifetimeMax);

    }

    /// <summary>
    /// Set the hit effect particle system color gradient
    /// </summary>
    private void SetHitEffectColorGradient(Gradient gradient)
    {
        // Set colour gradient
        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = ammoHitEffectParticleSystem.colorOverLifetime;
        colorOverLifetimeModule.color = gradient;
    }


    /// <summary>
    /// Set hit effect particle system starting values
    /// </summary>
    private void SetHitEffectParticleStartingValues(float duration, float startParticleSize, float startParticleSpeed, float startLifetime, float effectGravity, int maxParticles)
    {
        ParticleSystem.MainModule mainModule = ammoHitEffectParticleSystem.main;

        // Set particle system duration
        mainModule.duration = duration;

        // Set particle start size
        mainModule.startSize = startParticleSize;

        // Set particle start speed
        mainModule.startSpeed = startParticleSpeed;

        // Set particle start lifetime
        mainModule.startLifetime = startLifetime;

        // Set particle starting gravity
        mainModule.gravityModifier = effectGravity;

        // Set max particles
        mainModule.maxParticles = maxParticles;
    }

    /// <summary>
    /// Set hit effect particle system particle burst particle number
    /// </summary>
    private void SetHitEffectParticleEmission(int emissionRate, float burstParticleNumber)
    {
        ParticleSystem.EmissionModule emissionModule = ammoHitEffectParticleSystem.emission;

        // Set particle burst number
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0f, burstParticleNumber);
        emissionModule.SetBurst(0, burst);

        // Set particle emission rate
        emissionModule.rateOverTime = emissionRate;
    }

    /// <summary>
    /// Set hit effect particle system sprite
    /// </summary>
    private void SetHitEffectParticleSprite(Sprite sprite)
    {
        // Set particle burst number
        ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = ammoHitEffectParticleSystem.textureSheetAnimation;

        textureSheetAnimationModule.SetSprite(0, sprite);
    }

    /// <summary>
    /// Set the hit effect velocity over lifetime
    /// </summary>
    private void SetHitEffectVelocityOverLifeTime(Vector3 minVelocity, Vector3 maxVelocity)
    {
        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = ammoHitEffectParticleSystem.velocityOverLifetime;

        // Define min max X velocity
        ParticleSystem.MinMaxCurve minMaxCurveX = new ParticleSystem.MinMaxCurve();
        minMaxCurveX.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveX.constantMin = minVelocity.x;
        minMaxCurveX.constantMax = maxVelocity.x;
        velocityOverLifetimeModule.x = minMaxCurveX;

        // Define min max Y velocity
        ParticleSystem.MinMaxCurve minMaxCurveY = new ParticleSystem.MinMaxCurve();
        minMaxCurveY.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveY.constantMin = minVelocity.y;
        minMaxCurveY.constantMax = maxVelocity.y;
        velocityOverLifetimeModule.y = minMaxCurveY;

        // Define min max Z velocity
        ParticleSystem.MinMaxCurve minMaxCurveZ = new ParticleSystem.MinMaxCurve();
        minMaxCurveZ.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveZ.constantMin = minVelocity.z;
        minMaxCurveZ.constantMax = maxVelocity.z;
        velocityOverLifetimeModule.z = minMaxCurveZ;

    }

}