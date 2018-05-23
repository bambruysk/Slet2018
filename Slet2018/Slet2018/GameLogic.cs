using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Slet2018
{
    public class GameLogic
    {
        public int CurrentForce;

        public int Max_HP;
        public int HP;
        public int RedPlayers;
        public int GreenPlayers;
        public int BluePlayers;
        public int YellowPlayers;
        public enum CommandColors { RED, GREEN, BLUE, YELLOW, NOBODY }
        private Dictionary<CommandColors, int> players;
        private Dictionary<CommandColors, int> winPoints;
        public CommandColors currentOwner;
        private Timer gameTickTimer;
        private BeaconFilter beaconFilter;
        private BluetoothDataLayer bleDataLayer;
        public enum PointState { NEUTRAL, ATTACKED, IS_OWNED }
        public PointState pointState;
        public int elapsedTime;
        public int gameTimeInterval;

        private CommandColors CommandInAttcak;
        public GameLogic(int max_hp, int gameTime)
        {
            gameTickTimer = new Timer(1000);
            gameTickTimer.Elapsed += Timer_Elapsed;
            bleDataLayer = new BluetoothDataLayer();
            beaconFilter = new BeaconFilter(bleDataLayer);
            players = new Dictionary<CommandColors, int>();
            players.TryAdd(CommandColors.RED, 0);
            players.TryAdd(CommandColors.GREEN, 0);
            players.TryAdd(CommandColors.BLUE, 0);
            players.TryAdd(CommandColors.YELLOW, 0);
            winPoints.TryAdd(CommandColors.RED, 0);
            winPoints.TryAdd(CommandColors.GREEN, 0);
            winPoints.TryAdd(CommandColors.BLUE, 0);
            winPoints.TryAdd(CommandColors.YELLOW, 0);
            pointState = PointState.NEUTRAL;
            Max_HP = max_hp;
            HP = Max_HP;
            currentOwner = CommandColors.NOBODY;
            elapsedTime = gameTime;
            gameTimeInterval = gameTime;
        }

        public void StartGame()
        {
            gameTickTimer.Start();
        }

        public void ResetGame()
        {
            gameTickTimer.Stop();

            HP = Max_HP;
            currentOwner = CommandColors.NOBODY;
            elapsedTime = gameTimeInterval;

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int _redPlayers = 0;
            int _greenPlayers = 0;
            int _bluePlayers = 0;
            int _yellowPlayers = 0;
            bool commandIsChanged = false;
            elapsedTime--;
            if (elapsedTime == 0)
            {
                StopGame();
            }
            List<Beacon> beacons = beaconFilter.GetActiveBeeacons();
            foreach (var b in beacons)
            {
                switch (b.beaconType)
                {
                    case Beacon.BeaconType.ARTIFACT:
                        // Need think about it
                        break;
                    case Beacon.BeaconType.COMMAND_BLUE:
                        _bluePlayers++;
                        break;
                    case Beacon.BeaconType.COMMAND_RED:
                        _redPlayers++;
                        break;
                    case Beacon.BeaconType.COMMAND_YELLOW:
                        _yellowPlayers++;
                        break;
                    case Beacon.BeaconType.COMMAND_GREEN:
                        _greenPlayers++;
                        break;
                }
            }
            players[CommandColors.BLUE] = _bluePlayers;
            players[CommandColors.GREEN] = _greenPlayers;
            players[CommandColors.RED] = _redPlayers;
            players[CommandColors.YELLOW] = _yellowPlayers;
            if (players.Max().Value != players.Min().Value)
            {
                if (players.Max().Key != CommandInAttcak)
                {
                    commandIsChanged = true;
                }
                CommandInAttcak = players.Max().Key;
            }
            else
            {
                CommandInAttcak = CommandColors.NOBODY;
            }
            if (CommandInAttcak != currentOwner) ;
            switch (pointState)
            {
                case PointState.NEUTRAL:
                    {
                        if (CommandInAttcak != CommandColors.NOBODY)
                        {
                            pointState = PointState.ATTACKED;
                        }
                        break;
                    }
                case PointState.ATTACKED:
                    {
                    winPoints[currentOwner]++;
                        HP--;
                        if (HP == 0)
                        {
                            currentOwner = CommandInAttcak;
                            pointState = PointState.IS_OWNED;
                            HP = Max_HP;
                        }
                        break;
                    }
                case PointState.IS_OWNED:
                    winPoints[currentOwner]++;
                    if (commandIsChanged)
                        pointState = PointState.ATTACKED;
                    break;
                default:
                    break;
            }
        }

        private void StopGame()
        {
            gameTickTimer.Stop();

        }
    }
}