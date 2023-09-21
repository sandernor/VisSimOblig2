using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using JetBrains.Annotations;

public class TriangleGen : MonoBehaviour
{
    //private static string path = "Assets/VertexData.txt";

    //public TextAsset vertexData = Resources.Load<TextAsset>(path);

    public TextAsset vertexData;
    private char[] vertexDataArray;
    private List<string> line;
    private int lineCount = 0;
    private List<Vector3> vertexArray;
    private Vector3 v;
    private Mesh mesh;
    private List<int> triangles;
    private int[] triVerts;
    public Ball ball;
    public Triangle[] tris;
    public int triCount = 0;

    public struct Triangle
    {
        public int index;
        public Vector3[] vertices;

        public Triangle (int a, Vector3 b, Vector3 c, Vector3 d)
        {
            this.index = a;
            this.vertices = new Vector3[3];
            this.vertices[0] = b;
            this.vertices[1] = c;
            this.vertices[2] = d;
        }

    }




    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        line = new List<string>();
        vertexArray = new List<Vector3>();
        triangles = new List<int>();
        tris = new Triangle[4];
    }

    private void MeshUpdate()
    {
        mesh.Clear();
        Vector3[] vertPos = vertexArray.ToArray();
        mesh.vertices = vertPos;

        mesh.triangles = triVerts;
    }

    private void FileToLines()
    {
        string data = vertexData.ToString();

        System.IO.StreamReader file = new System.IO.StreamReader(Application.dataPath + "/VertexData.txt");

        while (file.ReadLine() != null)
        {
            lineCount += 1;
        }

        System.IO.StreamReader file2 = new System.IO.StreamReader(Application.dataPath + "/VertexData.txt");

        for (int i = 0; i < lineCount; i++)
        {
            line.Add(file2.ReadLine());
            //Debug.Log("Linje " + i + " : " + line[i]);
        }
    }

    private void GenerateVertices()
    {
       int numVertices = int.Parse(line[0]);
       for (int i = 1; i < numVertices+1; i++) 
       {
            string[] splitLine = line[i].Split(char.Parse(" "));

            float x, y, z;
            x = float.Parse(splitLine[0]);
            y = float.Parse(splitLine[1]);
            z = float.Parse(splitLine[2]);

            v = new Vector3(x, y, z);
            vertexArray.Add(v);

            //Debug.Log(v.position);

        }
    }

    private void GenerateTriangles()
    {
        int numVertices = int.Parse(line[0]);
        int numTriangles = int.Parse(line[numVertices + 1]);
        for (int i = numVertices + 2; i < numVertices + 2 + numTriangles; i++)
        {
            triangles.Add(int.Parse(line[i]));
            
            ///Debug.Log(numTriangles);
        }
        triVerts = triangles.ToArray();

        //Debug.Log(triVerts.Length);

        for (int i = 0; i < triVerts.Length; i += 3)
        {
            tris[i / 3] = new Triangle(i / 3, vertexArray[triVerts[i]], vertexArray[triVerts[i + 1]], vertexArray[triVerts[i + 2]]);
            triCount++;
        }


        //triVerts = new int[] {int.Parse(line[numVertices + 2]), int.Parse(line[numVertices + 3]), int.Parse(line[numVertices + 4]), int.Parse(line[numVertices + 5]), int.Parse(line[numVertices + 6]), int.Parse(line[numVertices + 7]), int.Parse(line[numVertices + 8]), int.Parse(line[numVertices + 9]), int.Parse(line[numVertices + 10]), int.Parse(line[numVertices + 11]), int.Parse(line[numVertices + 12]), int.Parse(line[numVertices + 13])};

        //for (int i = 0; i < lineCount; i++)
        //{
        //    //Debug.Log("Linje " + i + " : " + line[i]);
        //}

    }

    private void CalcNorms()
    {
        Vector3 NT0 = CrossProduct((vertexArray[0] - vertexArray[1]), (vertexArray[0] - vertexArray[2]));
        Vector3 NT1 = CrossProduct((vertexArray[1] - vertexArray[2]), (vertexArray[1] - vertexArray[3]));
        Vector3 NT2 = CrossProduct((vertexArray[2] - vertexArray[3]), (vertexArray[2] - vertexArray[4]));
        Vector3 NT3 = CrossProduct((vertexArray[2] - vertexArray[4]), (vertexArray[2] - vertexArray[5]));
        ball.Nt1 = NT0;
        ball.Nt2 = NT1;
        ball.Nt3 = NT2;
        ball.Nt4 = NT3;
        //float angle1 = Mathf.Asin(10.9f / 40f);
        //float angle2 = Mathf.Asin(7.9f / 40f);
        //float angle3 = Mathf.Asin(7.9f / 40f);
        //float angle4 = Mathf.Asin(7.8f / 40f);
    }

    public Vector3 CrossProduct(Vector3 v1, Vector3 v2)
    {
        float x, y, z;
        x = v1.y * v2.z - v2.y * v1.z;
        y = (v1.x * v2.z - v2.x * v1.z) * -1;
        z = v1.x * v2.y - v2.x * v1.y;

        Vector3 xvec = new Vector3(x, y, z);
        xvec = xvec.normalized; //optional
        return xvec;
    }

    private void Start()
    {
        FileToLines();
        GenerateVertices();
        GenerateTriangles();
        MeshUpdate();
        CalcNorms();
        //Debug.Log(triVerts.Length);
    }







}
