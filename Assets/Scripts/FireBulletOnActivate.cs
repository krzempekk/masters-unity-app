
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletOnActivate : MonoBehaviour {
    public GameObject bullet;
    public Transform spawnPoint;
    public float bulletSpeed = 20;

    void Start() {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
    }

    private void FireBullet(ActivateEventArgs arg) {
        GameObject spawnedBullet = Instantiate(bullet);
        spawnedBullet.transform.position = spawnPoint.position;
        spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * bulletSpeed;
        Destroy(spawnedBullet, 5);
    }
}
