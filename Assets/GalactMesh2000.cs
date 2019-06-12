using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalactMesh2000 : MonoBehaviour
{
    public Transform[] _tout;
    void Start()
    {
         _tout = this.GetComponentsInChildren<Transform>();
        foreach (Transform obj in _tout) // C'est à dire 31000 mesh
        {
            if (obj.gameObject.GetComponent<MeshFilter>() != null)
            {
                MeshCollider mesh;
                mesh = obj.gameObject.AddComponent<MeshCollider>();
                mesh.sharedMesh = obj.gameObject.GetComponent<MeshFilter>().mesh;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
