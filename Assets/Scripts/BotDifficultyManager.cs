using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;

public class BotDifficultyManager : MonoBehaviour
{
    [SerializeField] Bot bot;
    [SerializeField] int selectedDifficulty;
    [SerializeField] BotStats[] botDifficulties;
    [Header("Remote Config Parameters: ")]
    [SerializeField] bool enableRemotConfig = false;
    [SerializeField] string difficultyKey = "Difficulty";
    struct userAttribute{};
    struct appAttribute{};

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(()=> bot.isReady);

        //set stats default dari difficulty manager
        // sesuai selectedDifficulty dari inspector
        var newStats = botDifficulties[selectedDifficulty];
        bot.SetStats(newStats,true);

        // ambil difficulty dari remote config kalau enabbled
        if(enableRemotConfig == false)
        {
            yield break;
        }
        // tapi tunggu duulu sampe unity services siap
        yield return new WaitUntil(()=> UnityServices.State == ServicesInitializationState.Initialized && AuthenticationService.Instance.IsSignedIn);

        // daftar dulu untuk event fetch completed
        RemoteConfigService.Instance.FetchCompleted += OnRemoteConfigFetched;
        // lalu fetch di sini. cukup sekali di awal permainan
        RemoteConfigService.Instance.FetchConfigsAsync(new userAttribute(), new appAttribute());
    }

    private void OnDestroy()
    {
        // jangan lupa unregister event untuk menghindari memory leak
        RemoteConfigService.Instance.FetchCompleted -= OnRemoteConfigFetched;
    }

    // setiap kali data baru sudah didapatkan (melalui fetch) fungsi ini akan dipanggil
    private void OnRemoteConfigFetched(ConfigResponse response)
    {
        if(RemoteConfigService.Instance.appConfig.HasKey(difficultyKey) == false)
        {
            return;
        }

        switch (response.requestOrigin)

        {
            case ConfigOrigin.Default:
            case ConfigOrigin.Cached:
                break;    
            case ConfigOrigin.Remote:
                selectedDifficulty = RemoteConfigService.Instance.appConfig.GetInt(difficultyKey);
                selectedDifficulty = Mathf.Clamp(selectedDifficulty,0,botDifficulties.Length-1);
                var newStats = botDifficulties[selectedDifficulty];
                bot.SetStats(newStats,true);
                break;
        }
    }
}
