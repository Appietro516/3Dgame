﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Random = UnityEngine.Random;


public class CreatePath : MonoBehaviour {
	private TileMap tileMap;
	private int map_size;


	public GameObject core;

	public LTSpline ltpath;
	public int segments;

	public float path_y_offset = .5f;

	private Vector3 start = Vector3.zero;
	private Vector3 end = Vector3.zero;


	public static ArrayList enemies;



	// Use this for initialization
	void Awake () {
		tileMap = this.gameObject.GetComponent<TileMap>();
		this.gameObject.transform.position = new Vector3(-tileMap.size_x/2, 0,-tileMap.size_z/2);

		map_size = tileMap.size_x;
		enemies = new ArrayList();

		ltpath = generate_spline();
		Instantiate(core, tileMap.CenterPosition(new Vector3(end.x, end.y, end.z)), Quaternion.identity);
		//+ core.GetComponent<MeshFilter>().sharedMesh.bounds.extents.y/2
	}



	private LTSpline generate_spline(){
		Vector3[] current_path = new Vector3[4+(segments)];
		Vector3 start_control = new Vector3(0,path_y_offset,0);

		current_path[0] = start_control;

		Vector3 start = new Vector3(Random.Range(-1*map_size/2 + 1, map_size/2), path_y_offset,-1*map_size/2 + .5f);
		print(start);
		current_path[1] = start;

		Vector3 inter = start;
		Vector3 last_inter = new Vector3(map_size, path_y_offset, map_size); //impossible vector
		for(int i = 0; i < segments; i++){

			inter = generate_random_z(inter.z + 1, inter.z + (map_size/2 - inter.z)/( segments-i));


			current_path[i + 2]  = inter;
			last_inter = inter;
		}

		end =  new Vector3(map_size/2, path_y_offset,map_size/2);
		Vector3 end_control = new Vector3(end.x, path_y_offset,end.z-1);
		current_path[3+(segments)] = end_control;
		current_path[2+(segments)] = end;
		//print(end);

		return new LTSpline(current_path);


	}

	private Vector3 generate_random(){
		return new Vector3(Random.Range(-1*map_size/2, map_size/2), path_y_offset,Random.Range(-1*map_size/2, map_size/2));
	}


	private Vector3 generate_random_z(float z_constraint,float z_constraint2){
		return new Vector3(Random.Range(-map_size/2, map_size/2), path_y_offset,Random.Range(z_constraint, z_constraint2));
	}

	private Vector3 generate_random_x(float x_constraint1,float x_constraint2){
		return new Vector3(Random.Range(x_constraint1, x_constraint2), path_y_offset,Random.Range(-map_size/2, map_size/2));
	}



}
