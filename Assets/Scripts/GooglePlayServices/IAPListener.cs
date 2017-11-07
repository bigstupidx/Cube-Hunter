using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GooglePlayServices
{
    public interface IAPListener
    {
        void PurchaseStatus(string productId , bool success);
    }
}
