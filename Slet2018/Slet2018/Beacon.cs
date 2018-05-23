using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace Slet2018
{
    [Serializable]
    public partial class Beacon
    {
        public string Id;

        public BeaconType beaconType;

        public Beacon(string id,  Beacon.BeaconType beacon_type)
        {
            Id = id;

            beaconType = beacon_type;
        }
        public string GetShortId()
        {
            string id_short_str = Id;
            if (Id.Length > 12)
            {
                id_short_str = Id.Substring(Id.Length - 12);
               
            }
            return id_short_str;
        }

        public override string ToString()
        {
            string BeaconTypeStr;
            switch (beaconType)
            {
                case BeaconType.ARTIFACT:
                    BeaconTypeStr = "Артефакт";
                    break;
                case BeaconType.COMMAND_RED:
                    BeaconTypeStr = "Команда красных";
                    break;
                case BeaconType.COMMAND_BLUE:
                    BeaconTypeStr = "Команда синих";
                    break;
                case BeaconType.COMMAND_GREEN:
                    BeaconTypeStr = "Команда зеленых";
                    break;
                case BeaconType.COMMAND_YELLOW:
                    BeaconTypeStr = "Команеда желтых";
                    break;
                default:
                    BeaconTypeStr = "Нет типа";
                    break;
            }
            return (String.Format("ID: {0} Command : {1} Income {2} Тип : {3}", GetShortId(),  BeaconTypeStr));
        }

        //beaconType { get => beaconType; set => beaconType = value; }




    }
}