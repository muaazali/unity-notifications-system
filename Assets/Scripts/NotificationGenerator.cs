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
    [Header("Object References")]
    // The parent object that holds all the notifications.
    [SerializeField] Transform notificationContainer;
    // Notification and Error gameobjects (Text or TextMeshPro) that serve as templates.
    [SerializeField] GameObject notification, error;

    [SerializeField] int maximumNotifications = 3;
    [SerializeField] float notificationLifetime = 5f;

    private Queue<Notification> notifications = new Queue<Notification>();

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

        // Before inserting into the queue, remove the previous one if the count exceeds maximum.
        if (notifications.Count >= maximumNotifications) {
            Notification notificationToDelete = notifications.Peek();
            StopCoroutine(notificationToDelete.coroutine);
            notificationToDelete.coroutine = StartCoroutine(DeleteNotification(0f));
        }

        Notification newNotification;
        newNotification.gameObject = notificationGameObject;
        newNotification.lifetime = notificationLifetime;
        newNotification.text = text;
        // Start the timer to delete this notification after newNotification.lifetime seconds.
        newNotification.coroutine = StartCoroutine(DeleteNotification(newNotification.lifetime));
        notifications.Enqueue(newNotification);
    }

    IEnumerator DeleteNotification(float seconds) {
        yield return new WaitForSeconds(seconds);
        // Remove from the queue and delete the gameobject.
        Notification deletedNotification = notifications.Dequeue();
        Destroy(deletedNotification.gameObject);
    }
}
