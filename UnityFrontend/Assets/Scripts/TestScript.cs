using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private void OnValidate()
    {
        if (target == null)
            Debug.LogError("target cannot be null");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleVisibility()
    {
        target.SetActive(!target.activeSelf);
    }
}
