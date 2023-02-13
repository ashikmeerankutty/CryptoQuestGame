using UnityEngine;

[DisallowMultipleComponent]
public class WeaponShootEffect : MonoBehaviour
{
    private ParticleSystem shootEffectParticleSystem;

    private void Awake()
    {
        // Load components
        shootEffectParticleSystem = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// Set the Shoot Effect from the passed in WeaponShootEffectSO and aimAngle
    /// </summary>
    public void SetShootEffect(WeaponShootEffectSO shootEffect, float aimAngle)
    {
        // Set shoot effect color gradient
        SetShootEffectColorGradient(shootEffect.colorGradient);

        // Set shoot effect particle system starting values
        SetShootEffectParticleStartingValues(shootEffect.duration, shootEffect.startParticleSize, shootEffect.startParticleSpeed, shootEffect.startLifetime, shootEffect.effectGravity, shootEffect.maxParticleNumber);

        // Set shoot effect particle system particle burst particle number
        SetShootEffectParticleEmission(shootEffect.emissionRate, shootEffect.burstParticleNumber);

        // Set emmitter rotation
        SetEmmitterRotation(aimAngle);

        // Set shoot effect particle sprite
        SetShootEffectParticleSprite(shootEffect.sprite);

        // Set shoot effect lifetime min and max velocities
        SetShootEffectVelocityOverLifeTime(shootEffect.velocityOverLifetimeMin, shootEffect.velocityOverLifetimeMax);

    }

    /// <summary>
    /// Set the shoot effect particle system color gradient
    /// </summary>
    private void SetShootEffectColorGradient(Gradient gradient)
    {
        // Set colour gradient
        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = shootEffectParticleSystem.colorOverLifetime;
        colorOverLifetimeModule.color = gradient;
    }

    /// <summary>
    /// Set shoot effect particle system starting values
    /// </summary>
    private void SetShootEffectParticleStartingValues(float duration, float startParticleSize, float startParticleSpeed, float startLifetime, float effectGravity, int maxParticles)
    {
        ParticleSystem.MainModule mainModule = shootEffectParticleSystem.main;

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
    /// Set shoot effect particle system particle burst particle number
    /// </summary>
    private void SetShootEffectParticleEmission(int emissionRate, float burstParticleNumber)
    {
        ParticleSystem.EmissionModule emissionModule = shootEffectParticleSystem.emission;

        // Set particle burst number
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0f, burstParticleNumber);
        emissionModule.SetBurst(0, burst);

        // Set particle emission rate
        emissionModule.rateOverTime = emissionRate;
    }

    /// <summary>
    /// Set shoot effect particle system sprite
    /// </summary>
    private void SetShootEffectParticleSprite(Sprite sprite)
    {
        // Set particle burst number
        ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = shootEffectParticleSystem.textureSheetAnimation;

        textureSheetAnimationModule.SetSprite(0, sprite);

    }

    /// <summary>
    /// Set the rotation of the emmitter to match the aim angle
    /// </summary>
    private void SetEmmitterRotation(float aimAngle)
    {
        transform.eulerAngles = new Vector3(0f, 0f, aimAngle);
    }

    /// <summary>
    /// Set the shoot effect velocity over lifetime
    /// </summary>
    private void SetShootEffectVelocityOverLifeTime(Vector3 minVelocity, Vector3 maxVelocity)
    {
        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = shootEffectParticleSystem.velocityOverLifetime;

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