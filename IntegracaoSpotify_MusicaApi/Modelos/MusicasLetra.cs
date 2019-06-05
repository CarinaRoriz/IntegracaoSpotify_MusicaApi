using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegracaoSpotify_MusicaApi.Modelos
{
    public class MusicasLetra
    {
        public Musica Musica { get; set; }
        public LetrasMusicaBusca Letra { get; set; }
    }
}
