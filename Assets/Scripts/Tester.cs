using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] NotificationGenerator notificationGenerator;

    void Update() {
        if (Input.GetKeyDown(KeyCode.W)) {
            notificationGenerator.GenerateNotification("W is pressed.");
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            notificationGenerator.GenerateError("ERROR!");
        }
    }
}
