using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParticlePooler : MonoBehaviour
{
    public static ParticlePooler instance;

    private void Awake()
    {
        instance = this;
    }

    private List<GameObject> FX_Hit_List;
    private List<GameObject> FX_Skill1_List;
    private GameObject Fx_LevelUp_Object; // 하나만 생성해둘 것이라서
    
    public enum ParticleType
    {
        HIT,
        SKILL1,
        LEVELUP
    }

    public GameObject FX_Hit;
    public int amountToPool1;

    public GameObject FX_Skill1;
    public int amountToPool2;

    public GameObject FX_LevelUp;


    private Transform parent;

    // 생성
    private void Start()
    {
        parent = this.transform;

        FX_Hit_List = new List<GameObject>();
        FX_Skill1_List = new List<GameObject>();

        for (int i = 0; i < amountToPool1; i++)
        {
            CreateObject(ParticleType.HIT);
        }

        for (int i = 0; i < amountToPool2; i++)
        {
            CreateObject(ParticleType.SKILL1);
        }

        Fx_LevelUp_Object = (GameObject)Instantiate(FX_LevelUp);
        Fx_LevelUp_Object.SetActive(false);
        Fx_LevelUp_Object.AddComponent<InvisibleObject>().delayTime = 5f;
        Fx_LevelUp_Object.transform.SetParent(parent);
    }

    private void CreateObject(ParticleType particleType)
    {
        switch (particleType)
        {
            case ParticleType.HIT:
                GameObject hit = (GameObject)Instantiate(FX_Hit);
                hit.SetActive(false);
                hit.AddComponent<InvisibleObject>().delayTime = 2f;
                hit.transform.SetParent(parent);
                FX_Hit_List.Add(hit);
                break;
            case ParticleType.SKILL1:
                GameObject skill1 = (GameObject)Instantiate(FX_Skill1);
                skill1.SetActive(false);
                skill1.AddComponent<InvisibleObject>().delayTime = 5f;
                skill1.transform.SetParent(parent);
                FX_Skill1_List.Add(skill1);
                break;
        }
    }

    public GameObject GetPooledObject(ParticleType particleType)
    {
        switch (particleType)
        {
            case ParticleType.HIT:
                for (int i = 0; i < FX_Hit_List.Count; i++)
                {
                    if (!FX_Hit_List[i].activeInHierarchy)
                    {
                        return FX_Hit_List[i];
                    }
                }
                // 모자라면 추가
                CreateObject(ParticleType.HIT);
                return FX_Hit_List[FX_Hit_List.Count - 1];

            case ParticleType.SKILL1:
                for (int i = 0; i < FX_Skill1_List.Count; i++)
                {
                    if (!FX_Skill1_List[i].activeInHierarchy)
                    {
                        return FX_Skill1_List[i];
                    }
                }
                CreateObject(ParticleType.SKILL1);
                return FX_Skill1_List[FX_Skill1_List.Count - 1];

            case ParticleType.LEVELUP:
                return Fx_LevelUp_Object;
        }

        return null;
    }
}
