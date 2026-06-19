using UnityEngine;
public class Fauna : MonoBehaviour
{
    public FishType fishType;
    float speed;
    float distance;
    float offset;
    
    Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
        offset = Random.Range(0f, 100f);
        transform.localScale = Vector3.one; 
        switch (fishType)
        {
            case FishType.SmallFish:
            speed = Random.Range(1.0f, 1.5f);
            distance = Random.Range(10f, 20f);
            transform.localScale *= Random.Range(0.4f, 0.7f);
            break;
            case FishType.MediumFish:
            speed = Random.Range(0.7f, 0.9f);
            distance = Random.Range(10f, 20f);
            transform.localScale *= Random.Range(0.9f, 1.4f);
            break;
            case FishType.LargeFish:
            speed = Random.Range(0.3f, 0.5f);
            distance = Random.Range(10f, 20f);
            transform.localScale *= Random.Range(1.5f, 1.8f);
            break;
        }
    }
    void Update()
    {
        float x = Mathf.PingPong((Time.time + offset) * speed, distance * 2) - distance;
        transform.position = new Vector3
        (
            startPos.x + x,
            startPos.y,
            startPos.z
        );
    }
}
