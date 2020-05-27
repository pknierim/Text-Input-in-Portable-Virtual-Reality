using UnityEngine;

[ExecuteInEditMode]
public class SetOrthographicSize : MonoBehaviour {

	void Update()
	{
		if (GetComponent<Camera>() && transform.parent)
		GetComponent<Camera>().orthographicSize = transform.parent.localScale.z * 5.0f;
	}
}
