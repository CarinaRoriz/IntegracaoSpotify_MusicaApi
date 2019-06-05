using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IntegracaoSpotify_MusicaApi.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IntegracaoSpotify_MusicaApi.Controllers
{
    [Route("v1/musicas")]
    [ApiController]
    public class MusicaController : ControllerBase
    {
        [HttpGet("busca")]
        public async Task<ActionResult<BuscaMusica>> GetMusica(string nomeMusica, string artista)
        {
            string token = Request.Headers["Authorization"];

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            UriBuilder builder = new UriBuilder("https://api.spotify.com/v1/search");
            builder.Query = $"q=track:{nomeMusica}%20artist:{artista}&type=track";
            
            HttpResponseMessage resp = await client.GetAsync(builder.Uri);
            string msg = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BuscaMusica>(msg.Replace("\n",""));
        }

        [HttpGet]
        public async Task<ActionResult<Musica>> GetMusicaPorId(string id)
        {
            string token = Request.Headers["Authorization"];

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            HttpResponseMessage resp = await client.GetAsync($"https://api.spotify.com/v1/tracks/{id}");
            string msg = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Musica>(msg.Replace("\n", ""));
        }

        [HttpGet("busca/letra")]
        public async Task<ActionResult<MusicasLetra>> GetMusicaLetra(string id, string artista)
        {
            string token = Request.Headers["Authorization"];

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            
            HttpResponseMessage resp = await client.GetAsync($"https://api.spotify.com/v1/tracks/{id}");
            string msg = await resp.Content.ReadAsStringAsync();

            Musica musica = JsonConvert.DeserializeObject<Musica>(msg.Replace("\n", ""));

            HttpClient clientLetra = new HttpClient();

            UriBuilder builderLetra = new UriBuilder("https://localhost:5006/v1/letrasMusica");
            builderLetra.Query = $"nomeMusica={musica.name}&artista={musica.artists.FirstOrDefault().name}";

            HttpResponseMessage respLetra = await clientLetra.GetAsync(builderLetra.Uri);
            string msgLetra = await respLetra.Content.ReadAsStringAsync();

            LetrasMusicaBusca letra = JsonConvert.DeserializeObject<LetrasMusicaBusca>(msgLetra.Replace("\n", ""));

            return new MusicasLetra
            {
                Musica = musica,
                Letra = letra
            };
        }
    }
}