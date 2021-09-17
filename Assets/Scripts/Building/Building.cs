using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour {
    [SerializeField] private GameObject buildOverlayPrefab = null;
    [SerializeField] private GameObject healthBarPrefab = null;
    [SerializeField] private int maxHealth = 0;

    private BuildOverlay buildOverlay = null;
    protected bool built = false;
    protected Health healthBar = null;
    private bool killed = false;

    public virtual bool Built {
        get => built;
        set {
            built = value;
            if (value) {
                if (buildOverlay) {
                    Destroy(buildOverlay.gameObject);
                }
                buildOverlay = null;

                InitializeHealthBar();
                ResumeBuildingBehaviour();
            }
            else {
                PauseBuildingBehaviour();
                GameObject buildOverlayObject = Instantiate(buildOverlayPrefab, Vector3.zero, Quaternion.identity, transform);
                buildOverlay = buildOverlayObject.GetComponent<BuildOverlay>();
                if (!buildOverlay) {
                    Debug.LogError("Building.Built.set: buildOverlayObject does not have a BuildOverlay component");
                    return;
                }

                if (healthBar) {
                    Destroy(healthBar.gameObject);
                    healthBar = null;
                }
                buildOverlay.Initialize(this);
            }
        }
    }

    protected virtual void Start() {
        if (!Built) {
            Built = false;
        }
    }

    // Update is called once per frame
    protected virtual void Update() {
        if (!Built) {
            BuildSiteState siteState = SiteState();
            buildOverlay.UpdateSprite(siteState == BuildSiteState.OccupiedIrreplacable);
        }
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

    protected abstract void ResumeBuildingBehaviour();
    protected abstract void PauseBuildingBehaviour();
    protected abstract BuildSiteState ColliderBuildState(Collider2D collider2D);

    public virtual void Damage(int damageAmount) {
        healthBar.SubtractHealth(damageAmount);
        if (healthBar.CurrentHealth == 0) {
            Kill();
        }
    }

    protected virtual void Kill() {
        if (!killed) {
            Destroy(gameObject);
            killed = true;
        }
    }

    public virtual BuildSiteState SiteState() {
        int layerMask = 0;
        for (int targetLayer = 0; targetLayer < 32; targetLayer++) {
            if (!Physics.GetIgnoreLayerCollision(gameObject.layer, targetLayer)) {
                layerMask |= 1 << targetLayer;
            }
        }
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(layerMask);
        Collider2D collider2D = GetComponent<Collider2D>();
        if (!collider2D) {
            Debug.LogError("Building.SiteState: gameObject does not have Collider2D component");
            return BuildSiteState.Invalid;
        }
        List<Collider2D> collisionResults = new List<Collider2D>();
        collider2D.OverlapCollider(contactFilter2D, collisionResults);
        BuildSiteState buildSiteState = BuildSiteState.Free;
        foreach (Collider2D collisionResult in collisionResults) {
            switch (ColliderBuildState(collisionResult)) {
            case BuildSiteState.Invalid:
                return BuildSiteState.Invalid;
            case BuildSiteState.Free:
                break;
            case BuildSiteState.OccupiedReplacable:
                if (buildSiteState == BuildSiteState.Free) {
                    buildSiteState = BuildSiteState.OccupiedReplacable;
                }
                break;
            case BuildSiteState.OccupiedIrreplacable:
                return BuildSiteState.OccupiedIrreplacable;
            default:
                break;
            }
        }
        return buildSiteState;
    }
}
