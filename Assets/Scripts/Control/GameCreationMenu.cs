using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameCreationMenu : MonoBehaviour {

	public void Awake(){
		transform.Find("NavigationPanel").transform.Find("Button0").GetComponent<Button>().onClick.AddListener(delegate { OpenServerSettingsPanel(); });
		transform.Find("NavigationPanel").transform.Find("Button1").GetComponent<Button>().onClick.AddListener(delegate { OpenGalaxySettingsPanel(); });
		transform.Find("NavigationPanel").transform.Find("Button2").GetComponent<Button>().onClick.AddListener(delegate { StartServer(); });
		transform.Find("NavigationPanel").transform.Find("Button3").GetComponent<Button>().onClick.AddListener(delegate { ReturnToTitle(); });

		transform.Find("ServerSettingsPanel").transform.Find("Button0").GetComponent<Button>().onClick.AddListener(delegate { LoadServerSettings(); });
		transform.Find("ServerSettingsPanel").transform.Find("Button1").GetComponent<Button>().onClick.AddListener(delegate { SaveServerSettings(); });

		transform.Find("GalaxySettingsPanel").transform.Find("Button0").GetComponent<Button>().onClick.AddListener(delegate { LoadGalaxySettings(); });
		transform.Find("GalaxySettingsPanel").transform.Find("Button1").GetComponent<Button>().onClick.AddListener(delegate { SaveGalaxySettings(); });
	}

	public void OpenServerSettingsPanel(){
		transform.Find("NavigationPanel").transform.Find("Button0").GetComponent<Button>().interactable = false;
		transform.Find("NavigationPanel").transform.Find("Button1").GetComponent<Button>().interactable = true;

		transform.Find("ServerSettingsPanel").transform.localPosition = new Vector3(-192f, -64f, 0);
		transform.Find("GalaxySettingsPanel").transform.localPosition = new Vector3(832f, -64f, 0);

	}

	public void OpenGalaxySettingsPanel(){
		transform.Find("NavigationPanel").transform.Find("Button0").GetComponent<Button>().interactable = true;
		transform.Find("NavigationPanel").transform.Find("Button1").GetComponent<Button>().interactable = false;

		transform.Find("ServerSettingsPanel").transform.localPosition = new Vector3(832f, -64f, 0);
		transform.Find("GalaxySettingsPanel").transform.localPosition = new Vector3(192, -64f, 0);

	}

	public void StartServer(){
		SceneManager.LoadScene("Level");
	}

	public void ReturnToTitle(){
		Transform mainCanvas = transform.parent;
		GameObject currentMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/TitleMenu"));
		currentMenu.transform.SetParent(mainCanvas,false);
		Destroy(this);
	}

	public void LoadServerSettings(){
		//TODO, read from file
	}

	public void SaveServerSettings(){
		//TODO, write to file
	}

	public void LoadGalaxySettings(){
		//TODO, read from file
	}

	public void SaveGalaxySettings(){
		//TODO, write to file
	}

	public void ManualOverride(){
		transform.Find("ServerSettingsPanel").transform.Find("PossessionRulesDropdown").GetComponent<Dropdown>().Select();
		Debug.Log("araf");
		//StartCoroutine( Wait());
		
	}

	public IEnumerator Wait(){
		yield return new WaitForSeconds(1f);
		Debug.Log("snaraf");
		transform.Find("ServerSettingsPanel").transform.Find("AfkBehaviourDropdown").GetComponent<Dropdown>().enabled = true;
	}


}