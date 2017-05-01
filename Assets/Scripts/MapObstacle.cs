using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObstacle : MonoBehaviour {

	public enum Obstacle {

		Skip,
		Climb3M,
		Slide

	}


	public	Obstacle	obstacleType = Obstacle.Skip;


}
