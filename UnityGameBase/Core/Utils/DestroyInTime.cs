using UnityEngine;
using System.Collections;

public class DestroyInTime : MonoBehaviour
{
    public float time = 1;
    void Start()
    {
        Invoke("DestroyMe", time);
    }
	
    void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
