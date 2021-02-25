using UnityEngine;

public class BShotComponent : MonoBehaviour 
{
	[SerializeField]
	GameObject bShotBullet = null;

	void Start()
	{
		if (GetComponentInParent<Player>())
		{
			GetComponentInParent<Player>().Fire = bShotBullet;	
		}
	}
}
