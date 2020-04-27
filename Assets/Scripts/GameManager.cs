using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
    public List<Missile> missilePool;
    public Material[] materials;

    [HideInInspector] public List<Missile> activeMissiles;

    [SerializeField] private Transform missileTarget;
    private Vector3 missileTargetPos;

    [SerializeField] private float activeRadius;
    private Missile _cache;

    [SerializeField] private float missileSpeed = 4f;
    [SerializeField] private bool isGameRunning = true;

    [SerializeField] private float angle;
    private int _index;

    // Start is called before the first frame update
    void Start() {
        missileTargetPos = missileTarget.position;
        _index = 0;

        StartCoroutine(spawner());
    }

    // Update is called once per frame
    void Update() {
        missileTargetPos = missileTarget.position;
        float _deltaTime = Time.deltaTime;

        for (int i = 0; i < activeMissiles.Count; i++) {
            Vector3 mPos = missileTargetPos - activeMissiles[i].ThisTransform.position;
            activeMissiles[i].ThisTransform.position += mPos.normalized * (activeMissiles[i].speed * _deltaTime);
            activeMissiles[i].ThisTransform.LookAt(missileTargetPos);
        }
    }

    void SpawnMissile() {
        int rtype = Random.Range(0, materials.Length);
        _cache = missilePool[0];
        _cache.speed = Random.Range(3, 8);

        Renderer rend = _cache.GetComponent<Renderer>();
        rend.material = materials[rtype];

        activeMissiles.Add(_cache);
        missilePool.RemoveAt(0);

        float _angle = Random.Range(-angle, angle);
        Vector3 lDirection = new Vector3(Mathf.Sin(Mathf.Deg2Rad * _angle), 0f, Mathf.Cos(Mathf.Deg2Rad * _angle)) * activeRadius;
        //print(lDirection);
        //float randRadius = Random.Range(activeRadius, activeRadius * 2);

        //int _side = Random.Range(59, 999) % 15;
        //if (_side > 7)
        //    _side = -1;
        //else
        //    _side = 1;

        //float _spawnX = missileTargetPos.x + (randRadius * _side);

        //_side = Random.Range(29, 789) % 15;
        //if (_side > 7)
        //    _side = -1;
        //else
        //    _side = 1;

        //float _spawnZ = missileTargetPos.z + (randRadius * _side);

        Vector3 spawnPos = missileTargetPos + lDirection;
        spawnPos.y = Random.Range(spawnPos.y - 2f, spawnPos.y + 0.5f);

        _cache.ThisTransform.position = spawnPos;
        _cache.ThisTransform.LookAt(missileTargetPos);
        _index++;
    }

    IEnumerator spawner() {
        while (isGameRunning) {
#if UNITY_EDITOR
            print("spawned");
#endif
            //OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);

            if (missilePool.Count > 0)
                SpawnMissile();
            else {
                for (int i = 0; i < activeMissiles.Count; i++) {
                    missilePool.Add(activeMissiles[i]);
                }

                activeMissiles.Clear();
                SpawnMissile();
            }
            yield return new WaitForSeconds(1.5f);
        }
    }
}
