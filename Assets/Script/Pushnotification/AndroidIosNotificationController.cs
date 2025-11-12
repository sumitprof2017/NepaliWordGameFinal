using UnityEngine;
using Unity.Notifications;
using System.Collections;
using System;
using Unity.Notifications.Android;
public class AndroidIosNotificationController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
        StartCoroutine(RequestAuthorization());
        DontDestroyOnLoad(this);
    }

    public IEnumerator RequestAuthorization()
    {
        var request = NotificationCenter.RequestPermission();
        if (request.Status == NotificationsPermissionStatus.RequestPending)
            yield return request;
        Debug.Log("Permission result: " + request.Status);


    }

    public void Initialize()
    {
        var args = NotificationCenterArgs.Default;
        args.AndroidChannelId = "default";
        args.AndroidChannelName = "Notifications";
        args.AndroidChannelDescription = "Main notifications";
        NotificationCenter.Initialize(args);
    }


    //set up noptofication template
    public void SendNotification(string title, string text, int fireTime)
    {
        NotificationCenter.CancelScheduledNotification(1);

        var n = new Notification()
        {
            Identifier = 1,
            Title = "Lets learn some new words.",
            Text = "Time to learn",
        };
        var when = DateTime.Now.AddHours(fireTime);
        NotificationCenter.ScheduleNotification(n, new NotificationDateTimeSchedule(when));
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {

            SendNotification("Lets learn", "Lets learn some new words today", 8);

        }
    }
}


