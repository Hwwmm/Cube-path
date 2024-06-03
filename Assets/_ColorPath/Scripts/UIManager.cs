using SgLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if EASY_MOBILE
using EasyMobile;
#endif

public class UIManager : MonoBehaviour
{

    [Header("Object References")]
    public PlayerController playerController;
    public RandomBackground randomBackgroundController;
    public GameObject mainCanvas;
    public GameObject coloredButtons;
    public GameObject header;
    public GameObject title;
    public Text score;
    public Text progress;
    public Text level;
    public Text bestScore;
    public Text coinText;
    public GameObject newBestScore;
    public GameObject playBtn;
    public GameObject restartBtn;
    public GameObject menuButtons;
    public GameObject rewardUI;
    public GameObject settingsUI;
    public GameObject soundOnBtn;
    public GameObject soundOffBtn;
    public GameObject musicOnBtn;
    public GameObject musicOffBtn;

    [Header("Premium Features Buttons")]
    public GameObject watchRewardedAdBtn;
    public GameObject leaderboardBtn;
    public GameObject achievementBtn;
    public GameObject iapPurchaseBtn;
    public GameObject removeAdsBtn;
    public GameObject restorePurchaseBtn;

    [Header("In-App Purchase Store")]
    public GameObject storeUI;

    [Header("Sharing-Specific")]
    public GameObject shareUI;
    public ShareUIController shareUIController;

    Animator scoreAnimator;
    Animator dailyRewardAnimator;
    bool isWatchAdsForCoinBtnActive;

    void OnEnable()
    {
        GameManager.GameStateChanged += GameManager_GameStateChanged;
        ScoreManager.ScoreUpdated += OnScoreUpdated;
        ScoreManager.LevelUpdated += OnLevelUpdate;
    }

    void OnDisable()
    {
        GameManager.GameStateChanged -= GameManager_GameStateChanged;
        ScoreManager.ScoreUpdated -= OnScoreUpdated;
        ScoreManager.ScoreUpdated -= OnLevelUpdate;
    }

    // Use this for initialization
    void Start()
    {
        scoreAnimator = score.GetComponent<Animator>();

        Reset();
        ShowStartUI();
    }

    // Update is called once per frame
    void Update()
    {
        score.text = ScoreManager.Instance.Score.ToString();
        updateProgress();
        bestScore.text = ScoreManager.Instance.HighScore.ToString();
        coinText.text = CoinManager.Instance.Coins.ToString();
        level.text = ScoreManager.Instance.Level.ToString();

       
        if (settingsUI.activeSelf)
        {
            UpdateSoundButtons();
            UpdateMusicButtons();
        }
    }
    void updateProgress()
    {
        progress.text = string.Format("{0}/30", ScoreManager.Instance.Progress.ToString());
    }

    void GameManager_GameStateChanged(GameState newState, GameState oldState)
    {
        if (newState == GameState.Playing)
        {
            ShowGameUI();
        }
        else if (newState == GameState.PreGameOver)
        {
            // Before game over, i.e. game potentially will be recovered
        }
        else if (newState == GameState.GameOver)
        {
            Invoke("ShowGameOverUI", 1f);
        }
    }
   
    void OnScoreUpdated(int newScore)
    {
        scoreAnimator.Play("NewScore");
    }

    void OnLevelUpdate(int level)
    {
        randomBackgroundController.ChangeBackground();
        UpdateDestroyLevelTime();
        
    }
    void UpdateDestroyLevelTime()
    {
       
        print(playerController.destroyPlaneTime - 0.05f);
        float newtime = playerController.destroyPlaneTime - 0.05f;
        print("new time" +newtime);
        if (playerController.destroyPlaneTime >= 0.3)
        {
            playerController.destroyPlaneTime = newtime;
        }

    }

    void Reset()
    {
        mainCanvas.SetActive(true);
        header.SetActive(false);
        title.SetActive(false);
        score.gameObject.SetActive(false);
        newBestScore.SetActive(false);
        playBtn.SetActive(false);
        menuButtons.SetActive(false);
       

        // Enable or disable premium stuff
        bool enablePremium = IsPremiumFeaturesEnabled();
        leaderboardBtn.SetActive(enablePremium);
        iapPurchaseBtn.SetActive(enablePremium);
        removeAdsBtn.SetActive(enablePremium);
        restorePurchaseBtn.SetActive(enablePremium);

        // Hidden by default
        storeUI.SetActive(false);
        settingsUI.SetActive(false);
        shareUI.SetActive(false);

        // These premium feature buttons are hidden by default
        // and shown when certain criteria are met (e.g. rewarded ad is loaded)
        watchRewardedAdBtn.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void EndGame()
    {
        GameManager.Instance.GameOver();
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame(0.2f);
    }

    public void ShowStartUI()
    {

        settingsUI.SetActive(false);

        header.SetActive(true);
        title.SetActive(true);
        playBtn.SetActive(true);
        restartBtn.SetActive(false);
        menuButtons.SetActive(true);
        coloredButtons.SetActive(false);

        // If first launch: show "WatchForCoins" and "DailyReward" buttons if the conditions are met
        if (GameManager.GameCount == 0)
        {
            ShowWatchForCoinsBtn();
            ShowDailyRewardBtn();
        }
    }

    public void ShowGameUI()
    {
        playerController.StartGame();
        header.SetActive(true);
        title.SetActive(false);
        score.gameObject.SetActive(true);
        playBtn.SetActive(false);
        menuButtons.SetActive(false);
        watchRewardedAdBtn.SetActive(false);
        coloredButtons.SetActive(true);
    }

    public void ShowGameOverUI()
    {
        header.SetActive(true);
        title.SetActive(false);
        score.gameObject.SetActive(true);
        newBestScore.SetActive(ScoreManager.Instance.HasNewHighScore);

        playBtn.SetActive(false);
        restartBtn.SetActive(true);
        menuButtons.SetActive(true);
        settingsUI.SetActive(false);
        coloredButtons.SetActive(false);

        // Show 'daily reward' button
        ShowDailyRewardBtn();

        // Show these if premium features are enabled (and relevant conditions are met)
        if (IsPremiumFeaturesEnabled())
        {
            ShowShareUI();
            ShowWatchForCoinsBtn();
        }
    }

    void ShowWatchForCoinsBtn()
    {
        // Only show "watch for coins button" if a rewarded ad is loaded and premium features are enabled
#if EASY_MOBILE
        if (IsPremiumFeaturesEnabled() && AdDisplayer.Instance.CanShowRewardedAd() && AdDisplayer.Instance.watchAdToEarnCoins)
        {
            watchRewardedAdBtn.SetActive(true);
            watchRewardedAdBtn.GetComponent<Animator>().SetTrigger("activate");
        }
        else
        {
            watchRewardedAdBtn.SetActive(false);
        }
#endif
    }

    void ShowDailyRewardBtn()
    {
    }

    public void ShowSettingsUI()
    {
        settingsUI.SetActive(true);
    }

    public void HideSettingsUI()
    {
        settingsUI.SetActive(false);
    }

    public void ShowStoreUI()
    {
        storeUI.SetActive(true);
    }

    public void HideStoreUI()
    {
        storeUI.SetActive(false);
    }

    public void WatchRewardedAd()
    {
#if EASY_MOBILE
        // Hide the button
        watchRewardedAdBtn.SetActive(false);

        AdDisplayer.CompleteRewardedAdToEarnCoins += OnCompleteRewardedAdToEarnCoins;
        AdDisplayer.Instance.ShowRewardedAdToEarnCoins();
#endif
    }

    void OnCompleteRewardedAdToEarnCoins()
    {
#if EASY_MOBILE
        // Unsubscribe
        AdDisplayer.CompleteRewardedAdToEarnCoins -= OnCompleteRewardedAdToEarnCoins;

        // Give the coins!
        ShowRewardUI(AdDisplayer.Instance.rewardedCoins);
#endif
    }

    public void GrabDailyReward()
    {
        if (DailyRewardController.Instance.CanRewardNow())
        {
            int reward = DailyRewardController.Instance.GetRandomReward();

            // Round the number and make it mutiplies of 5 only.
            int roundedReward = (reward / 5) * 5;

            // Show the reward UI
            ShowRewardUI(roundedReward);

            // Update next time for the reward
            DailyRewardController.Instance.ResetNextRewardTime();
        }
    }

    public void ShowRewardUI(int reward)
    {
        rewardUI.SetActive(true);
        rewardUI.GetComponent<RewardUIController>().Reward(reward);
    }

    public void HideRewardUI()
    {
        rewardUI.GetComponent<RewardUIController>().Close();
    }

    public void ShowLeaderboardUI()
    {
#if EASY_MOBILE
        if (GameServices.IsInitialized())
        {
            GameServices.ShowLeaderboardUI();
        }
        else
        {
#if UNITY_IOS
            NativeUI.Alert("Service Unavailable", "The user is not logged in to Game Center.");
#elif UNITY_ANDROID
            GameServices.Init();
#endif
        }
#endif
    }

    public void ShowAchievementsUI()
    {
#if EASY_MOBILE
        if (GameServices.IsInitialized())
        {
            GameServices.ShowAchievementsUI();
        }
        else
        {
#if UNITY_IOS
            NativeUI.Alert("Service Unavailable", "The user is not logged in to Game Center.");
#elif UNITY_ANDROID
            GameServices.Init();
#endif
        }
#endif
    }

    public void PurchaseRemoveAds()
    {
#if EASY_MOBILE
        InAppPurchaser.Instance.Purchase(InAppPurchaser.Instance.removeAds);
#endif
    }

    public void RestorePurchase()
    {
#if EASY_MOBILE
        InAppPurchaser.Instance.RestorePurchase();
#endif
    }

    public void ShowShareUI()
    {
        if (!ScreenshotSharer.Instance.disableSharing)
        {
            Texture2D texture = ScreenshotSharer.Instance.CapturedScreenshot;
            shareUIController.ImgTex = texture;

#if EASY_MOBILE_PRO
            AnimatedClip clip = ScreenshotSharer.Instance.RecordedClip;
            shareUIController.AnimClip = clip;
#endif

            shareUI.SetActive(true);
        }
    }

    public void HideShareUI()
    {
        shareUI.SetActive(false);
    }

    public void ToggleSound()
    {
        SoundManager.Instance.ToggleSound();
    }

    public void ToggleMusic()
    {
        SoundManager.Instance.ToggleMusic();
    }

    public void RateApp()
    {
        Utilities.RateApp();
    }

    public void OpenTwitterPage()
    {
        Utilities.OpenTwitterPage();
    }

    public void OpenFacebookPage()
    {
        Utilities.OpenFacebookPage();
    }

    public void ButtonClickSound()
    {
        Utilities.ButtonClickSound();
    }

    void UpdateSoundButtons()
    {
        if (SoundManager.Instance.IsSoundOff())
        {
            soundOnBtn.gameObject.SetActive(false);
            soundOffBtn.gameObject.SetActive(true);
        }
        else
        {
            soundOnBtn.gameObject.SetActive(true);
            soundOffBtn.gameObject.SetActive(false);
        }
    }

    void UpdateMusicButtons()
    {
        if (SoundManager.Instance.IsMusicOff())
        {
            musicOffBtn.gameObject.SetActive(true);
            musicOnBtn.gameObject.SetActive(false);
        }
        else
        {
            musicOffBtn.gameObject.SetActive(false);
            musicOnBtn.gameObject.SetActive(true);
        }
    }

    bool IsPremiumFeaturesEnabled()
    {
        return PremiumFeaturesManager.Instance != null && PremiumFeaturesManager.Instance.enablePremiumFeatures;
    }
    public void LoadCharacterScene()
    {
        SceneManager.LoadScene("CharacterSelection");
    }
    public void ShowColoredButtons()
    {
        coloredButtons.SetActive(true);
    }

    public void HideColoredButtons()
    {
        coloredButtons.SetActive(false);
    }
}
