using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Class used to play collision FX when nodes collide with walls
/// </summary>
public class CollisionManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private VisualEffect _collisionFX;

    private float _collisionTimer = 0f;
    private float _collisionTimeSeparation = 0.5f;

    private void Update()
    {
        _collisionTimer += Time.deltaTime;
    }
    
    /// <summary>
    /// Filters collision events to exclude player and prevents multiple collisions
    /// from happening in a short timeframe
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (_collisionTimer < _collisionTimeSeparation) return;
        if (collision.gameObject.GetComponent<CollisionManager>()) return;
        if (collision.gameObject.GetComponentInParent<Player>()) return;
        
        Debug.Log($"Collided with {collision.gameObject.name}");
        
        PlayCollisionFX(collision);
        _rigidbody.velocity *= -1;
        _collisionTimer = 0;
    }

    /// <summary>
    /// Uses information from passed in collision parameter to get the normal of the
    /// object collided with and uses that info to play collision FX that are rotated relative
    /// to that normal.
    /// </summary>
    /// <param name="collision"></param>
    private void PlayCollisionFX(Collision collision)
    {
        var vfx = Instantiate(_collisionFX, collision.contacts[0].point,
            Quaternion.Euler(collision.contacts[0].normal * 90));
        vfx.transform.rotation = Quaternion.Euler(vfx.transform.eulerAngles.x, vfx.transform.eulerAngles.y + 90,
            vfx.transform.eulerAngles.z);
        vfx.Play();
        Destroy(vfx.gameObject, 2f);
    }
}
