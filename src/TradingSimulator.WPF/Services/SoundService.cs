using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;

namespace TradingSimulator.WPF.Services
{
    public class SoundService
    {
        public void Play(string fileName)
        {
            try
            {
                var path = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Assets", "Sounds", fileName);

                var player = new SoundPlayer(path);
                player.Play();
            }
            catch
            {
            }
        }
    }
}

