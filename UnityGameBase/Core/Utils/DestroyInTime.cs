using UnityEngine;
using System.Collections;

public class DestroyInTime : MonoBehaviour
{
    public float time = 1;
    
	void Start()
	{
		StartCoroutine(DestroyAfterTime());
	}
	
	IEnumerator DestroyAfterTime()
    {
		yield return new WaitForSeconds(time);

		Destroy(this.gameObject);
    }
}
