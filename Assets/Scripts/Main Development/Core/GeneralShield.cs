using UnityEngine;

namespace Defender.Core {
    /// <summary>
    /// Manages the General Left/Right Shield of the player
    /// Handles Trigger Enter Event, for the missiles. Updates Health Based on it
    /// </summary>
    public class GeneralShield : MonoBehaviour {
        private const string RIGHT_MISSILE_TAG = "RM";
        private const string LEFT_MISSILE_TAG = "LM";

        [SerializeField] private bool isLeftShield;

        private void OnTriggerEnter(Collider other) {
            //IF Left Shield
            if (isLeftShield) {
                //If Right Missile Hit
                if (other.CompareTag(RIGHT_MISSILE_TAG)) {
                    //GameManager.instance.TakeDamage();
                    //WaveManager.PutBackToPool();
                    Destroy(other.gameObject);
                }
                //If Left Missile Hit
                else if (other.CompareTag(LEFT_MISSILE_TAG)) {
                    print(other.name);
                    Destroy(other.gameObject);
                    //WaveManager.PutBackToPool();
                }
            } else //If Right Shield
              {
                //If Left Missile Hit
                if (other.CompareTag(LEFT_MISSILE_TAG)) {
                    //GameManager.instance.TakeDamage();
                    //WaveManager.PutBackToPool();
                    Destroy(other.gameObject);
                }
                //If Right Missile Hit
                else if (other.CompareTag(RIGHT_MISSILE_TAG)) {
                    //WaveManager.PutBackToPool();
                    Destroy(other.gameObject);
                }
            }
        }
    }
}