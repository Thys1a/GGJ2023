using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TestTwistEffect()
    {
        StartCoroutine(TestEffect());
    }
    public IEnumerator TestEffect()
    {
        var twistEffect = transform.parent.Find("TwistEffectLayer").GetComponent<TwistEffect>();
        twistEffect.speed = Random.Range(0, 10) > 4 ? 300 : -300;
        twistEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);

    }
}
