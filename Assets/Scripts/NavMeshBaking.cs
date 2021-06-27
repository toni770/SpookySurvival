using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaking : MonoBehaviour
{
    NavMeshSurface surface;
    // Start is called before the first frame update

    public void setSurface(NavMeshSurface sur)
    {
        surface = sur;
    }
    public void Bake()
    {
        surface.BuildNavMesh();
    }
}
