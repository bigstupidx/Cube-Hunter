using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Collections;
using GooglePlayGames.BasicApi.SavedGame;

namespace Assets.Scripts.GooglePlayServices
{
    public class GPGSManager : MonoBehaviour
    {
        public static GPGSManager Instance { get; set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (this != Instance)
                Destroy(this);

            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Google Play Service oyun hizmetlerinin yapılandırılmasını sağlar.
        /// </summary>
        /// <param name="isMultiplayerGame">Oyununuzda RTMultiplayer sınıfını kullanacaksanız değerini true,
        /// kullanmayacaksanız değerini false yapın.</param>
        public void Init()
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                .Build();

            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }
        
        /// <summary>
        /// Google Play Service cihazdaki herhangi bir hesaba giriş yapar.
        /// </summary>
        /// <param name="silent">silent değerinin false olması durumunda Google Play Service, bir hesaba başarılı bir şekilde 
        /// giriş yaparsa bununla ilgili bir popup gösterir ( "Hoşgeldin" şeklinde ) , eğer değeri true ise popup gösterilmez.</param>
        public void SignIn(bool silent)
        {
            if (!PlayGamesPlatform.Instance.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.Authenticate((bool success) =>
                {
                    if (success)
                    {
                        //Google Play Service popup arayüzü, yerine kendi arayüzünüzü göstermek 
                        //istiyoanız burada gösterebilirsiniz. Ya da başarılı giriş yapıldığında ayrı bir multiplayer 
                        //sahneniz varsa onu açabilir veya multiplayer ile ilgili bir buton varsa aktif edebilirisiniz.
                        Debug.Log("Hesaba giriş yapıldı.");
                    }
                    else
                    {
                        Debug.Log("HATA! Hesaba giriş yapılamadı.");
                    }

                }, silent);
            }
        }

        /// <summary>
        /// Oturum açılmış olan hesaptan çıkış yapar.
        /// </summary>
        public void SignOut()
        {
            PlayGamesPlatform.Instance.SignOut();
        }

        /// <summary>
        /// achievementId ye göre başarı kilidini açar.
        /// </summary>
        /// <param name="achievementId">Başarı(Achievement) ID</param>
        public void UnlockAchievement(string achievementId)
        {
            Social.ReportProgress(achievementId, 100.0f, (bool success) => {
                if (success)
                    Debug.Log("Başarı kilidi açıldı");
                else
                    Debug.Log("HATA! Başarı kilidi açılamadı.");
            });
        }

        /// <summary>
        /// Artan(Incremental) bir başarının(Achievement) değerini arttırır.
        /// </summary>
        /// <param name="achievementId">Başarı(Achievement) ID</param>
        /// <param name="incrementValue">Artım değeri</param>
        public void IncrementAchievement(string achievementId, int incrementValue)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(achievementId, incrementValue, (bool success) =>
            {
                if (success)
                    Debug.Log("Başarı arttırıldı.");
                else
                    Debug.Log("HATA! Başarı arttırılamadı.");
            });
        }

        /// <summary>
        /// Lider sıralama tablosuna skor gönderir. Skoru önceden kontrol etmenize gerek yok 
        /// çünkü server tarafında bu skorlar kontrol ediliyor eğer gönderilen skor oyuncunun 
        /// kayıtlı skorundan küçük ise Leaderboard'a eklenmiyor.
        /// </summary>
        /// <param name="score">Gönderilecek skor</param>
        /// <param name="leaderboardId">Leaderboard ID</param>
        public void PostScore(long score , string leaderboardId)
        {
            Social.ReportScore(score, leaderboardId, (bool success) => {
                if (success)
                    Debug.Log("Skor gönderildi");
                else
                    Debug.Log("HATA! Skor gönderilemedi.");
            });
        }

        /// <summary>
        /// Açılan ve açılmayan tüm başarıları(Achievement), Play Service UI'ı ile listeler.
        /// </summary>
        public void ShowAchivementUI()
        {
            Social.ShowAchievementsUI();
        }

        /// <summary>
        /// Tüm lider sıralamalarını(Leaderboard), Play Service UI'ı ile listeler.
        /// </summary>
        public void ShowLeaderboardUI()
        {
            Social.ShowLeaderboardUI();
        }

        /// <summary>
        /// Belirli bir lider sıralamasını(Leaderboard), Play Service UI'ı ile listeler.
        /// </summary>
        /// <param name="leaderboardId">Leaderboard ID</param>
        public void ShowLeaderboardUI(string leaderboardId)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
        }
        
    }
}
