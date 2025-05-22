using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour {
    public Transform player;                     // Oyuncu referansı
    public GameObject tilePrefab;                // Tek tip tile prefab
    private float tileLength = 39.55f;              // Her tile'ın Z ekseninde uzunluğu
    public int tilesOnScreen = 5;                // Aynı anda ekranda olacak tile sayısı

    private float spawnZ = 0f;                   // Yeni tile'ın spawn olacağı Z pozisyonu
    private Queue<GameObject> activeTiles = new Queue<GameObject>();


    [SerializeField] private Transform tileVisual;      // tilePrefab içindeki TileVisual'ı drag-drop ile ver

    void Start() {
        // İlk tile'ı spawn et
        GameObject firstTile = SpawnTile();
        float firstZ = firstTile.transform.position.z;
        //Debug.Log($"🏃 First Pos: {firstTile.transform.position}");

        // İkinci tile'ı spawn et
        GameObject secondTile = SpawnTile();
        float secondZ = secondTile.transform.position.z;
        //Debug.Log($"🏃 Sec Pos: {secondTile.transform.position}");

        tileLength = Mathf.Abs(secondZ - firstZ);
        //Debug.Log($"📏 Measured tileLength: {tileLength}");

        // Visual'ı bul
        Transform visual = firstTile.transform.Find(tileVisual.name);
        float runnerZ = GetZ(visual);
        player.position = new Vector3(0, 0, runnerZ);
        Debug.Log($"🏃 Runner Pos: {player.position}");

        // Diğerlerini oluştur
        for (int i = 2; i < tilesOnScreen; i++) {
            SpawnTile();
        }
    }

    void Update() {
        // Yeni tile üretme mantığı
        if (player.position.z > (spawnZ - tileLength * tilesOnScreen)) {
            SpawnTile();
        }

        // 🔽 FIFO silme kontrolü — sadece en eski tile
        if (activeTiles.Count > 0) {
            GameObject oldestTile = activeTiles.Peek(); // sadece en eski tile
            TileGround tileGround = oldestTile.GetComponentInChildren<TileGround>();

            if (tileGround != null && tileGround.ShouldBeDeleted()) {
                DeleteOldTile();
            }
        }
    }
    
    GameObject SpawnTile() {
        GameObject go = Instantiate(tilePrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);
        activeTiles.Enqueue(go);
        spawnZ += tileLength;
      //  Debug.Log("🧱 Tile world position: " + go.transform.position);
        return go;
    }


    //EN ESKİ TILEDAN GERCEKTEN CIKTIGI AN SİLİNEBİLİR(ZIPLAMADIYSA)
    //ZIPLAMA DA EGER 2.5SN BOYUNCA TILE A GERİ DÖNMEDİYSE (ZIPLAMA SONRASI YERE DÜŞME) TILE SİLİNİR
    void DeleteOldTile() {
        GameObject oldTile = activeTiles.Dequeue();
        Destroy(oldTile);
    }

    float GetZ(Transform visual) {
        Renderer[] renderers = visual.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return 0f;

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers) bounds.Encapsulate(r.bounds);

        return bounds.min.z;
    }


}
