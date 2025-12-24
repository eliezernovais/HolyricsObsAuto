using HolyricsObsAuto.Holyrics;
using HolyricsObsAuto.Models;
using HolyricsObsAuto.Services;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication;
using OBSWebsocketDotNet.Types;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;


class Program
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"MENU\n" +
                $"1 - Definir IP Holyrics =                 {Dados.IPHolyrics}\n" +
                $"2 - Definir IP OBS =                      {Dados.IPOBS}\n" +
                $"3 - Definir Porta OBS =                   {Dados.PortaOBS}\n" +
                $"4 - Definir Senha OBS =                   {Dados.SenhaOBS}\n" +
                $"5 - Definir Nome da Cena das Letras =     {Dados.Cena_Letras}\n" +
                $"6 - Definir Nome do Grupo de Hino =       {Dados.Grupo_Hino}\n" +
                $"7 - Definir Nome do Grupo do Versiculo =  {Dados.Grupo_Versiculo}\n" +
                $"0 - Iniciar Programa\n" +
                $"Enter - Fechar o Programa");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Digite o IP do Holyrics: ");
                    Dados.IPHolyrics = Console.ReadLine();
                    continue;
                case "2":
                    Console.Write("Digite o IP do OBS: ");
                    Dados.IPOBS = Console.ReadLine();
                    continue;
                case "3":
                    Console.Write("Digite a Porta do OBS: ");
                    try
                    {
                        Dados.PortaOBS = int.Parse(Console.ReadLine());

                    }
                    catch
                    {
                        Console.WriteLine("Porta inválida! Pressione Enter para continuar...");
                        Console.ReadLine();
                        continue;
                    }
                    continue;
                case "4":
                    Console.Write("Digite a Senha do OBS: ");
                    Dados.SenhaOBS = Console.ReadLine();
                    continue;
                case "5":
                    Console.Write("Digite o Nome da Cena das Letras: ");
                    Dados.Cena_Letras = Console.ReadLine();
                    continue;
                case "6":
                    Console.Write("Digite o Nome do Grupo de Hino: ");
                    Dados.Grupo_Hino = Console.ReadLine();
                    continue;
                case "7":
                    Console.Write("Digite o Nome do Grupo do Versiculo: ");
                    Dados.Grupo_Versiculo = Console.ReadLine();
                    continue;
                case "0":
                    State.Running = true;
                    break;
                case "":
                    State.Running = false;
                    break;
            }
            if (!State.Running) break;
            var conectadoTcs = new TaskCompletionSource<bool>();
            OBSService.obs.ConnectAsync($"ws://{Dados.IPOBS}:{Dados.PortaOBS}", $"{Dados.SenhaOBS}");
            OBSService.obs.Connected += (s, e) =>
            {
                Console.WriteLine("Conectado ao OBS");
                conectadoTcs.TrySetResult(true);
            };

            OBSService.obs.Disconnected += (s, e) =>
            {
                Console.WriteLine("Desconectado do OBS");
            };

            Console.WriteLine("Conectando...");
            await conectadoTcs.Task;

            List<SceneItemDetails> lista = OBSService.obs.GetSceneItemList($"{Dados.Cena_Letras}");
            for (int i = 0; i < lista.Count; i++)
            {
                Console.WriteLine($"{lista[i].SourceName} - {lista[i].ItemId}");
            }
            var automacao = new AutomationService();
            _ = automacao.StartAsync();

            Console.ReadLine();
            State.Running = false;
            continue;
        }
    }
}