using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct Notification {
    public string text;
    public float lifetime;
    public GameObject gameObject;
    public Coroutine coroutine;
}

public class NotificationGenerator : MonoBehaviour
{
    [SerializeField] Transform notificationContainer;
    [SerializeField] GameObject notification, error;

    [SerializeField] int maximumNotifications = 3;
    [SerializeField] float notificationLifetime = 5f;

    private Queue<Notification> notifications = new Queue<Notification>();

    void Start() {
        //notifications = new Queue<GameObject>();
    }

    public void GenerateNotification(string text) {
        GameObject duplicateNotification = Instantiate(notification, notificationContainer);
        ShowNotification(duplicateNotification, text, 3f);
    }

    public void GenerateError(string text) {
        GameObject duplicateError = Instantiate(error, notificationContainer);
        ShowNotification(duplicateError, text, 5f);
    }

    void ShowNotification(GameObject notificationGameObject, string text, float secondsToShow) {
        notificationGameObject.GetComponent<TextMeshProUGUI>().text = text;
        notificationGameObject.SetActive(true);
        if (notifications.Count >= maximumNotifications) {
            Notification notificationToDelete = notifications.Peek();
            StopCoroutine(notificationToDelete.coroutine);
            notificationToDelete.coroutine = StartCoroutine(DeleteNotification(0f));
        }
        Notification newNotification;
        newNotification.gameObject = notificationGameObject;
        newNotification.lifetime = notificationLifetime;
        newNotification.text = text;
        newNotification.coroutine = StartCoroutine(DeleteNotification(newNotification.lifetime));
        notifications.Enqueue(newNotification);
    }

    IEnumerator DeleteNotification(float seconds) {
        yield return new WaitForSeconds(seconds);
        Notification deletedNotification = notifications.Dequeue();
        Destroy(deletedNotification.gameObject);
    }
}
