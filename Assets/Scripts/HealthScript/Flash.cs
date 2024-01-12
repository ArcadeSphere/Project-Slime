using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material flashmat;
    [SerializeField] private float flashduration;
    private SpriteRenderer sr;
    private Material originalmat;
    private Coroutine flashRoutine;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalmat = sr.material;
    }


    public void flash_time()
    {
      
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(flashing_routine());
    }


    private IEnumerator flashing_routine()
    {
  
        sr.material = flashmat;
        yield return new WaitForSeconds(flashduration);
        sr.material = originalmat;
        flashRoutine = null;
    }
}
