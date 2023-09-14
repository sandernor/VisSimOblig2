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
    private List <string> line;
    private int lineCount = 0;
    private List <Vertex> vertexArray;
    private Vertex v;


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
            Debug.Log("Linje " + i + " : " + line[i]);
        }
    }

    private void GenerateVertices()
    {
       int numVertices = int.Parse(line[0]);
       for (int i = 0; i < numVertices; i++) 
       {
            v = new Vertex();

            vertexArray.Add(v);
       }
    }

    private void Start()
    {
        line = new List<string> ();
        vertexArray = new List<Vertex>();
        FileToLines();
    }







}
