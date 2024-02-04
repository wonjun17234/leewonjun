using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Character : MonoBehaviour
{
    public float hp = 100;

    public SkinnedMeshRenderer colorRenderer;
    public Color currentColor;
    public Color hitColor;
    public GameObject volume;

    public virtual void hit(float damage)
    {
        hp -= damage;
        StartCoroutine("colorSet");
    }
    public IEnumerator colorSet()
    {
        foreach(Material material in colorRenderer.materials)
        {
            material.color = hitColor;
        }
        if(GetComponent<PlayerController>() != null)
        {
            volume.GetComponent<PostProcessVolume>().enabled = true;
        }
        yield return new WaitForSeconds(0.4f);
        if (GetComponent<PlayerController>() != null)
        {
            volume.GetComponent<PostProcessVolume>().enabled = false;
        }
        foreach (Material material in colorRenderer.materials)
        {
            material.color = currentColor;
        }
    }
}
