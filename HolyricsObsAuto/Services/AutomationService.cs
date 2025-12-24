using HolyricsObsAuto.Holyrics;
using HolyricsObsAuto.Models;
using OBSWebsocketDotNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
namespace HolyricsObsAuto.Services
{
    public static class State
    {
        public static bool Running = true;
    }
    public static class OBSService
    {
        public static OBSWebsocket obs = new OBSWebsocket();
    }
    class Letras(string nomeCena, int idItem)
    {
        public void Ativar()
        {
            OBSService.obs.SetSceneItemEnabled(nomeCena, idItem, true);
        }
        public void Desativar()
        {
            OBSService.obs.SetSceneItemEnabled(nomeCena, idItem, false);
        }
    }

    public class AutomationService
    {

        private readonly HolyricsClient _holyricsClient;
        public AutomationService()
        {
            _holyricsClient = new HolyricsClient();
        }

        string ultimo = "empty";
        static string cena = $"{Dados.Cena_Letras}";
        static string grphino = $"{Dados.Grupo_Hino}";
        static string grpvers = $"{Dados.Grupo_Versiculo}";
        static int idItem(string nomeCena, string nomeItem)
        {
            return OBSService.obs.GetSceneItemList(nomeCena).Find(item => item.SourceName == nomeItem).ItemId;
        }
        static int idhino = idItem(cena, grphino);
        static int idvers = idItem(cena, grpvers);
        Letras hino = new Letras(cena, idhino);
        Letras vers = new Letras(cena, idvers);
        
        public async Task StartAsync()
        {
            while (State.Running == true)
            {
                await VerificarHolyricsAsync();
                await Task.Delay(200);
            }
        }
        private async Task VerificarHolyricsAsync()
        {
            try
            {
                var estado = await _holyricsClient.GetEstadoAtualAsync();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {estado.map.type}");
                switch (estado.map.type)
                {
                    case "empty":
                        if (ultimo == "empty") return;
                        ultimo = "empty";
                        Console.WriteLine("Nenhum mapa carregado.");
                        hino.Desativar();
                        vers.Desativar();
                        break;
                    case "BIBLE":
                        if (ultimo == "BIBLE") return;
                        ultimo = "BIBLE";
                        Console.WriteLine("Mapa Bíblico carregado.");
                        hino.Desativar();
                        await Task.Delay(200);
                        vers.Ativar();
                        break;
                    case "MUSIC":
                        if (ultimo == "MUSIC") return;
                        ultimo = "MUSIC";
                        Console.WriteLine("Mapa Musical carregado.");
                        vers.Desativar();
                        await Task.Delay(200);
                        hino.Ativar();
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                //ignora
            }
        }

    }
}
