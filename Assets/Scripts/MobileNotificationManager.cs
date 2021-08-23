using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class MobileNotificationManager : MonoBehaviour
{
#if UNITY_ANDROID
    
    public AndroidNotificationChannel defaultNotificationChannel;
    
    private AndroidNotification notification;
    private int identifier;

    void Start()
    {
        defaultNotificationChannel = new AndroidNotificationChannel()
        {
            Id = "default_channel",
            Name = "Default channel",
            Description = "For generic notifications",
            Importance = Importance.Default
        };

        AndroidNotificationCenter.RegisterNotificationChannel(defaultNotificationChannel);

        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler =
        delegate (AndroidNotificationIntentData data)
        {
            var msg = "Notification received : " + data.Id + "\n";
            msg += "\n Notification received: ";
            msg += "\n .Title: " + data.Notification.Title;
            msg += "\n .Body: " + data.Notification.Text;
            msg += "\n .Channel: " + data.Channel;
            Debug.Log(msg);
        };

        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;
    }

    private void OnApplicationPause(bool pause)
    {
        DateTime now = DateTime.Now;
        notification = new AndroidNotification()
        {
            Title = "Chore Manager",
            Text = "Your chores are waiting for you!",
            SmallIcon = "app_icon_small",
            LargeIcon = "app_icon_large",
            FireTime = now.AddDays(1)
        };

        if (AndroidNotificationCenter.
        CheckScheduledNotificationStatus(identifier) == NotificationStatus.Scheduled)
        {
            // If the user has left the app and the app is not running, send them a new notification
            // Replace the currently sceduled notification with a new notification
            AndroidNotificationCenter.UpdateScheduledNotification(identifier, notification, "default_channel");
        }

        else if (AndroidNotificationCenter.
        CheckScheduledNotificationStatus(identifier) == NotificationStatus.Delivered)
        {
            // Remove the notification from the status bar
            AndroidNotificationCenter.CancelNotification(identifier);
        }

        else if (AndroidNotificationCenter.
        CheckScheduledNotificationStatus(identifier) == NotificationStatus.Unknown)
        {
            identifier = AndroidNotificationCenter.SendNotification(notification, "default channel");
        }
    }

#endif
}
