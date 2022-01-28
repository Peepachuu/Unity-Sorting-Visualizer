
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private GameObject bar1;
    private GameObject bar2;

    void Start()
    {
        bar1 = Instantiate(prefab, Vector3.left, Quaternion.identity);
        bar2 = Instantiate(prefab, Vector3.right, Quaternion.identity);
        bar1.GetComponent<SpriteRenderer>().color = Color.red;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            bar2 = bar1;
        }
    }
    
}
