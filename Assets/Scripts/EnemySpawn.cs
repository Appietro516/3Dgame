﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawn : MonoBehaviour {
	float SpawnTimer =  1f;
	float time  = 0;

	float wave_start = 5f;
	float wavetime = 0;

	public CreatePath mg;
	public GameObject enemy;

	public GameObject waveNotifier;


	public int start_points;

	private bool waveOn = false;
	private int points;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (!waveOn){
			waveNotifier.SetActive(true);
			wavetime += Time.deltaTime * Time.timeScale;
			if ((wavetime) >= wave_start){
				waveOn = true;
				waveNotifier.SetActive(false);
				PlayerBehaviors.wave += 1;
				points = start_points * PlayerBehaviors.wave;
				wavetime = 0;
			}
		}

		else{
			GameObject current_enemy = null;
			time += Time.deltaTime * Time.timeScale;
			if ((time) >= SpawnTimer){
				if (points >= 0){
					current_enemy = Object.Instantiate(enemy);
					this.time = 0;
				}

			}
			if (current_enemy != null){

				//generate enemy
				Enemy enemy_stats = current_enemy.GetComponent<Enemy>();
				int temp_points = Random.Range(3, PlayerBehaviors.wave*9);
				enemy_stats.value = temp_points/3;

				points -= temp_points;


				enemy_stats.health = Random.Range(1,temp_points);
				temp_points -= enemy_stats.health;

				enemy_stats.speed = Random.Range(1,temp_points);
				temp_points -= enemy_stats.speed;

				enemy_stats.damage = temp_points;



				//move enemy_stats
				LeanTween.moveSpline(current_enemy, mg.ltpath, mg.ltpath.distance * Time.timeScale/enemy_stats.speed);

				//add enemy to global enemy list
				CreatePath.enemies.Add(current_enemy);
				current_enemy = null;

			}

			if(points <= 0 && CreatePath.enemies.Count == 0){
				waveOn = false;
			}
		}

	}


}
