using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private int maxHealth = 0;
    [SerializeField] private int goldLoot = 0;
    [SerializeField] private GameObject healthBarPrefab = null;

    private Vector3 moveVector = Vector3.zero;
    private List<Building> hittingBuildings = new List<Building>();
    private Health healthBar = null;

    // Start is called before the first frame update
    void Start() {
        InitializeHealthBar();
        StartCoroutine(DamageTouchingBuildings());
    }

    // Update is called once per frame
    void Update() {
        setSpriteRotation();
    }

    void FixedUpdate() {
        setMoveVector();
        move();
    }

    protected void InitializeHealthBar() {
        if (maxHealth == 0) {
            return;
        }
        GameObject healthBarObj = Instantiate(healthBarPrefab, Vector3.zero, Quaternion.identity,
            transform);
        healthBarObj.transform.localPosition = Vector3.zero;
        healthBar = healthBarObj.GetComponent<Health>();
        if (!healthBar) {
            Debug.LogError("Building.InitializeHealthBar: healthBarObj does not have a Health " +
                "component");
            return;
        }
        healthBar.Initialize(gameObject, maxHealth);
    }

    void setMoveVector() {
        Base baseInstance = Base.Instance();
        if (baseInstance == null) {
            return;
        }
        Vector3 moveDirection = (baseInstance.transform.position - transform.position).normalized;
        moveVector = moveDirection * speed * Time.fixedDeltaTime;
    }

    void move() {
        Collider2D collider2D = GetComponent<Collider2D>();
        if (collider2D == null) {
            Debug.LogError("Enemy.move: Collider2D component does not exist");
            return;
        }
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(LayerMask.GetMask("Building"));
        List<RaycastHit2D> raycastHit2DList = new List<RaycastHit2D>();
        if (collider2D.Cast(moveVector.normalized, contactFilter2D, raycastHit2DList, moveVector.magnitude) == 0) {
            transform.position += moveVector;
        }
        else {
            hittingBuildings.Clear();
            foreach (RaycastHit2D hit in raycastHit2DList) {
                Building building = hit.transform.gameObject.GetComponent<Building>();
                if (building == null) {
                    Debug.LogError("Enemy.move: builidng does not contain Building component");
                    continue;
                }
                hittingBuildings.Add(building);
            }
        }
    }

    void setSpriteRotation() {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, moveVector);
        healthBar.transform.rotation = Quaternion.identity;
    }

    IEnumerator DamageTouchingBuildings() {
        while (true) {
            yield return new WaitForSeconds(1.0f);
            foreach (Building building in hittingBuildings) {
                building.Damage(1);
            }
        }
    }

    public void Damage(int baseDamage) {
        healthBar.SubtractHealth(baseDamage);
        if (healthBar.CurrentHealth == 0) {
            Kill();
        }
    }

    public void Kill() {
        LevelGameManager.Instance.AddGold(goldLoot);
        LevelGameManager.Instance.IncrementEnemyKillCount();
        Destroy(gameObject);
    }
}
