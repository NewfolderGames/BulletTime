using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObstacle : MonoBehaviour {

	public enum Obstacle {

		Skip,
		Climb3M,
		Climb4M,
		Slide,
		Swing

	}

	public	Obstacle	obstacleType = Obstacle.Skip;
	public	bool		obstacleMidair = false;

	void Awake() {

		switch (obstacleType) {

			case Obstacle.Swing: case Obstacle.Skip :
				obstacleMidair = true;
				break;
			
			default :
				obstacleMidair = false;
				break;

		}

	}


}
