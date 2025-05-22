using UnityEngine;

public class TileGround : MonoBehaviour {
    private bool playerExited = false;
    private float exitTime = 0f;
    public float exitThreshold = 2.5f; // Yarım saniye sonra silinsin

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerExited = true;
            exitTime = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerExited = false; // Geri döndü, silinmesin
        }
    }

    //EN ESKİ TILEDAN GERCEKTEN CIKTIGI AN SİLİNEBİLİR(ZIPLAMADIYSA)
    //ZIPLAMA DA EGER 2.5SN BOYUNCA TILE A GERİ DÖNMEDİYSE (ZIPLAMA SONRASI YERE DÜŞME) TILE SİLİNİR
    public bool ShouldBeDeleted() {
        return playerExited && (Time.time - exitTime > exitThreshold);
    }
}
