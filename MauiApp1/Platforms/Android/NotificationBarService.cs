using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;

namespace MauiApp1;

[Service]
public class NotificationBarService : Service
{
    public static string AppName { get; private set; }
    public static string NotificationChannelId { get; private set; }
    public static int ServiceRunningNotificationId { get; private set; }
    public static int LargeIcon { get; private set; }
    public static int SmallIcon { get; private set; }

    public static Context Context { get; private set; }
    public static NotificationBarService Current { get; private set; }

    public NotificationManager NotificationManager { get; private set; }
    public Notification.Builder Builder { get; private set; }


    public static void Init(string appName, string notificationChannelId, int serviceRunningNotificationId, int largeIcon, int smallIcon)
    {
        AppName = appName;
        LargeIcon = largeIcon;
        SmallIcon = smallIcon;
        NotificationChannelId = notificationChannelId;
        ServiceRunningNotificationId = serviceRunningNotificationId;
    }

    public override IBinder OnBind(Intent intent)
    {
        return null;
    }

    public override void OnDestroy()
    {
        if (Current == this)
            Current = null;

        base.OnDestroy();
    }

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        if (Current != null)
            return StartCommandResult.Sticky;

        Current = this;
        Context = MainActivity.Current;

        NotificationManager = (NotificationManager)Context.GetSystemService(NotificationService);

        Intent launchIntent = Context.PackageManager.GetLaunchIntentForPackage(PackageName);

        var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
            : PendingIntentFlags.UpdateCurrent;

        PendingIntent pendingIntent = PendingIntent.GetActivity(Context, 0, launchIntent, pendingIntentFlags);

        Builder = new Notification.Builder(Context, NotificationChannelId)
            .SetSmallIcon(SmallIcon)
            .SetLargeIcon(BitmapFactory.DecodeResource(Resources, LargeIcon))
            .SetContentIntent(pendingIntent)
            .SetOngoing(true)
            //.AddAction(BuildRestartTimerAction())
            //.AddAction(BuildStopServiceAction())
            ;

        Builder.SetContentTitle(AppName);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel(NotificationChannelId, AppName, NotificationImportance.Default);
            channel.SetSound(null, null);
            NotificationManager.CreateNotificationChannel(channel);
        }

        // Enlist this instance of the service as a foreground service
        StartForeground(ServiceRunningNotificationId, Builder.Build());

        return StartCommandResult.Sticky;
    }

}
