using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float hp = 100;

    public SkinnedMeshRenderer colorRenderer;
    public Color currentColor;
    public Color hitColor;

    public void hit(float damage)
    {
        hp -= damage;
        StartCoroutine("colorSet");
    }
    public IEnumerator colorSet()
    {
        Debug.Log(colorRenderer.materials.Length);
        foreach(Material material in colorRenderer.materials)
        {
            material.color = hitColor;
        }

        yield return new WaitForSeconds(0.4f);

        foreach (Material material in colorRenderer.materials)
        {
            material.color = currentColor;
        }
    }
}
