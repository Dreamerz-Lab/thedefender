using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MissileManager : MonoBehaviour {
    //Player Properties
    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform playerCollider;
    [SerializeField] private Vector3 playerColliderOffset;
    [SerializeField] private Vector3 missileTargetOffset;
    private Vector3 missileTargetPos;

    //Wave Properties
    [FoldoutGroup("Wave Properties")]
    [Indent(1)] [SerializeField] private WaveScriptableObject[] waves;
    [FoldoutGroup("Wave Properties")]
    [Indent(1)] [SerializeField] private WaveScriptableObject currentWave;
    [FoldoutGroup("Wave Properties")]
    [Indent(1)] [SerializeField] private int currentindex;

    private float lastSpawnTime;
    [SerializeField] private int missiledSpawnCount;
    private float waveTimer;
    private float waveStartTime;

    //Missile Properties
    public struct MissileProperties {
        public Vector3 Position;
        public Vector3 Direction;
        public float Speed;
    }

    [FoldoutGroup("Missile Properties")]
    [Indent(1)] [SerializeField] private float missileSpawnRadius = 1f;
    [FoldoutGroup("Missile Properties")]
    [Indent(1)] [SerializeField] private Transform[] Incomings;
    //[FoldoutGroup("Missile Properties/Missile Prefabs")]
    //[Indent(1)] [SerializeField] private Transform missileR;
    //[FoldoutGroup("Missile Properties/Missile Prefabs")]
    //[Indent(1)] [SerializeField] private Transform missileLR;

    [FoldoutGroup("Missile Properties")]
    [SerializeField] private int mListCapacity = 10;
    [SerializeField] private int bigMissileFactor = 20;

    [SerializeField] private GameObject[] Portals;

    private List<Missile> missiles;
    private Missile _cacheMissile;

    public bool isGameStart;
    public AudioSource _MainAudio;
    public TMPro.TMP_Text UI_Timer;
    public TMPro.TMP_Text UI_Score;
    public int WaveTargetTime = 120;
    public int Score = 0;

    public GameObject WrongHitMarker;
    public Transform m5;
    public Transform p5;

    public UnityEngine.Events.UnityEvent GameEndEvent;

    private WaitForSeconds _wait = new WaitForSeconds(0.1f);

    public void Correct() {
        ScoreUpdate(5);
        WrongHitMarker.SetActive(false);
        StopCoroutine("CorrectHitIndicator");
        StartCoroutine("CorrectHitIndicator");
    }

    public void Wrong() {
        WrongHitMarker.SetActive(true);
        StopCoroutine("WrongHitIndicator");
        StartCoroutine("WrongHitIndicator");
    }

    IEnumerator CorrectHitIndicator() {
        p5.gameObject.SetActive(true);

        Vector3 _p5Pos = p5.position;
        p5.position = new Vector3(_p5Pos.x, -3, _p5Pos.z);

        while (p5.position.y < 3f) {
            p5.position += new Vector3(0, 0.1f, 0);
            yield return null;
        }

        p5.gameObject.SetActive(false);
    }

    IEnumerator WrongHitIndicator() {
        m5.gameObject.SetActive(true);

        Vector3 _m5Pos = m5.position;
        m5.position = new Vector3(_m5Pos.x, -3, _m5Pos.z);

        while (m5.position.y < 3f) {
            m5.position += new Vector3(0, 0.1f, 0);
            yield return null;
        }

        m5.gameObject.SetActive(false);
    }

    public void MissileMinYIncrease(TMPro.TMP_Text txt) {
        currentWave.MinY++;
        txt.text = "Min Y " + currentWave.MinY.ToString();
    }

    public void MissileMinYDecrease(TMPro.TMP_Text txt) {
        currentWave.MinY--;
        txt.text = "Min Y " + currentWave.MinY.ToString();
    }

    public void MissileMaxYIncrease(TMPro.TMP_Text txt) {
        currentWave.MaxY++;
        txt.text = "Max Y " + currentWave.MaxY.ToString();
    }

    public void MissileMaxYDecrease(TMPro.TMP_Text txt) {
        currentWave.MaxY--;
        txt.text = "Max Y " + currentWave.MaxY.ToString();
    }

    public void SpeedIncrease(TMPro.TMP_Text txt) {
        currentWave.StaticSpeed++;
        txt.text = "Speed " + currentWave.StaticSpeed.ToString();
    }

    public void SpeedDecrease(TMPro.TMP_Text txt) {
        currentWave.StaticSpeed--;
        txt.text = "Speed " + currentWave.StaticSpeed.ToString();
    }

    public void SpawnTimerIncrease(TMPro.TMP_Text txt) {
        currentWave.MissileSpawnFrequency += 0.1f;
        txt.text = "Spawn " + currentWave.MissileSpawnFrequency.ToString();
    }

    public void SpawnTimerDecrease(TMPro.TMP_Text txt) {
        currentWave.MissileSpawnFrequency -= 0.1f;
        txt.text = "Spawn " + currentWave.MissileSpawnFrequency.ToString();
    }

    public void AngleIncrease(TMPro.TMP_Text txt) {
        currentWave.missileOrigin[0].SpawnAngle += 1f;
        txt.text = "Angle " + currentWave.missileOrigin[0].SpawnAngle.ToString();
    }

    public void AngleDecrease(TMPro.TMP_Text txt) {
        currentWave.missileOrigin[0].SpawnAngle -= 1f;
        txt.text = "Angle " + currentWave.missileOrigin[0].SpawnAngle.ToString();
    }

    private void Start() {
        _wait = new WaitForSeconds(0.1f);
        missiles = new List<Missile>(mListCapacity);

        _MainAudio.volume = 1;
        _MainAudio.pitch = 1;
    }

    public void GameStart() {
        isGameStart = true;

        missiles.Clear();

        missiledSpawnCount = 0;
        currentindex = 0;
        currentWave = waves[0];
        missileTargetPos = playerPos.position;

        _MainAudio.volume = 1;
        _MainAudio.pitch = 1;

        waveStartTime = Time.time;

        Score = 0;
        UI_Score.text = Score.ToString();

        Invoke("WaveUpdate", 40);
    }

    public void GameStop() {
        isGameStart = false;
        missiles.Clear();

        StartCoroutine("musicFadeOut");
    }

    public void WaveUpdate() {
        currentWave = waves[1];
    }

    IEnumerator musicFadeOut() {
        while (_MainAudio.volume > 0) {
            _MainAudio.volume -= 0.03f;
            _MainAudio.pitch -= 0.03f;
            yield return _wait;
        }
    }

    public void ScoreUpdate(int value) {
        Score += value;
        if (Score < 0)
            Score = 0;

        UI_Score.text = Score.ToString();
    }

    private void SpawnMissile() {
        float _spawnAngle = currentWave.missileOrigin[0].SpawnAngle;
        float _angle = (90f * (int)currentWave.missileOrigin[Random.Range(0, currentWave.missileOrigin.Length)].OriginDirection) + Random.Range(-_spawnAngle, _spawnAngle);

        Vector3 lDirection = new Vector3(Mathf.Sin(Mathf.Deg2Rad * _angle), 0f, Mathf.Cos(Mathf.Deg2Rad * _angle)) * missileSpawnRadius;
        Vector3 spawnPos = missileTargetPos + lDirection;
        lDirection.Normalize();

        spawnPos.y = missileTargetPos.y + Random.Range(currentWave.MinY, currentWave.MaxY);

        int _index = (int)currentWave.incomings[Random.Range(2424, 9340) % currentWave.incomings.Length];
        Transform _spawn = Incomings[_index];
        GameObject _portal = GameObject.Instantiate(Portals[_index], spawnPos, Quaternion.LookRotation(-1 * lDirection, Vector3.up));

        _cacheMissile = Instantiate(_spawn, spawnPos, Quaternion.LookRotation(-1 * lDirection, Vector3.up)).GetComponent<Missile>();

        Destroy(_portal, 1.5f);
        Destroy(_cacheMissile.gameObject, 8f);
        /*
        if ((Random.Range(3453, 39531) % 30) > bigMissileFactor) {
            _cacheMissile = Instantiate(missileLR, spawnPos, Quaternion.LookRotation(-1 * lDirection, Vector3.up)).GetComponent<Missile>();
        } else {
            bool _probability = ((Random.Range(231, 9843) % 20) > (20 * currentWave.MissileHandMatchProbability));
            float pForwardAngle = Vector3.Angle(Vector3.forward, playerPos.forward);

            if (_angle >= pForwardAngle && _angle < (pForwardAngle + 180)) {
                if (_probability)
                    _cacheMissile = Instantiate(missileR, spawnPos, Quaternion.LookRotation(-1 * lDirection, Vector3.up)).GetComponent<Missile>();
                else
                    _cacheMissile = Instantiate(missileR, spawnPos, Quaternion.LookRotation(-1 * lDirection, Vector3.up)).GetComponent<Missile>();
            } else {
                if (_probability)
                    _cacheMissile = Instantiate(missileR, spawnPos, Quaternion.LookRotation(-1 * lDirection, Vector3.up)).GetComponent<Missile>();
                else
                    _cacheMissile = Instantiate(missileR, spawnPos, Quaternion.LookRotation(-1 * lDirection, Vector3.up)).GetComponent<Missile>();
            }
        }*/

        //_cacheMissile.speed = Random.Range(currentWave.MinSpeed, currentWave.MaxSpeed);
        _cacheMissile.speed = currentWave.StaticSpeed;
        _cacheMissile.thisPosition = spawnPos;
        _cacheMissile.targetDirection = ((missileTargetPos + new Vector3(Random.Range(-missileTargetOffset.x, missileTargetOffset.x), 0, 0)) - spawnPos).normalized;
        _cacheMissile.MissileStart();
        missiles.Add(_cacheMissile);

        missiledSpawnCount++;
    }

    private void Update() {
        if (!isGameStart)
            return;

        float _currentTime = Time.time;

        waveTimer = WaveTargetTime - (_currentTime - waveStartTime);

        missileTargetPos = playerPos.position;
        playerCollider.position = missileTargetPos + playerColliderOffset;
        missileTargetPos.y += missileTargetOffset.y;

        float _deltaTime = Time.deltaTime;

        if ((_currentTime - lastSpawnTime) >= currentWave.MissileSpawnFrequency) {
            lastSpawnTime = _currentTime;
            SpawnMissile();
        }

        for (int i = 0; i < missiles.Count; i++) {
            if (missiles[i] == null)
                missiles.RemoveAt(i);

            missiles[i].MissileUpdate(missileTargetPos, _deltaTime);
        }

        if (waveTimer <= 0) {
            GameEndEvent.Invoke();
            return;
        }

        UI_Timer.text = System.Math.Round(waveTimer, 2).ToString();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, missileSpawnRadius);
    }
#endif

}
