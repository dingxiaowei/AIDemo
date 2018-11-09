using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FP_Shooting : MonoBehaviour {
	
    public float 		ShootingSlowness;
    public GameObject 	SpawnPositionCar;
    public GameObject 	Cardridge;
	public float 		carimpulse = 20f;
    private bool 		beingHandled = false;


    void Start () {
       


    }
    private  IEnumerator  Shooting()
    {

             beingHandled = true;
		GameObject cardridge;
		if (SpawnPositionCar) cardridge = (GameObject)Instantiate(Cardridge, SpawnPositionCar.transform.position + SpawnPositionCar.transform.right, SpawnPositionCar.transform.rotation);
        else  cardridge = (GameObject)Instantiate(Cardridge, SpawnPositionCar.transform.position + SpawnPositionCar.transform.forward, SpawnPositionCar.transform.rotation);        
        yield return new WaitForSeconds(ShootingSlowness);
		beingHandled = false;
 
    }
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Mouse0) && !beingHandled) {
			StartCoroutine (Shooting ());

		}

	}


}

