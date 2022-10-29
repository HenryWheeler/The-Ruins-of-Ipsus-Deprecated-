using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class TargetReticle
    {
        public static int x;
        public static int y;
        public static bool targeting = false;
        private static Player player;
        private static List<SFX> sfxPos;
        public TargetReticle(Player _player) { player = _player; sfxPos = new List<SFX>(); }
        public static void StartTargeting()
        {
            x = player.x; y = player.y;
            targeting = true; player.turnActive = false;
            Move(0, 0);
            RLKey key = RLKey.Unknown; Action.TargetAction(key);
        }
        public static void StopTargeting()
        {
            targeting = false; player.turnActive = true;
            foreach (SFX sfx in sfxPos) { Map.sfx[sfx.x, sfx.y] = null; }
            sfxPos.Clear();
            RLKey key = RLKey.Unknown; Action.PlayerAction(player, key);
            Log.ClearLogDisplay();
        }
        public static void Move(int _x, int _y)
        {
            if (CMath.CheckBounds(x + _x, y + _y))
            {
                foreach (SFX sfx in sfxPos) { Map.sfx[sfx.x, sfx.y] = null; }
                sfxPos.Clear();
                x += _x; y += _y;
                CreateLine();
            }
        }
        public static void CreateLine()
        {
            int t;
            int x = player.x; int y = player.y;
            int delta_x = TargetReticle.x - player.x; int delta_y = TargetReticle.y - player.y;
            int abs_delta_x = Math.Abs(delta_x); int abs_delta_y = Math.Abs(delta_y);
            int sign_x = Math.Sign(delta_x); int sign_y = Math.Sign(delta_y);
            bool hasConnected = false;

            if (abs_delta_x > abs_delta_y)
            {
                t = abs_delta_y * 2 - abs_delta_x;
                do
                {
                    if (t >= 0) { y += sign_y; t -= abs_delta_x * 2; }
                    x += sign_x;
                    t += abs_delta_y * 2;
                    if (x == TargetReticle.x && y == TargetReticle.y) { hasConnected = true; sfxPos.Add(Reticle(x, y, 'X', "Yellow")); }
                    else { sfxPos.Add(Reticle(x, y, '.', "White")); }
                }
                while (!hasConnected);
            }
            else
            {
                t = abs_delta_x * 2 - abs_delta_y;
                do
                {
                    if (t >= 0) { x += sign_x; t -= abs_delta_y * 2; }
                    y += sign_y;
                    t += abs_delta_x * 2;
                    if (x == TargetReticle.x && y == TargetReticle.y) { hasConnected = true; sfxPos.Add(Reticle(x, y, 'X', "Yellow")); }
                    else { sfxPos.Add(Reticle(x, y, '.', "White")); }
                }
                while (!hasConnected);
            }
        }
        public static void Fire()
        {
            if (Map.map[x, y].visible)
            {
                if (Map.map[x, y].actor != null)
                {
                    foreach (SFX sfx in sfxPos) { Map.sfx[sfx.x, sfx.y] = null; }
                    sfxPos.Clear();
                    player.Attack(Map.map[x, y].actor, player, 1);
                }
                else { Log.AddToStoredLog("There is nothing there for you to fire at.", true); }
            }
            else { Log.AddToStoredLog("You cannot fire at what you cannot see.", true); }
        }
        public static SFX Reticle(int x, int y, char character, string color)
        {
            SFX sfx = null;
            if (!Map.map[x, y].visible) { sfx = new SFX(x, y, character, "Gray", "Black", false); }
            else { sfx = new SFX(x, y, character, color, "Black", false); }
            Map.sfx[x, y] = sfx; return sfx; 
        }
    }
}
