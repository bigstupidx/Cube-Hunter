using System;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections.Generic;

namespace Assets.Scripts.GooglePlayServices
{
    [RequireComponent(typeof(GPGSManager))]
    public class RTMultiplayer : MonoBehaviour, RealTimeMultiplayerListener
    {
        public static RTMultiplayer Instance { get; set; }
        public MPLobbyListener lobbyListener;
        public MPProcessListener processListener;
        private Invitation incomingInvitaiton;


        private void Start()
        {
            Instance = this;
            Init();
        }
        
        public void Init()
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .WithInvitationDelegate(OnInvitationReceived)
            .Build();

            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }

        /// <summary>
        /// Hızlı bir şekilde oluşturulmuş uygun bir oyun odası var mı diye kontrol eder.
        /// Eğer boş bir oyun odası yoksa belirlenen parametre değerlerince oyun odası kurar.
        /// Bu parametreler minimum ve maksimum katılımcı sayısını belirler. Burada dikkat etmeniz
        /// gereken yer katılımcı sayınıza kendinizi katmayacaksınız. Örneğin oyununuz iki kişilik
        /// bir oyun ise minimumOpponents = 1 ve maximumOpponents = 1 olmalıdır. Böylece Google Play Service
        /// sizin için iki kişilik bir oyun odası oluşturur veya sizi iki kişilik bir oyun odasına ekler.
        /// Başka bir örnek verecek olursak minimumOpponents = 3 ve maximumOpponents = 5 değerleri için
        /// oyun odanız 6 oyuncuya kadar destekler. Eğer Google Play Service belirli bir süre içinde 6 oyuncu
        /// bulamazsa daha fazla bekletmemek için bulduğu 4 oyuncu ile oyun odasını kurar.
        /// </summary>
        /// <param name="minimumOpponents">Minimum katılımcı sayısı.</param>
        /// <param name="maximumOpponents">Maksimum katılımcı sayısı.</param>
        /// <param name="gameVariation">Oyun içindeki farklı oyun tarzlarını belirten parametredir. Örnek olarak bir First-Person-Shooter
        /// oyununda oynamak için Verilen görevi tamamlama ve Hedef vurma şeklinde iki görev olsun. 1.si için gameVariation değeri 1 iken
        /// diğeri için 2 olur. Eğer oyununuzda bu gibi varyasyonlar yok ise varsayılan değer olarak 0 değerini vermelisiniz </param>
        public void QuickMatch(uint minimumOpponents, uint maximumOpponents, uint gameVariation)
        {
            PlayGamesPlatform.Instance.RealTime.CreateQuickGame(minimumOpponents, maximumOpponents, gameVariation, this);
        }

        /// <summary>
        /// Google Play Service' in davet arayüzü açılır. Açılan arayüzde aktif oyuncular listelenir ve birlikte
        /// oynamak için istek gönderebilirsiniz. Bunun yanında Aktif olsun ya da olmasın herhangi bir arkadaşınıza 
        /// istek göndererek arkadaşınızı oyuna davet edebilirsiniz. Ve oyun odasındaki katılımcı sayısı parametre 
        /// değerlerince belirlenir. Bu parametreler minimum ve maksimum katılımcı sayısını belirler. Burada dikkat etmeniz
        /// gereken yer katılımcı sayınıza kendinizi katmayacaksınız. Örneğin oyununuz iki kişilik
        /// bir oyun ise minimumOpponents = 1 ve maximumOpponents = 1 olmalıdır. Böylece Google Play Service
        /// sizin için iki kişilik bir oyun odası oluşturur veya sizi iki kişilik bir oyun odasına ekler.
        /// Başka bir örnek verecek olursak minimumOpponents = 3 ve maximumOpponents = 5 değerleri için
        /// oyun odanız 6 oyuncuya kadar destekler. Eğer Google Play Service belirli bir süre içinde 6 oyuncu
        /// bulamazsa daha fazla bekletmemek için bulduğu 4 oyuncu ile oyun odasını kurar.
        /// </summary>
        /// <param name="minimumOpponents">Minimum katılımcı sayısı.</param>
        /// <param name="maximumOpponents">Maksimum katılımcı sayısı.</param>
        /// <param name="gameVariation">Oyun içindeki farklı oyun tarzlarını belirten parametredir. Örnek olarak bir First-Person-Shooter
        /// oyununda oynamak için Verilen görevi tamamlama ve Hedef vurma şeklinde iki görev olsun. 1.si için gameVariation değeri 1 iken
        /// diğeri için 2 olur. Eğer oyununuzda bu gibi varyasyonlar yok ise varsayılan değer olarak 0 değerini vermelisiniz </param>
        public void QuickMatchWithInvitation(uint minimumOpponents, uint maximumOpponents, uint gameVariation)
        {
            PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(minimumOpponents, maximumOpponents, gameVariation, this);
        }

        /// <summary>
        /// Oyun odasından çıkmanızı sağlar.
        /// </summary>
        public void LeaveRoom()
        {
            PlayGamesPlatform.Instance.RealTime.LeaveRoom();
            lobbyListener = null;
            processListener = null;            
        }

        /// <summary>
        /// Oyun odasındaki bağlı oyuncuların listesini Participant listesi olarak döndürür.
        /// </summary>
        /// <returns></returns>
        public List<Participant> GetAllPlayers()
        {
            return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
        }

        /// <summary>
        /// Oyun odasındaki kendi oyuncu bilgilerinizi Participant olarak döndürür. Bu bilgiler aktif oyun odasında geçerlidir.
        /// oyun odası kapatıldığında her oyuncunun katılımcı bilgileri silinir. Ve yeni bir oyun odasına girdiğinde farklı
        /// katılımcı bilgileri alır.
        /// </summary>
        /// <returns></returns>
        public Participant GetSelf()
        {
            return PlayGamesPlatform.Instance.RealTime.GetSelf();         
        }

        /// <summary>
        /// Oyun odasındaki kendi oyuncu ID bilginizi döndürür. Bu ID değeri sadece bulunduğu aktif oyun odasında geçerlidir.
        /// Farklı bir oyun odasına girdiğinde bu Id değeri değişecektir.
        /// </summary>
        /// <returns></returns>
        public string GetMyPlayerId()
        {
            return GetSelf().ParticipantId;
        }
        
        /// <summary>
        /// Google Play Service' in oyuncuların odaya katılması beklerken gösteridiği bekleme arayüzü.
        /// </summary> 
        public void ShowWaitingUI()
        {
            PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
        }

        /// <summary>
        /// Gelen daveti kabul eder.
        /// </summary>
        public void AcceptInvitation()
        {
            PlayGamesPlatform.Instance.RealTime.AcceptInvitation(incomingInvitaiton.InvitationId, Instance);
        }

        /// <summary>
        /// Gelen daveti reddeder.
        /// </summary>
        public void DeclineInvitation()
        {
            PlayGamesPlatform.Instance.RealTime.DeclineInvitation(incomingInvitaiton.InvitationId);
            incomingInvitaiton = null;
        }

        /// <summary>
        /// Oyun içinde gönderilmek istenen veriyi oyun odasına bağlı tüm oyunculara gönderir.
        /// Bu veriler byte dizisi şeklinde paketlenmiş halde pozisyon , rotasyon , ateş etme ... gibi 
        /// veriler olabilir.
        /// </summary>
        /// <param name="reliable">Değeri eğer true ise güvenilir bir mesaj gönderir. Bunu TCP Protokollerini
        /// kullanarak yapar ve gönderilen verinin diğer oyunculara iletildiğinden emin olur. Eğer değeri false 
        /// ise UDP protokolünü kullanır ve veri kayıpları yaşanabilir. Ama saniyede 6 ila 30 defa arasında veri
        /// gönderildiği için önemli bir eksiklik yaratmayacaktır. reliable ve unreliable arasındaki diğer fark ise
        /// unreliable gönderimlerin hızlı olmasıdır ve hız oyun için veri gönderiminde oldukça önemli bir unsurdur.
        /// Buyüzden pozisyon , rotasyon gibi sürekli gönderilecek verileri UDP protokolü ile gönderilmelidir. Böyle 
        /// yapmak zorunda değilsiniz ama yaygın kullanımı böyledir. Eğer Bu tip işlemlerde TCP kullanırsanız yavaşlık
        /// sorunu ile karşı karşıya kalırsınız. TCP protokolünü kullanmanız gereken yer çok büyük önem arz eden veri
        /// gönderimleridir ki buna oyun bitirme mesajı örnek olarak verilebilir. Eğer bir kullanıcı oyunu bitirdiyse 
        /// diğer oyuncular oynamaya devam etmemeliler ( tabi bu oyunun senaryosuna göre değişiklik gösterebilir )
        /// bu yüzden oyunu bitirme mesajının diğer kullanıcılara gittiğinden emin olmak için TCP protokollerini
        /// kullanmalıyız.</param>
        /// <param name="data">Paketlenmiş gönderilmek istenen veri</param>
        public void SendDataToAll(bool reliable , byte[] data)
        {
            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliable, data);
        }

        /// <summary>
        /// Oyun içinde gönderilmek istenen veriyi oyun odasına bağlı tüm oyunculara gönderir.
        /// Bu veriler byte dizisi şeklinde paketlenmiş halde pozisyon , rotasyon , ateş etme ... gibi 
        /// veriler olabilir.
        /// </summary>
        /// <param name="reliable">Değeri eğer true ise güvenilir bir mesaj gönderir. Bunu TCP Protokollerini
        /// kullanarak yapar ve gönderilen verinin diğer oyunculara iletildiğinden emin olur. Eğer değeri false 
        /// ise UDP protokolünü kullanır ve veri kayıpları yaşanabilir. Ama saniyede 6 ila 30 defa arasında veri
        /// gönderildiği için önemli bir eksiklik yaratmayacaktır. reliable ve unreliable arasındaki diğer fark ise
        /// unreliable gönderimlerin hızlı olmasıdır ve hız oyun için veri gönderiminde oldukça önemli bir unsurdur.
        /// Buyüzden pozisyon , rotasyon gibi sürekli gönderilecek verileri UDP protokolü ile gönderilmelidir. Böyle 
        /// yapmak zorunda değilsiniz ama yaygın kullanımı böyledir. Eğer Bu tip işlemlerde TCP kullanırsanız yavaşlık
        /// sorunu ile karşı karşıya kalırsınız. TCP protokolünü kullanmanız gereken yer çok büyük önem arz eden veri
        /// gönderimleridir ki buna oyun bitirme mesajı örnek olarak verilebilir. Eğer bir kullanıcı oyunu bitirdiyse 
        /// diğer oyuncular oynamaya devam etmemeliler ( tabi bu oyunun senaryosuna göre değişiklik gösterebilir )
        /// bu yüzden oyunu bitirme mesajının diğer kullanıcılara gittiğinden emin olmak için TCP protokollerini
        /// kullanmalıyız.</param>
        /// <param name="paricipantId">Verinin gönderileceği oyuncunun katılımcı Id değeri </param>
        /// <param name="data">Paketlenmiş gönderilmek istenen veri</param>
        public void SendDataToPlayer(bool reliable, string participantId, byte[] data)
        {
            PlayGamesPlatform.Instance.RealTime.SendMessage(reliable, participantId, data);
        }

        /// <summary>
        /// Başka oyuncular tarafından gönderilen oyun davetlerini yakalar.
        /// </summary>
        /// <param name="invitation">Gönderilen oyun daveti</param>
        /// <param name="shouldAutoAccept">Eğer bu değer true ise gelen davet hemen kabul edilir ve otomatik olarak oyuncu oyuna girer. 
        /// Eğer false olursa değeri, bir davet kabul etme arayüzü göstererek seçimi kullanıcıya bırakmış olursunuz.
        /// shouldAutoAccept = false olması durumunda gelen davet incomingInvitaiton değişkenine aktarılır ve bunu değişkeni oyun içinde sürekli
        /// kontrol ederek herhangi bir davet geldiğinde oyuncuya gösterebilirisiniz.</param>
        public void OnInvitationReceived(Invitation invitation, bool shouldAutoAccept)
        {
            if (shouldAutoAccept)
            {
                //Oyun başlar.
                //Kendinize ait bir bekleme arayüzü hazırladıysanız burada kullanabilirsiniz.
                //Eğer bir bekleme UI'nız yoksa ShowWaitingUI() metodunu kullanın.
                PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitation.InvitationId, Instance);
            }
            else
            {
                incomingInvitaiton = invitation;
            }
        }

        private void ShowMessage(string message)
        {
            Debug.Log(message);
            if (lobbyListener != null)
                lobbyListener.SetLobbyMessage(message);
        }


        #region RealTimeMultiplayerListener Methods

        /// <summary>
        /// Odadan ayrılıp ayrılmadığınızı dinler.
        /// </summary>
        public void OnLeftRoom()
        {
            ShowMessage("Odadan ayrıldınız.");

            if (processListener != null)
                processListener.LeaveRoom();
        }

        /// <summary>
        /// Bir oyuncunun odadan ayrılıp ayrılmadığını dinler.
        /// </summary>
        /// <param name="participant">Odadan ayrılan oyuncunun Participant türündeki bilgileri.</param>
        public void OnParticipantLeft(Participant participant)
        {
            ShowMessage("Oyuncu " + participant.ParticipantId + " ayrıldı.");
            if (processListener != null)
                processListener.PlayerLeftRoom(participant.ParticipantId);
        }

        /// <summary>
        /// Bağlanan oyuncu olup olmadığını dinler.
        /// </summary>
        /// <param name="participantIds">Katılan oyuncuların dizi şeklindeki katılımcı Id bilgileri.</param>
        public void OnPeersConnected(string[] participantIds)
        {
            foreach (string item in participantIds)
                ShowMessage("Oyuncu " + item + " odaya katıldı.");
        }

        /// <summary>
        /// Ayrılan oyuncunun olup olmadığını dinler.
        /// </summary>
        /// <param name="participantIds">Ayrılan oyuncuların dizi şeklindeki katılımcı Id bilgileri.</param>
        public void OnPeersDisconnected(string[] participantIds)
        {
            foreach (string item in participantIds)
                ShowMessage("Oyuncu " + item + " odadan ayrıldı.");
        }

        /// <summary>
        /// Diğer oyunculardan gönderilen veriler(konum,rotasyon ...) burada alınır ve işlenir.
        /// </summary>
        /// <param name="isReliable">Gelen verinin hangi protokolle gönderildiği bilgisini tutar.</param>
        /// <param name="senderId">Veriyi gönderen oyuncunun katılımcı Id'si.</param>
        /// <param name="data">Gelen veri.</param>
        public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
        {
            if (processListener != null)
                processListener.DataReceived(senderId, data);
        }

        /// <summary>
        /// Oyuncuyu bir oyun odasına eklemeye çalışır. OnRoomConnected methodunu kullanır.
        /// </summary>
        /// <param name="success">Oyuncuyu odaya ekleme işleminin sonucunu tutar. Eğer başarılı 
        /// bir şekilde odaya ekleme yapıldıysa değeri true , ekleme yapılamadıysa değeri false olur.</param>
        public void OnRoomConnected(bool success)
        {
            if (success)
            {
                if (lobbyListener != null)
                    lobbyListener.StartGame();
            }
            else
                ShowMessage("Odaya bağlanırken hata oluştu.");
        }

        /// <summary>
        /// Oyun kulurulumunun hangi aşamada olduğunu yüzdelik değerinde gösterir. Çok verimli değil
        /// fakat kullanıcının hiçbir şey görmemesindense bunu kullanmak oyunu daha iyi gösterebilir.
        /// </summary>
        /// <param name="percent">İşlemin yüzdelik değerini gösterir. %20 ye geldiğinde bekler ve katılımcı aramaya başlar.
        /// Katılımcı bulana kadar percent değeri 20 de sabit kalır.</param>
        public void OnRoomSetupProgress(float percent)
        {
            if (lobbyListener != null)
                lobbyListener.SetLobbyStatusPercent(percent);
        }
        #endregion
    }
}
