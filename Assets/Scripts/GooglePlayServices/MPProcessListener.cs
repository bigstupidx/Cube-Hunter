using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GooglePlayServices
{
    public interface MPProcessListener
    {
        void DataReceived(string senderId, byte[] data);
        void LeaveRoom();
        void PlayerLeftRoom(string playerId);
    }
}
