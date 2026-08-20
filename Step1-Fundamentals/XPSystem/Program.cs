using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPSystem
{
    public class Player
    {
        public int Nivel { get; private set; } = 1;
        public int XPAtual { get; private set; } = 0;

        private List<string> _historicoXP = new List<string>();

        public void GanharXP(int quantidade)
        {
            XPAtual += quantidade;
            _historicoXP.Add($"Ganhou {quantidade} de Xp em {DateTime.Now:HH:mm}");

            while (XPAtual >= XPParaProximoNivel())
            {
                XPAtual -= XPParaProximoNivel();
                Nivel++;
                _historicoXP.Add($"UPOU PARA NÍVEL {Nivel} em {DateTime.Now:HH:mm}");
                Console.WriteLine($"UPOU DE NÍVEL! Agora você está no nível {Nivel}.");
            }
        }

        public int XPParaProximoNivel()
        {
            return Nivel * 100;
        }


        public void MostrarStatus()
        {
            Console.WriteLine("=== STATUS ===");
            Console.WriteLine($"Nivel: {Nivel}");
            Console.WriteLine($"XP Atual: {XPAtual}/{XPParaProximoNivel()}");
            Console.WriteLine("------------------------\n");
        }

        public void MostrarHistorico()
        {
            Console.WriteLine("\n=== HISTÓRICO ===");
            if(_historicoXP.Count == 0)
            {
                Console.WriteLine("Nenhuma ação registrada.");
                return;
            }
            foreach(var h in _historicoXP)
            {
                Console.WriteLine(h);
            }
    Console.WriteLine("----------------------- \n");
        }

    }

  
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            string opcao = "";

            while(opcao != "S")
            {
                Console.WriteLine("1 - Ganhar XP");
                Console.WriteLine("2 - Ver Status");
                Console.WriteLine("3 - Ver Historico");
                Console.WriteLine("S - Sair");
                Console.Write("Escolha: ");
                opcao = Console.ReadLine().ToUpper();

                switch (opcao)
                {
                    case "1":
                        Console.Write("Quanto XP ganhou? ");
                        if (int.TryParse(Console.ReadLine(), out int xp))
                            player.GanharXP(xp);
                        else
                            Console.WriteLine("Valor Invalido.");
                        break;
                    case "2":
                        player.MostrarStatus();
                        break;
                    case "3":
                        player.MostrarHistorico();
                        break;
                    case "S":
                        Console.WriteLine("Saindo...");
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }
    }
}
