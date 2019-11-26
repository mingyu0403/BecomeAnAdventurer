using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageUIPooler : MonoBehaviour
{
    // 어택 정보 구조체
    public struct AttackInfo
    {
        public int damage; // 데미지
        public DamageType damageType; // 데미지 유형
    }

    public enum DamageType
    {
        BASIC,
        CRITICAL,
        MISS
    }

    public List<GameObject> basicDamage_UiList;
    public List<GameObject> criticalDamage_UiList;

    public GameObject basicDamageUI;
    public GameObject criticalDamageUI;

    public int amountToPool;
    public Transform parent;

    // 생성
    private void Start()
    {
        basicDamage_UiList = new List<GameObject>();
        criticalDamage_UiList = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            CreateObject(DamageType.BASIC);
            CreateObject(DamageType.CRITICAL);
        }
    }

    private void CreateObject(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.BASIC:
                GameObject basic = (GameObject)Instantiate(basicDamageUI);
                basic.SetActive(false);
                basic.AddComponent<InvisibleObject>().delayTime = 2f;
                basic.transform.SetParent(parent);
                basicDamage_UiList.Add(basic);
                break;
            case DamageType.CRITICAL:
                GameObject cri = (GameObject)Instantiate(criticalDamageUI);
                cri.SetActive(false);
                cri.AddComponent<InvisibleObject>().delayTime = 2f;
                cri.transform.SetParent(parent);
                criticalDamage_UiList.Add(cri);
                break;
            case DamageType.MISS:
                break;
        }
    }

    public GameObject GetPooledObject(AttackInfo attackInfo)
    {
        switch (attackInfo.damageType)
        {
            case DamageType.BASIC:
                for (int i = 0; i < basicDamage_UiList.Count; i++)
                {
                    if (!basicDamage_UiList[i].activeInHierarchy)
                    {
                        basicDamage_UiList[i].GetComponent<TextMeshProUGUI>().text = attackInfo.damage.ToString();
                        return basicDamage_UiList[i];
                    }
                }
                // 부족하면 만들어주자
                CreateObject(DamageType.BASIC);
                basicDamage_UiList[basicDamage_UiList.Count - 1].GetComponent<TextMeshProUGUI>().text = attackInfo.damage.ToString();
                return basicDamage_UiList[basicDamage_UiList.Count - 1];

            case DamageType.CRITICAL:
                for (int i = 0; i < criticalDamage_UiList.Count; i++)
                {
                    if (!criticalDamage_UiList[i].activeInHierarchy)
                    {
                        criticalDamage_UiList[i].GetComponent<TextMeshProUGUI>().text = attackInfo.damage.ToString();
                        return criticalDamage_UiList[i];
                    }
                }
                CreateObject(DamageType.CRITICAL);
                criticalDamage_UiList[criticalDamage_UiList.Count - 1].GetComponent<TextMeshProUGUI>().text = attackInfo.damage.ToString();
                return criticalDamage_UiList[criticalDamage_UiList.Count - 1];
                
            case DamageType.MISS:
                break;
        }
        return null;
    }
}
