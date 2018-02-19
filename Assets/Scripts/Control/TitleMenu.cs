using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour {

	public void Awake(){
		transform.Find("Panel").transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { JoinAsHost(); });
	}

	public void JoinAsHost(){
		Transform mainCanvas = transform.parent;
		GameObject currentMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/GameCreateMenu"));
		currentMenu.transform.SetParent(mainCanvas,false);
		Destroy(transform.gameObject);
	}

	public void JoinAsClient(){}

	public void StartSandbox(){
		SceneManager.LoadScene("Level");
	}

	public void Quit(){}

	public void ManualOverride(){
		GUIUtility.keyboardControl = 0;
	}


}