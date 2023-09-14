using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class TriangleGen : MonoBehaviour
{
    //private static string path = "Assets/VertexData.txt";

    //public TextAsset vertexData = Resources.Load<TextAsset>(path);

    public TextAsset vertexData;
    private char[] vertexDataArray;
    private List<string> line;
    private int lineCount = 0;
    private List<Vertex> vertexArray;
    private Vertex v;
    private Mesh mesh;
    private List<int> triangles;
    private int[] triVerts;


    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void MeshUpdate()
    {
        mesh.Clear();
        Vector3[] vertPos = new Vector3[] { vertexArray[0].position, vertexArray[1].position, vertexArray[2].position, vertexArray[3].position, vertexArray[4].position, vertexArray[5].position };
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

            v = new Vertex();
            v.position = new Vector3(x, y, z);
            vertexArray.Add(v);
            //Debug.Log(v.position);

        }
    }

    private void GenerateTriangles()
    {
        int numVertices = int.Parse(line[0]);
        int numTriangles = int.Parse(line[numVertices + 1]);
        //for (int i = numVertices + 2; i < numVertices + 2 + numTriangles; i+=1)
        //{
        //    triangles.Add(int.Parse(line[i]));
        //    Debug.Log(numTriangles);
        //}

        triVerts = new int[] {int.Parse(line[numVertices + 2]), int.Parse(line[numVertices + 3]), int.Parse(line[numVertices + 4]), int.Parse(line[numVertices + 5]), int.Parse(line[numVertices + 6]), int.Parse(line[numVertices + 7]), int.Parse(line[numVertices + 8]), int.Parse(line[numVertices + 9]), int.Parse(line[numVertices + 10]), int.Parse(line[numVertices + 11]), int.Parse(line[numVertices + 12]), int.Parse(line[numVertices + 13])};

        //for (int i = 0; i < lineCount; i++)
        //{
        //    //Debug.Log("Linje " + i + " : " + line[i]);
        //}

    }

    private void Start()
    {
        line = new List<string> ();
        vertexArray = new List<Vertex>();
        triangles = new List<int> ();
        FileToLines();
        GenerateVertices();
        GenerateTriangles();
        MeshUpdate();
    }







}
