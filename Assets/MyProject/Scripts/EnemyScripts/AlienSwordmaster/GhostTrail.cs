using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    [SerializeField] private float activeTime = 4f;
    [SerializeField] private float meshDestroyDelay = 0.5f;
    [SerializeField] private float meshRefreshRate = 1f;
    [SerializeField] private Material[] material;
    [SerializeField] private string shaderVarRef;
    [SerializeField] private float shaderVarRate = 0.1f;
    [SerializeField] private float shaderVarRefreshRate = 0.05f;
    private bool isTrailActive;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock) && !isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        }
    }

    private IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;
            if (skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject obj = new GameObject();
                obj.transform.SetPositionAndRotation(transform.position, transform.rotation);

                MeshRenderer mR = obj.AddComponent<MeshRenderer>();
                MeshFilter mF = obj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);
                mF.mesh = mesh;
                mR.SetMaterials(material.ToList());

                StartCoroutine(AnimateMaterialFloat(mR.materials, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(obj, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
    }

    private IEnumerator AnimateMaterialFloat(Material[] mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat[0].GetFloat(shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat[0].SetFloat(shaderVarRef, valueToAnimate);
            mat[1].SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
