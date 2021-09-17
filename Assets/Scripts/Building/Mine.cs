using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building {
    public float delay = 0.0f;
    public Collider2D aoeCollider = null;

    public override bool Built {
        get => built;
        set {
            if (!built) {
                base.Built = value;
            }
        }
    }

    private void triggerExplosion() {
        IEnumerator explodeAfterDelay() {
            yield return new WaitForSeconds(delay);
            explode();
            yield return null;
        }
        StartCoroutine(explodeAfterDelay());
    }

    private void explode() {
        if (aoeCollider == null) {
            Debug.LogError("Mine.explode: aoeCollider not set");
            return;
        }
        List<Collider2D> collisionResults = new List<Collider2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(LayerMask.GetMask("Enemy"));
        if (aoeCollider.OverlapCollider(contactFilter2D, collisionResults) > 0) {
            foreach (Collider2D collisionResult in collisionResults) {
                Enemy enemy = collisionResult.gameObject.GetComponent<Enemy>();
                if (!enemy) {
                    Debug.LogError("Mine.explode: collisionResult.gameObject does not contain Enemy component");
                    continue;
                }
                enemy.Damage(10);
            }
        }
        Destroy(gameObject);
    }

    public override void Damage(int damageAmount) {}

    protected override void ResumeBuildingBehaviour() {
        triggerExplosion();
    }

    protected override void PauseBuildingBehaviour() {}

    protected override BuildSiteState ColliderBuildState(Collider2D collider2D) {
        return collider2D.gameObject.name == "Base" ? BuildSiteState.OccupiedIrreplacable :
            BuildSiteState.Free;
    }
}
