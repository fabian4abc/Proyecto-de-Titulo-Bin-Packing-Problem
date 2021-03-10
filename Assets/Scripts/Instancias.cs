
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using UnityEditor;


public class Instancias : MonoBehaviour
{
    // Start is called before the first frame update
    private int pieceID = 0;
    public string fileName;
    private List<float> numeros;

    private float factor;


    // Use this for initialization
    void Awake()
    {
        StreamReader reader = new StreamReader("Assets/Files/" + fileName);
        string itemStrings = reader.ReadLine();
        string line;

        using (StreamReader sr = new StreamReader("Assets/Files/" + fileName)) {
            // Read the stream to a string, and write the string to the console.
            line = sr.ReadToEnd();
        }
        //print(line);
        string[] values;
        values = line.Split(' ');
        numeros = new List<float>();
        for (int ii = 0; ii < values.Length; ii++) {
            numeros.Add(float.Parse(values[ii]));
        }
        
        factor = 1f;
        

    }

    void Start() {
        Crear();
    }

    // Update is called once per frame
    void Update() { }  

    void Crear()
    {
        GameObject piece = new GameObject("Empty");
        pieceID++;
        piece.name = "Piece" + pieceID.ToString();

        int nVertices = numeros.Count / 2;
        var vertices2D = new Vector2[nVertices];
        int ind = 0;
        for (int i = 0; i <= (2*nVertices)-2; i = i + 2)
        {
            vertices2D[ind] = new Vector2(numeros[i] * factor, numeros[i + 1] * factor);
            ind++;
        }

        var vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);

        var triangulator = new Triangulator(vertices2D);
        var indices = triangulator.Triangulate();

        MeshRenderer meshRenderer = piece.AddComponent<MeshRenderer>();
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = Color.green;
        meshRenderer.material = mat;

        MeshFilter filter = piece.AddComponent<MeshFilter>();
        Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices3D;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        filter.mesh = mesh;
        //
        //Rigidbody2D rb2D = piece.AddComponent<Rigidbody2D>();
        //rb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Get triangles and vertices from mesh
        int[] triangles = piece.GetComponent<MeshFilter>().mesh.triangles;
        Vector3[] vertices = piece.GetComponent<MeshFilter>().mesh.vertices;
        
        
        // Get just the outer edges from the mesh's triangles (ignore or remove any shared edges)
        Dictionary<string, KeyValuePair<int, int>> edges = new Dictionary<string, KeyValuePair<int, int>>();
        for (int i = 0; i < triangles.Length; i += 3) {
            for (int e = 0; e < 3; e++) {
                int vert1 = triangles[i + e];
                int vert2 = triangles[i + e + 1 > i + 2 ? i : i + e + 1];
                string edge = Mathf.Min(vert1, vert2) + ":" + Mathf.Max(vert1, vert2);
                if (edges.ContainsKey(edge)) {
                    edges.Remove(edge);
                }
                else {
                    edges.Add(edge, new KeyValuePair<int, int>(vert1, vert2));
                }
            }
        }

        // Create edge lookup (Key is first vertex, Value is second vertex, of each edge)
        Dictionary<int, int> lookup = new Dictionary<int, int>();
        foreach (KeyValuePair<int, int> edge in edges.Values) {
            if (lookup.ContainsKey(edge.Key) == false) {
                lookup.Add(edge.Key, edge.Value);
            }
        }

        // Create empty polygon collider
        PolygonCollider2D polygonCollider = piece.AddComponent<PolygonCollider2D>();
        polygonCollider.pathCount = 0;

        // Loop through edge vertices in order
        int startVert = 0;
        int nextVert = startVert;
        int highestVert = startVert;
        List<Vector2> colliderPath = new List<Vector2>();
        while (true) {
            // Add vertex to collider path
            colliderPath.Add(vertices[nextVert]);
            // Get next vertex
            nextVert = lookup[nextVert];
            // Store highest vertex (to know what shape to move to next)
            if (nextVert > highestVert) {
                highestVert = nextVert;
            }

            // Shape complete
            if (nextVert == startVert)
            {
                // Add path to polygon collider
                polygonCollider.pathCount++;
                polygonCollider.SetPath(polygonCollider.pathCount - 1, colliderPath.ToArray());
                colliderPath.Clear();

                // Go to next shape if one exists
                if (lookup.ContainsKey(highestVert + 1)) {
                    // Set starting and next vertices
                    startVert = highestVert + 1;
                    nextVert = startVert;
                    // Continue to next loop
                    continue;
                }
                // No more verts
                break;
            }
        }
        //
        

        /*string path = "./Assets/Prefabs/Inte1.obj";
        StreamWriter w = new StreamWriter(path, false);
        string linea = "o P1" + "\n";
        w.WriteLine(linea);
        linea = "g P1" + "\n";
        w.WriteLine(linea);
        for (int i = 0;i < numeros.Count; i = i+2)
        {
            linea = "v " + numeros[i]*factor + " " + numeros[i + 1]*factor + " 0\n";
            w.WriteLine(linea);
        }

        for (int i = 0; i < indices.Length; i+=3)
        {
            linea = "f " + indices[i] + " " + indices[i+1] + " " + indices[i+2] + "\n";
            w.WriteLine(linea);
        }
        w.Close();

        string path2 = "./Assets/Prefabs/Inte1.prefab";
        PrefabUtility.SaveAsPrefabAsset(piece, path2);
        */
    }

    
}
