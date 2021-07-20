using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    const string name = "newMesh";
    private static int Dimension = 10;
    private static float UVScale = 2f;
    public static Mesh Generate(int dim, float uvsc)
    {
        Dimension = dim;
        UVScale = uvsc;

        //Mesh Setup
        Mesh Mesh = new Mesh();
        Mesh.name = name;

        Mesh.vertices = GenerateVerts();
        Mesh.triangles = GenerateTries(Mesh.vertices.Length);
        Mesh.uv = GenerateUVs(Mesh.vertices.Length);
        Mesh.RecalculateNormals();
        Mesh.RecalculateBounds();
        Mesh.Optimize();
        //Mesh.MarkDynamic();

        return Mesh;

       
        
    }

    private static Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(Dimension + 1) * (Dimension + 1)];

        //equaly distributed verts
        for (int x = 0; x <= Dimension; x++)
            for (int z = 0; z <= Dimension; z++)
                verts[index(x, z)] = new Vector3(x, 0, z);

        return verts;
    }

    private static int[] GenerateTries(int meshLength)
    {
        var tries = new int[meshLength * 6];

        //two triangles are one tile
        for (int x = 0; x < Dimension; x++)
        {
            for (int z = 0; z < Dimension; z++)
            {
                tries[index(x, z) * 6 + 0] = index(x, z);
                tries[index(x, z) * 6 + 1] = index(x + 1, z + 1);
                tries[index(x, z) * 6 + 2] = index(x + 1, z);
                tries[index(x, z) * 6 + 3] = index(x, z);
                tries[index(x, z) * 6 + 4] = index(x, z + 1);
                tries[index(x, z) * 6 + 5] = index(x + 1, z + 1);
            }
        }

        return tries;
    }


    private static Vector2[] GenerateUVs(int meshLength)
    {
        var uvs = new Vector2[meshLength];

        //always set one uv over n tiles than flip the uv and set it again
        for (int x = 0; x <= Dimension; x++)
        {
            for (int z = 0; z <= Dimension; z++)
            {
                var vec = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                uvs[index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }

        return uvs;
    }

    private static int index(int x, int z)
    {
        return x * (Dimension + 1) + z;
    }
}
