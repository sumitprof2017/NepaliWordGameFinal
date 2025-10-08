using Google.Play.AppUpdate;
using System.Collections;
using UnityEngine;

public class SelfUpdate : MonoBehaviour
{
    private AppUpdateManager appUpdateManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        appUpdateManager = new AppUpdateManager();
        StartCoroutine(CheckForUpdate());
    }

    // Update is called once per frame

    private IEnumerator CheckForUpdate()
    {
        var updateInfoOp = appUpdateManager.GetAppUpdateInfo();
        yield return updateInfoOp;

        Debug.Log($"isSuccessful {updateInfoOp.IsSuccessful}");

        if (updateInfoOp.IsSuccessful)
        {
            var updateInfo = updateInfoOp.GetResult();

            Debug.Log($"updateAvailability {updateInfo.UpdateAvailability}");

            if (updateInfo.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                if (updateInfo.IsUpdateTypeAllowed(AppUpdateOptions.ImmediateAppUpdateOptions()))
                {
                    var options = AppUpdateOptions.ImmediateAppUpdateOptions();
                    var request = appUpdateManager.StartUpdate(updateInfo, options);
                    yield return request;

                    if (request.Status == AppUpdateStatus.Canceled || request.Status == AppUpdateStatus.Failed)
                    {
                        Debug.LogWarning("Update was dismissed or failed.");
                    }
                }
            }
        }
    }
}
