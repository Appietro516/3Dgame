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

	delegate GameObject CreateEnemyPointer();
    CreateEnemyPointer CreateEnemy;



	public int start_points;

	private bool waveOn = false;
	private int points;

	// Use this for initialization
	void Start () {
		CreateEnemy = CreateBasic;
	}

	GameObject CreateBasic(){
		GameObject current_enemy = Object.Instantiate(enemy);
		Enemy enemy_stats = current_enemy.GetComponent<Enemy>();
		int temp_points = Random.Range(1 + (PlayerBehaviors.wave), 6 + (PlayerBehaviors.wave*2));
		enemy_stats.value = 4;

		points -= temp_points;


		enemy_stats.health = Random.Range(2,temp_points);
		temp_points -= enemy_stats.health;

		enemy_stats.speed = Random.Range(2,temp_points);
		temp_points -= enemy_stats.speed;

		enemy_stats.damage = Mathf.Max(1,temp_points);

		int max_stat = Mathf.Max(enemy_stats.health, enemy_stats.speed, enemy_stats.damage);
		if (max_stat == enemy_stats.health){
			current_enemy.GetComponent<Renderer>().material.color = Color.green;
		}
		else if (max_stat == enemy_stats.speed){
			current_enemy.GetComponent<Renderer>().material.color = Color.blue;
		}

		else if (max_stat == enemy_stats.damage){
			current_enemy.GetComponent<Renderer>().material.color = Color.red;
		}

		return current_enemy;

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
				points = start_points * PlayerBehaviors.wave * 3;
				wavetime = 0;
			}
		}

		else{
			GameObject current_enemy = null;
			time += Time.deltaTime * Time.timeScale;
			if ((time) >= SpawnTimer){
				if (points >= 0){
					this.time = 0;
					current_enemy = CreateEnemy();
					SpawnTimer = Random.Range(0.2f, 1.0f);
				}

			}
			if (current_enemy != null){
				//move enemy_stats
				LeanTween.moveSpline(current_enemy, mg.ltpath, mg.ltpath.distance * Time.timeScale/current_enemy.GetComponent<Enemy>().speed);

				//add enemy to global enemy list
				if (current_enemy != null){
					CreatePath.enemies.Add(current_enemy);
					current_enemy = null;
				}

			}

			if(points <= 0 && CreatePath.enemies.Count == 0){
				waveOn = false;
				foreach(GameObject tower in CreatePath.towers){
					AbstractTower tower_stats = tower.GetComponent<AbstractTower>();
					if (tower_stats is BankTower){
						PlayerBehaviors.money += tower_stats.damage;
					}
				}
			}
		}

	}


}
