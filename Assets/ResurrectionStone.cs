using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionStone : MonoBehaviour
{
    [SerializeField]
    Collider ringCollider;
    [SerializeField]
    GameObject floatingText;
    bool animationRoutine;

    private void Start()
    {
        animationRoutine = false;
        EquipScript.horcruxHit += HitCheck;
    }

    private void Update()
    {
        if(animationRoutine)
            transform.Rotate(Vector3.up * 60 * Time.deltaTime);
    }

    void HitCheck(Collider other)
    {
        if (other.Equals(ringCollider))
        {
            animationRoutine = true;
            this.transform.SetParent(null);
            StartCoroutine(Enlarge(transform, 1.8f, 5));
        }
    }

    public IEnumerator Enlarge(Transform target, float multiplier, float interval)
    {
        float totalDuration = interval;
        Vector3 startingScale = target.localScale;

        while (interval > 0)
        {
            target.localScale = Vector3.Lerp(startingScale * multiplier, startingScale, interval / totalDuration);
            interval -= Time.deltaTime;
            yield return null;
        }

        GameObject thisText = Instantiate(floatingText, new Vector3(transform.position.x + Camera.main.transform.forward.x, 0,
                                                        transform.position.z + Camera.main.transform.forward.z), Quaternion.identity);
        thisText.transform.LookAt(Camera.main.transform.position);
        thisText.transform.Rotate(Vector3.up, 150);
    }
    
}
