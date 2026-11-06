using UnityEngine;
using UnityEngine.UI;
public class Speed200Controller : MonoBehaviour
{
    public float maxSpeed = 220f;
    public float acceleration = 35f;
    public float brakeForce = 60f;
    public float turnSpeed = 55f;
    public float currentSpeed = 0f;
    public bool turboOn = false;
    public Text speedText;
    private Rigidbody busRb;
    void Start(){ busRb = GetComponent<Rigidbody>(); }
    void Update()
    {
        float steer = Input.GetAxis("Horizontal");
        transform.Rotate(0, steer * turnSpeed * Time.deltaTime * (currentSpeed/maxSpeed), 0);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x,-7f,7f), transform.position.y, transform.position.z);
        if(Input.GetKeyDown(KeyCode.T)) turboOn = !turboOn;
        if(speedText!=null) speedText.text = "BR TRAVELS : " + (int)currentSpeed + " KM/H" + (turboOn ? " TURBO ON" : "");
    }
    void FixedUpdate()
    {
        float input = Input.GetAxis("Vertical");
        if(Input.GetKey(KeyCode.W)) input = 1f;
        if(Input.GetKey(KeyCode.S)) input = -1f;
        float acc = turboOn ? acceleration * 1.8f : acceleration;
        float max = turboOn ? 260f : maxSpeed;
        if(input>0) currentSpeed += acc * Time.deltaTime;
        else if(input<0) currentSpeed -= brakeForce * Time.deltaTime;
        else currentSpeed -= 8f * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed,0f,max);
        busRb.velocity = transform.forward * (currentSpeed/3.6f);
    }
}
public class DamageSystem : MonoBehaviour
{
    public int health = 100;
    public GameObject smokeEffect;
    public GameObject bumper;
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Traffic"))
        {
            health -= 20;
            if(health <= 60 && smokeEffect!=null) smokeEffect.SetActive(true);
            if(health <= 30 && bumper!=null) bumper.SetActive(false);
            if(health <= 0) { Debug.Log("BR BUS KHARAB HO GAYI"); Time.timeScale = 0; }
        }
    }
}
public class TrafficSystem : MonoBehaviour
{
    public GameObject[] trafficPrefabs;
    public Transform[] spawnPoints;
    private float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1.5f)
        {
            timer = 0f;
            int r = Random.Range(0, spawnPoints.Length);
            int v = Random.Range(0, trafficPrefabs.Length);
            Vector3 pos = spawnPoints[r].position + new Vector3(0,0,90f);
            Instantiate(trafficPrefabs[v], pos, Quaternion.identity);
        }
    }
}
public class SignalSystem : MonoBehaviour
{
    public int challan = 0;
    public Text challanText;
    public bool isRedLight = false;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Signal") && isRedLight)
        {
            challan += 500;
            if(challanText!=null) challanText.text = "CHALLAN: Rs " + challan;
            Debug.Log("BR BUS - Red Light Jump! Rs 500 Challan + Police aa rahi hai");
        }
    }
}
public class CustomTravelsName : MonoBehaviour
{
    public Text busNameText;
    public string travelsName = "B.R. TRAVELS";
    void Start()
    {
        if(busNameText!=null) busNameText.text = travelsName;
    }
}
public class VillageMapManager : MonoBehaviour
{
    public GameObject bhainsPrefab;
    public GameObject chaiStallPrefab;
    public Transform[] villagePoints;
    void Start()
    {
        foreach(Transform p in villagePoints)
        {
            int r = Random.Range(0,3);
            if(r==0 && bhainsPrefab!=null) Instantiate(bhainsPrefab, p.position, Quaternion.identity);
            if(r==1 && chaiStallPrefab!=null) Instantiate(chaiStallPrefab, p.position, Quaternion.identity);
        }
    }
}
