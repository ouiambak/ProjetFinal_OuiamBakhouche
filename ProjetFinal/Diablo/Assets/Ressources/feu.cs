using UnityEngine;

public class feu : MonoBehaviour
{
    [SerializeField] private float _speed = 10f; // Vitesse du projectile
    [SerializeField] private float _lifetime = 5f; // Durée de vie du projectile
    private int _damage;
    private Vector3 _targetPosition; // Position cible au moment du lancement

    private void Start()
    {
       // Destroy(gameObject, _lifetime); // Détruire le projectile après un certain temps
    }

    public void SetTarget(Vector3 targetPosition, int damage)
    {
        _targetPosition = targetPosition + new Vector3(0f,1.5f,0f); // Enregistrer la position cible
        _damage = damage; // Enregistrer les dégâts du projectile

        // Calcul de la direction initiale
        Vector3 direction = (_targetPosition - transform.position).normalized;
        GetComponent<Rigidbody>().velocity = direction * _speed; // Appliquer la vitesse au projectile
        transform.rotation = Quaternion.LookRotation(direction); // Aligner le projectile avec la direction
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Vérifier si le projectile touche le héros
        {
            PlayerHealthAndDefense playerHealth = other.GetComponent<PlayerHealthAndDefense>();
            if (playerHealth != null)
            {
                playerHealth.ReceiveDamage(_damage); // Infliger des dégâts au héros
            }

            //Destroy(gameObject); // Détruire le projectile après l'impact
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Wall")) // Si le projectile touche un mur
        {
            //Destroy(gameObject); // Détruire le projectile
        }
    }
}
