using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MadokaMagica.Crystal.CrystalComponents
{
    internal class OrbManager : MonoBehaviour
    {
        public float attackSpeedStat;
        private float tick;
        public int combofactor;
        public enum orbs
        {
            none = 0,
            echo = 1,
            slice = 2,
            crystal = 3
        }
        public List<orbs> orbBar = new List<orbs>();

        public int RemoveOrb(int[] orbtoremove, orbs orbtype)
        {
            var count = 0;
            for (int i = 0; i < 3; i++)
            {
                if (orbtoremove[i] != -1)
                {
                    orbBar.RemoveAt(orbtoremove[i]);
                }
            }
            count++;
            return count;
        }

        public int[] ReturnOrbIndexies(orbs orbToReturn, bool returnSecondBatch)
        {
            int[] constructor = { -1, -1, -1 };
            for (int i = 0; i <= orbBar.Count - 1; i++)
            {
                if (orbBar[i] == orbToReturn)
                {
                    constructor[0] = i;
                    constructor[1] = i + 1 < orbBar.Count && orbBar[i + 1] == orbToReturn ? i + 1 : -1;
                    constructor[2] = i + 2 < orbBar.Count && orbBar[i + 2] == orbToReturn ? i + 2 : -1;
                    break;
                }
            }
            if (returnSecondBatch)
            {
                for (int i = 0; i <= orbBar.Count - 1; i++)
                {
                    if (orbBar[i] == orbToReturn && i != constructor[0] && i != constructor[1] && i != constructor[2])
                    {
                        constructor[0] = i;
                        constructor[1] = i + 1 < orbBar.Count && orbBar[i + 1] == orbToReturn ? i + 1 : -1;
                        constructor[2] = i + 2 < orbBar.Count && orbBar[i + 2] == orbToReturn ? i + 2 : -1;
                        break;
                    }
                }

            }
            return constructor;
        }

        public int[] ReturnNextOrbIndexies(int startIndex)
        {
            int[] constructor = { -1, -1, -1 };
            var nextorb = orbBar[startIndex + 1]; 
            constructor[0] = startIndex + 1 < orbBar.Count ? startIndex + 1 : -1;
            constructor[1] = startIndex + 2 < orbBar.Count && orbBar[startIndex + 2] == nextorb ? startIndex + 2 : -1;
            constructor[2] = startIndex + 3 < orbBar.Count && orbBar[startIndex + 2] == nextorb ? startIndex + 3 : -1;

            return constructor;
        }

        public int[] InvertedReturnOrbIndexies(orbs orbToReturn)
        {
            int[] constructor = { -1, -1, -1 };
            for (int i = orbBar.Count - 1; i >= 0; i--)
            {
                if (orbBar[i] == orbToReturn)
                {
                    constructor[0] = i;
                    constructor[1] = i - 1 >= 0 && orbBar[i - 1] == orbToReturn ? i - 1 : -1;
                    constructor[2] = i - 2 >= 0 && orbBar[i - 2] == orbToReturn ? i - 2 : -1;
                    break;
                }
            }
            return constructor;
        }
        void FixedUpdate()
        {
            tick += Time.fixedDeltaTime * attackSpeedStat * combofactor;
            if (tick > 1f)
            {
                var rand = Random.Range(1, 3);
                if (rand == 1) orbBar.Add(orbs.echo);
                if (rand == 2) orbBar.Add(orbs.slice);
                if (rand == 3) orbBar.Add(orbs.crystal);
            }
        }
    }
}
