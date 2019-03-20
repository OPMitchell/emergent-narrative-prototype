using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
	public float speed = 5.0f;
	public float targetOrtho;
	public float smoothZoomSpeed = 5.0f;
	public float zoomSpeed = 1.0f;
	public float minOrtho = 1.0f;
	public float maxOrtho = 20.0f;
	
	void Start()
	{
		Camera.main.orthographicSize = minOrtho;
		targetOrtho = Camera.main.orthographicSize;
	}

	void Update () 
	{
		Translate();
		Zoom();
	}

	void Translate()
	{
		float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
		float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
		if(!(transform.position.x + horizontal < 0) && !(transform.position.x + horizontal > 172))
			transform.Translate(horizontal, 0, 0);
		if(!(transform.position.y + vertical < 0) && !(transform.position.y + vertical > 172))
			transform.Translate(0, vertical, 0);
	}

	void Zoom()
	{
		if(Input.GetKeyDown(KeyCode.Q))
		{
			targetOrtho -= zoomSpeed;
            targetOrtho = Mathf.Clamp (targetOrtho, minOrtho, maxOrtho);
		}
		if(Input.GetKeyDown(KeyCode.E))
		{
			targetOrtho += zoomSpeed;
            targetOrtho = Mathf.Clamp (targetOrtho, minOrtho, maxOrtho);
		}
		Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, targetOrtho, smoothZoomSpeed * Time.deltaTime);
	}
}
