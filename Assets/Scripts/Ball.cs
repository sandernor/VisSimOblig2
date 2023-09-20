using Microsoft.SqlServer.Server;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float g = -9.81f;
    private float m = 0.005f;
    private float G;

    private float oldVel;
    private float newVel;
    private float curVel;

    public GameObject triGen;
    private TriangleGen genScript;

    public Vector3 Nt1;
    public Vector3 Nt2;
    public Vector3 Nt3;
    public Vector3 Nt4;

    private Vector3 pos;

    RaycastHit hit;

    private void Awake()
    {
        G = m * g;
        curVel = G;
        genScript = triGen.GetComponent("TriangleGen") as TriangleGen;
    }
    private void FixedUpdate()
    {
        //G += g * Time.deltaTime;
        //Debug.Log(G);
        transform.position += calcPos();
        //if(WhatTri() != -1) Debug.Log(WhatTri());
        Debug.Log(curVel.ToString());
    }

    private Vector3 calcPos()
    {
        //Debug.Log(Grounded());
        //if(Grounded())
        //{
        //    float sin1 = Mathf.Sin(10.9f / 40f);
        //    Vector3 N = (G * Nt1 * sin1) * -1;
        //    Vector3 pos = N;
        //    pos.y += G * 0.3f;
        //    return pos;
        //}
        //else
        newVel = curVel + G * Time.fixedDeltaTime;
        curVel = newVel;
        {
            Vector3 pos = new Vector3(0, newVel, 0);
            return pos;
        }
    }

    private int WhatTri()
    {
        //Debug.Log(genScript.triCount);
        for (int i = 0; i < genScript.triCount; i++) 
        {
            if (inTri(genScript.tris[i].vertices[0], genScript.tris[i].vertices[1], genScript.tris[i].vertices[2], transform.position))
            {
                return i;
            }
            else continue;
        }
        return -1;
    }

    //private int Sort()
    //{

    //}

    //public bool inTri(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
    //{
    //    // Prepare our barycentric variables
    //    Vector3 u = B - A;
    //    Vector3 v = C - A;
    //    Vector3 w = P - A;

    //    Vector3 vCrossW = Vector3.Cross(v, w);
    //    Vector3 vCrossU = Vector3.Cross(v, u);
      
    //    if (Vector3.Dot(vCrossW, vCrossU) < 0)
    //        return false;

    //    Vector3 uCrossW = Vector3.Cross(u, w);
    //    Vector3 uCrossV = Vector3.Cross(u, v);

    //    // Test sign of t
    //    if (Vector3.Dot(uCrossW, uCrossV) < 0)
    //        return false;

    //    // At this piont, we know that r and t and both > 0
    //    float denom = uCrossV.magnitude;
    //    float r = vCrossW.magnitude / denom;
    //    float t = uCrossW.magnitude / denom;

    //    return (r + t <= 1);
    //}

    public bool inTri(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
    {
        // Prepare our barycentric variables
        //float BAz = B.z - A.z;
        //float BAx = B.x - A.x;
        //float CAz = C.z - A.z;
        //float CAx = C.x - A.x;
        //float PAz = P.z - A.z;

        //float w1 = (A.x * (CAz) + (PAz) * (CAx) - P.x * (CAz)) / (BAz) * (CAx) - (BAx) * (CAz);
        //float w2 = (P.z - A.z - w1 * (BAz)) / (CAz);

        Vector3 u = B - A;
        Vector3 v = C - A;
        Vector3 w = P - A;

        Vector3 vCrossW = Vector3.Cross(v, w);
        Vector3 vCrossU = Vector3.Cross(v, u);

        if (Vector3.Dot(vCrossW, vCrossU) < 0)
            return false;

        Vector3 uCrossW = Vector3.Cross(u, w);
        Vector3 uCrossV = Vector3.Cross(u, v);

        //Test sign of t
        if (Vector3.Dot(uCrossW, uCrossV) < 0)
            return false;

        //At this piont, we know that r and t and both > 0
        float denom = uCrossV.magnitude;
        float r = vCrossW.magnitude / denom;
        float t = uCrossW.magnitude / denom;

        //if (w1 >= 0f && w2 >= 0f && (w1 + w2) <= 1f) 
        //{
        //    return true;
        //}
        //else return false;

        return (r >= 0 && t >= 0 && r + t <= 1);
    }
}
