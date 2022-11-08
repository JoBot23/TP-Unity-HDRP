using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public Transform target;
    public Transform[] Bones;
    public int NbBonesLimit = 4;

    void Update()
    {
        doCCD(target.position, 1f);
    }

    private void doCCD(Vector3 targetPos, float blending)
    {
        for (int i = 1; i < Bones.Length && i <= NbBonesLimit; i++)
        {
            Vector3 directionActuelle = (Bones[0].position - Bones[i].position).normalized;
            Vector3 directionDesiree = (targetPos - Bones[i].position).normalized;
            Quaternion rotation = Quaternion.FromToRotation(directionActuelle, directionDesiree.normalized);
            Bones[i].rotation = Quaternion.Lerp(Bones[i].rotation, (rotation) * Bones[i].rotation, blending);
        }
    }
}
