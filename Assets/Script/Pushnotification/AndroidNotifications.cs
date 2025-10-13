using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.Android;
public class AndroidNotifications : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void RequestAuthorization()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.Post_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.Post_NOTIFICATIONS");
        }
    }

    // Update is called once per frame
   public void RegisterNotificationChannel()
    {

        var channel = new AndroidNotificationChannel
        {
            Id = "default_channel",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Lets learn some new words today."
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }


    //set up noptofication template
    public void SendNotification(string title,string text,int fireTTimeInHours)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddHours(fireTTimeInHours);
        notification.LargeIcon = "icon_0";

        AndroidNotificationCenter.SendNotification(notification, "default_channel");
    }
}
