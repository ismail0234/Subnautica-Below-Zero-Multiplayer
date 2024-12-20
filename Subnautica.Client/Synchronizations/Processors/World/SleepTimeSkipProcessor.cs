namespace Subnautica.Client.Synchronizations.Processors.World
{
    using System.Collections;

    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Modules;
    using Subnautica.Network.Models.Core;

    using UnityEngine;

    using UWE;

    using ServerModel = Subnautica.Network.Models.Server;

    public class SleepTimeSkipProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.SleepTimeSkipArgs>();
            if (packet == null)
            {
                return false;
            }

            if (DayNightCycle.main.timePassedAsFloat < packet.SkipModeEndTime)
            {
                CoroutineHost.StartCoroutine(this.BedExitInUseMode(packet));
            }
            else
            {
                if (SleepScreen.Instance.IsEnabled)
                {
                    SleepScreen.Instance.ExitInUseMode();
                }
            }

            return true;
        }

        /**
         *
         * Oyuncuyu yataktan uyandırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator BedExitInUseMode(ServerModel.SleepTimeSkipArgs packet)
        {
            global::Player.main.timeLastSleep = packet.TimeLastSleep;

            float differenceTime = (DayNightCycle.main.timePassedAsFloat - packet.TimeLastSleep);

            DayNightCycle.main.skipTimeMode    = true;
            DayNightCycle.main.skipModeEndTime = (packet.TimeLastSleep + packet.TimeAmount) - differenceTime;
            DayNightCycle.main._dayNightSpeed  = packet.TimeAmount / packet.SkipDuration;

            float waitTime = packet.SkipDuration - (packet.SkipDuration * (differenceTime / packet.TimeAmount));

            yield return new WaitForSecondsRealtime(waitTime);

            if (SleepScreen.Instance.IsEnabled)
            {
                SleepScreen.Instance.ExitInUseMode();   
            }
        }
    }
}