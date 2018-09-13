using System;

namespace NonStandardEvents
{
    public delegate void NetworkFoundEventHandler(string ssid);

    public delegate void ExtendedNetworkFoundEventHandler(string ssid, int strength);

    class WifiScanner
    {
        public event NetworkFoundEventHandler NetworkFound = delegate { };

        public event ExtendedNetworkFoundEventHandler ExtendedNetworkFound = delegate { };

        public event Action Connected = delegate { };

        // rest of the code
        public void RaiseFound(string ssid, int strength = 0)
        {
            NetworkFound(ssid);
            ExtendedNetworkFound(ssid, strength);
        }

        public void RaiseConnected()
        {
            Connected();
        }
    }
}
