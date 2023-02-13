using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterializeEffect : MonoBehaviour
{
    /// <summary>
    /// Materialize effect coroutine - used for the materialiuse special effect
    /// </summary>
    public IEnumerator MaterializeRoutine(Shader materializeShader, Color materializeColor, float materializeTime, SpriteRenderer[] spriteRendererArray, Material normalMaterial)
    {
        Material materializeMaterial = new Material(materializeShader);

        materializeMaterial.SetColor("_EmissionColor", materializeColor);

        // Set materialize material in sprite renderers
        foreach (SpriteRenderer spriteRenderer in spriteRendererArray)
        {
            spriteRenderer.material = materializeMaterial;
        }

        float dissolveAmount = 0f;

        // materialize enemy
        while (dissolveAmount < 1f)
        {
            dissolveAmount += Time.deltaTime / materializeTime;

            materializeMaterial.SetFloat("_DissolveAmount", dissolveAmount);

            yield return null;

        }


        // Set standard material in sprite renderers
        foreach (SpriteRenderer spriteRenderer in spriteRendererArray)
        {
            spriteRenderer.material = normalMaterial;
        }

    }
}
