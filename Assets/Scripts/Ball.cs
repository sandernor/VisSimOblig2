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

    //private float oldVel;
    //private float newVel;
    //private float curVel;

    private Vector3 curVel;
    private Vector3 newVel;
    private Vector3 acceleration;
    private Vector3 newPos;
    private Vector3 prevPos;
    private Vector3 N;

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
        curVel = new Vector3(0f, G, 0f);
        prevPos = transform.position;
        acceleration = new Vector3(0f, G, 0f);
        genScript = triGen.GetComponent("TriangleGen") as TriangleGen;
    }
    private void FixedUpdate()
    {
        transform.position += calcPos();
        //Debug.Log(curVel.ToString());
    }

    private Vector3 calcPos()
    {
        Debug.Log(WhatTri());
        newVel = curVel + acceleration * Time.fixedDeltaTime;
        if (WhatTri() != -1)
        {
            N = Vector3.Dot(Normal(genScript.tris[WhatTri()].vertices[0], genScript.tris[WhatTri()].vertices[1], genScript.tris[WhatTri()].vertices[2]), -newVel) * Normal(genScript.tris[WhatTri()].vertices[0], genScript.tris[WhatTri()].vertices[1], genScript.tris[WhatTri()].vertices[2]);

        }
        else N = Vector3.zero;
        Debug.DrawRay(transform.position, N*2000f, Color.red);
        newVel = newVel + N;
        newPos = prevPos + newVel * Time.fixedDeltaTime;
        prevPos = newPos;
        curVel = newVel;
        prevPos = newPos;
        return newPos;
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

    public bool inTri(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
    {
        Vector3 u = B - A;
        Vector3 v = C - A;
        Vector3 w = P - A;

        Vector3 vCrossW = Vector3.Cross(v, w);
        Vector3 vCrossU = Vector3.Cross(v, u);

        if (Vector3.Dot(vCrossW, vCrossU) < 0)
            return false;

        Vector3 uCrossW = Vector3.Cross(u, w);
        Vector3 uCrossV = Vector3.Cross(u, v);

        if (Vector3.Dot(uCrossW, uCrossV) < 0)
            return false;

        float denom = uCrossV.magnitude;
        float r = vCrossW.magnitude / denom;
        float t = uCrossW.magnitude / denom;

        return (r >= 0 && t >= 0 && r + t <= 1);
    }

    public Vector3 Normal(Vector3 A, Vector3 B, Vector3 C)
    {
        Vector3 u = B - A;
        Vector3 v = C - A;

        Vector3 norm = Vector3.Normalize(Vector3.Cross(u, v));

        return norm;
    }
}
