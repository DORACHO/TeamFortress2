using UnityEngine;

public class J_HillerMove : MonoBehaviour
{
    public LayerMask playerLayer;
    private int playerlayer;
    private bool isFire = false;
    public float speed = 15f;
    public Transform firePosition;
    public GameObject bulletFactory;
    private GameObject bullet;
    private float nextFireTime;
    public float fireRate = 2f;
    private bool isReloading = false;
    public int baseHPIncrease = 1;
    public float HPIncreaseRate = 10f;
    private BoxCollider boxCollider;
    public int maxHP = 185;
    private float hpIncreaseTimer = 0f;
    private float hpIncreaseInterval = 1f;
    private float hpIncreaseAmount = 0f;
    public int HillerHP = 150;
    public int HillerMP;
    private bool isMP;
    void Start()
    {
        nextFireTime = Time.deltaTime + fireRate;
        playerlayer = LayerMask.NameToLayer("Player");
    }

    void Update()
    {
        Debug.DrawRay(firePosition.position, firePosition.forward * 20f, Color.green);

        RaycastHit hit;

        if (Physics.Raycast(firePosition.position, firePosition.forward, out hit, 1 << playerlayer))
        {
            isFire = true;
        }
        else
        {
            isFire = false;
        }

        if (!isReloading && isFire && Time.deltaTime >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.deltaTime + fireRate;
        }

        // Calculate HP increase over time based on HPIncreaseRate
        if (isFire)
        {
            hpIncreaseTimer += Time.deltaTime;
            if (hpIncreaseTimer >= hpIncreaseInterval)
            {
                hpIncreaseAmount = baseHPIncrease * HPIncreaseRate;
                hpIncreaseTimer -= hpIncreaseInterval;
            }


        }
        else
        {
            hpIncreaseTimer = 0f;
        }

        // Increase HP
        //HPManager.instance.HP += Mathf.RoundToInt(hpIncreaseAmount);

        //if (HPManager.instance.HP > maxHP)
        //{
        //    HPManager.instance.HP = maxHP;
        //}
    }

    void Fire()
    {
        Ray ray = new Ray(firePosition.position, firePosition.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerLayer))
        {
            GameObject bullet = Instantiate(bulletFactory, firePosition.position, firePosition.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * speed;

            // Increase HP
            int HPIncrease = baseHPIncrease;
            HPManager.instance.HP += HPIncrease;

            if (HPManager.instance.HP > maxHP)
            {
                HPManager.instance.HP = maxHP;
            }
            if (HPManager.instance.HP < maxHP)
            {
                HPManager.instance.HP++;
            }
        }
    }
    void MP()
    {
        //MP를 쓸경우 본인의 HP가 줄어든다
        if (isMP)
        {
            HPManager.instance.HP--;
        }
    }
}
