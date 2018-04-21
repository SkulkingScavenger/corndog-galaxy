using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour {

	public void Awake(){
		transform.Find("Panel").transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { JoinAsHost(); });
		transform.Find("Panel").transform.Find("Button1").GetComponent<Button>().onClick.AddListener(delegate { JoinAsClient(); });
		transform.Find("Panel").transform.Find("Button2").GetComponent<Button>().onClick.AddListener(delegate { CreateSpecies(); });
		transform.Find("Panel").transform.Find("Button3").GetComponent<Button>().onClick.AddListener(delegate { Quit(); });
	}

	public void JoinAsHost(){
		Transform mainCanvas = transform.parent;
		GameObject currentMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/GameCreateMenu"));
		currentMenu.transform.SetParent(mainCanvas,false);
		Destroy(transform.gameObject);
	}

	public void JoinAsClient(){
		Transform mainCanvas = transform.parent;
		GameObject currentMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/GameJoinMenu"));
		currentMenu.transform.SetParent(mainCanvas,false);
		Destroy(transform.gameObject);
	}

	public void CreateSpecies(){
		Transform mainCanvas = transform.parent;
		GameObject currentMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/SpeciesCreationMenu"));
		currentMenu.transform.SetParent(mainCanvas,false);
		Destroy(transform.gameObject);
	}


	public void Quit(){
		Application.Quit();
	}
}