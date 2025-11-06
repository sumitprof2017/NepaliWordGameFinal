/*#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

#if Unity_IOS

using Unity.Notifications.iOS;

#endif
using UnityEngine;

public class NotificationController : MonoBehaviour
{

    [SerializeField] AndroidIosNotificationController androidNotifications;
#if Unity_IOS
    [SerializeField] IosNotifications iosNotifications;
#endif
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
#if UNITY_ANDROID
        androidNotifications.RequestAuthorization();
        androidNotifications.RegisterNotificationChannel();
#endif

#if Unity_IOS
        StartCoroutine(iosNotifications.RequestAuthorization());
#endif
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {

#if UNITY_ANDROID
            AndroidNotificationCenter.CancelAllNotifications();
            androidNotifications.SendNotification("Lets learn", "Lets learn some new words today", 15);
#endif

#if Unity_IOS
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            iosNotifications.SendNotification("Lets learn", "Lets learn some new words today", "Lets learn new words", 12);
#endif
        }
    }

    // Update is called once per frame

}
*/