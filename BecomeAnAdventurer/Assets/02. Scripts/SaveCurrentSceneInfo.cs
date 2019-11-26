using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveCurrentSceneInfo : MonoBehaviour
{
    public static SaveCurrentSceneInfo instace;
    private GameObject Player;
    public bool canPlayerUsePortal = true;

    private string currentSceneName;
    public int portalNumber = 1;


    private void Awake()
    {
        instace = this;
    }

    private void OnEnable()
    {
        Debug.Log("플레이어 생성함");

        SceneManager.sceneLoaded += OnSceneLoaded;

        Player = Resources.Load("DontDestryOnSceneLoad") as GameObject;
        Instantiate(Player);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;

        // Debug.Log("씬 교체됨, 현재 씬: " + scene.name);
        
        CurrentSceneManager currentSceneManager = GameObject.FindWithTag("SCENE_MANAGER").GetComponent<CurrentSceneManager>();
        
        // 플레이어 위치를 포탈 위치로 옮김
        GameManager.instance.playerManager.transform.position = currentSceneManager.PortalList[portalNumber].position;

        // NPC들에게 퀘스트 진행 세팅
        GameManager.instance.playerManager.SetQuestToAllNPC();

        // 씬 전환 효과
        GameUiManager.instance.ShowFadeIn();
    }

    public void SceneChange(string SceneName, int portalNumber)
    {
        if (!canPlayerUsePortal)
        {
            return;
        }
        canPlayerUsePortal = false;

        // Debug.Log("포탈 탐, 현재 씬 : " + currentSceneName + ", 이동할 씬 : " + SceneName + ", 포탈 넘버 : " + portalNumber );
        if (this.currentSceneName != SceneName)
        {
            SceneManager.LoadScene(SceneName);
        }
        SaveCurrentSceneInfo.instace.portalNumber = portalNumber;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
