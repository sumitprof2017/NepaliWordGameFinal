using System.Collections;
#if Unity_IOS
using Unity.Notifications.iOS;
#endif
using UnityEngine;
#if Unity_IOS
public class IosNotifications : MonoBehaviour
{
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public IEnumerator RequestAuthorization()
    {
        using var request = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge,true);
        while (!request.IsFinished) {
        yield return null;
        }

    }

    public void SendNotification(string title,string body,string subtitle,int fireTimeInHours)
    {
        var timneTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new System.TimeSpan(fireTimeInHours, 0, 0),
            Repeats = false
        };

        var notifications = new iOSNotification()
        {
            Identifier = "Lets learn some new words today.",
            Title = title,
            Body = body,
            Subtitle = subtitle,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Badge),
            CategoryIdentifier = "default_category",
            ThreadIdentifier = "thread1",
            Trigger = timneTrigger
        };

        iOSNotificationCenter.ScheduleNotification(notifications);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
#endif