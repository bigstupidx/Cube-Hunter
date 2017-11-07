using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GooglePlayServices
{
    public interface AdManagerListener
    {
        void AdManagerMessage(string messageText);
        void AdManagerMessage(string messageKey, string messageText);
        void RewardedAdWatchedStatus(bool isWatched);
    }
}
