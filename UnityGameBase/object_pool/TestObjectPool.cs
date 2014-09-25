using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestObjectPool : MonoBehaviour
{
	Vector3 pos1 = new Vector3(-10,0,0);
	Vector3 pos2 = new Vector3(10,0,0);
	float frag,time;
	List<GameObject> instances = new List<GameObject>();
	// Use this for initialization
	void Start ()
	{
		GameObject pr = GameObject.CreatePrimitive(PrimitiveType.Cube);
		UGBObjectPool.AddObjectDefinition(pr,1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		frag = (Mathf.Sin(Time.time) + 1) / 2.0f; 
		this.transform.position = Vector3.Lerp(pos1, pos2, frag);
		
		time += Time.deltaTime;
		
		if(time > 0.15f)
		{
			GameObject go = UGBObjectPool.GetObjectInstance(1);
			go.transform.position = this.transform.position;
			instances.Add(go);
			time = 0;
			Stack<GameObject> stack = new Stack<GameObject>(instances);
			while(stack.Count > 0)
			{
				GameObject el = stack.Pop();
				if(el.transform.position.y < -1500)
				{
					el.transform.rigidbody.velocity = Vector3.zero;
					el.transform.rigidbody.angularVelocity = Vector3.zero;
					UGBObjectPool.ReturnObjectInstance(el,1);
				}
			}
		}
		
	}
}

