using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GooglePlayServices
{
    public interface MPLobbyListener
    {
        void SetLobbyVisibility(bool visibility);
        void SetLobbyMessage(string message);
        void SetLobbyStatusPercent(float percent);
        void StartGame();
    }
}
