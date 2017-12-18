using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupContext : MonoBehaviour
{
    public Voxel VoxelPrefab;
    public MeshTable MeshTable;

    private Voxel[] _voxels;

	// Use this for initialization
	void Start ()
    {
        // create voxels
        _voxels = new Voxel[10];
        for(int i = 0; i < _voxels.Length; i++)
        {
            var voxel = Instantiate(VoxelPrefab);
            // set voxel position

            //voxel.MeshTable = MeshTable;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
