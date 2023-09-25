using Microsoft.SqlServer.Server;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    private float g = -9.81f;
    private float m = 0.014f;
    private float G;

    private Vector3 curVel;
    private Vector3 newVel;
    private Vector3 acceleration;
    private Vector3 newPos;
    private Vector3 prevPos;
    private Vector3 N;
    private Vector3 prevN;

    public GameObject triGen;
    private TriangleGen genScript;

    private Vector3 pos;

    RaycastHit hit;

    private void Awake()
    {
        G = m * g;
        curVel = new Vector3(0f, G, 0f);
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
            prevN = N;
        }
        Debug.DrawRay(transform.position, N * 2000f, Color.red);
        if (WhatTri() != -1) 
        {
            if (Grounded())
            {
                newVel = newVel + N;
            }
        }

        newPos = newVel * Time.fixedDeltaTime;
        curVel = newVel;
        return newPos;
    }

    private bool Grounded()
    {
        Vector3 P = transform.position;
        float r = 0.02f;
        Vector3 C = genScript.tris[WhatTri()].vertices[0];
        Vector3 norm = Normal(genScript.tris[WhatTri()].vertices[0], genScript.tris[WhatTri()].vertices[1], genScript.tris[WhatTri()].vertices[2]);
        Vector3 y = Vector3.Dot(P - C, norm) * norm;
        if (y.magnitude <= r)
        {
            return true;
        }
        else
            return false;
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

    public Vector3 baryCoords(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
    {
        Vector3 u = B - A;
        Vector3 v = C - A;
        Vector3 w = P - A;

        Vector3 vCrossW = Vector3.Cross(v, w);
        Vector3 vCrossU = Vector3.Cross(v, u);

        Vector3 uCrossW = Vector3.Cross(u, w);
        Vector3 uCrossV = Vector3.Cross(u, v);

        Vector3 result = new Vector3();
        float denom = uCrossV.magnitude;
        result.y = vCrossW.magnitude / denom;
        result.z = uCrossW.magnitude / denom;
        result.x = 1f - result.y - result.z;

        return result;
    }

    public Vector3 Normal(Vector3 A, Vector3 B, Vector3 C)
    {
        Vector3 u = B - A;
        Vector3 v = C - A;

        Vector3 norm = Vector3.Normalize(Vector3.Cross(u, v));

        return norm;
    }
}
