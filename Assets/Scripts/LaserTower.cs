using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower: AbstractTower {
	GameObject targetedEnemy = null;

	public override bool fire(){
		if(targetedEnemy != null && inRange(targetedEnemy)){
			loaded = false;
			Enemy enemy_stats = targetedEnemy.GetComponent<Enemy>();
			enemy_stats.health-= damage;
			since_fired = 0;
			return true;

		}

		this.targetedEnemy = getTarget();
		return false;
	}

	public override void miscUpdate(){
		Vector3[] points = new Vector3[2];
		if(targetedEnemy){
			points[0] = Vector3.Scale(this.gameObject.transform.position, new Vector3(1,2f,1));
			points[1] = targetedEnemy.transform.position;
			pointlight.enabled = true;
			line.SetPositions(points);
		}
		else{
			pointlight.enabled = false;
			line.SetPositions(points);
		}
	}

	private GameObject getTarget(){
		GameObject target = null;

		foreach(GameObject targetedEnemy in CreatePath.enemies){
			if (inRange(targetedEnemy)){
				if (target == null){
					target = targetedEnemy;
					continue;
				}

			 	Enemy enemy_stats = targetedEnemy.GetComponent<Enemy>();
				Enemy target_stats = target.GetComponent<Enemy>();

				if(!speed_targets){
					if (low_health_targets){
						if(enemy_stats.health < target_stats.health){
							target = targetedEnemy;
						}
					}
					else{
						if(enemy_stats.health > target_stats.health){
							target = targetedEnemy;
						}
					}
				}
				else{
					if(enemy_stats.speed > target_stats.speed){
						target = targetedEnemy;
					}
				}
			}

		}
		return target;
	}


	void OnMouseDown(){
		if (!PlayerBehaviors.paused){
			if (PlayerBehaviors.money >= upgradeCost){
				range += 1;
				PlayerBehaviors.money -= upgradeCost;
			}
		}
	}


}