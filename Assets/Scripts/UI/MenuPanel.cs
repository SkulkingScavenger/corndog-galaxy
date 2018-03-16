using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuPanel : UIBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public bool inBox = false;


	void Awake(){
		GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		float mx = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).x;
		float my = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).y;
		Rect rect = GetComponent<RectTransform>().rect;
		// float dx = Mathf.Abs(mx - rect.x);
		// float dy = Mathf.Abs(my - rect.y);
		// Debug.Log(rect.x);
		// Debug.Log(rect.y);
		inBox = rect.Contains(new Vector2(mx,my));
	}

	public void OnPointerEnter(PointerEventData eventData){
		inBox = true;
	}

	public void OnPointerExit(PointerEventData eventData){
		inBox = true;
	}

	void Update(){
		if (Input.GetMouseButtonDown(0) && inBox){
			GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			float x = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).x;
			float y = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).y;
			Vector3 v = new Vector3(x,y,-2f);
			GameObject effect = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/ZakaasClickEffect"), v, Quaternion.identity);
		}
	}
}

