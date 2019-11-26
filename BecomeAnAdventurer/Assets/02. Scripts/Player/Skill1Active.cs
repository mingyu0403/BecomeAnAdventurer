using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1Active : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Effect 생성
            GameObject skillEffect = ParticlePooler.instance.GetPooledObject(ParticlePooler.ParticleType.SKILL1);
            if (skillEffect != null)
            {
                skillEffect.transform.position = this.transform.position;
                skillEffect.SetActive(true);
            }
            Destroy(this.gameObject);
        }

    }
}
