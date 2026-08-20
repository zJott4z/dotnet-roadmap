using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WaveSystem
{

    public class Inimigo
    {
        public string Nome { get; private set; }
        public int Vida { get; private set; }


        public Inimigo(string nome, int vida)
        {
            Nome = nome;
            Vida = vida;
        }

        public void TomarDano(int dano)
        {
            Vida -= dano;
            if(Vida <= 0)
            {
                Console.WriteLine($"{Nome} foi derrotado!");
            }
        }

        public bool EstaVivo()
        {
            return Vida >= 0;
        }
    }


    public class WaveController
    {
        private List<Inimigo> _ondaAtual = new List<Inimigo>();

        public int WaveAtual { get; private set; } = 0;

        private Random _random = new Random();

        public void GerarWave()
        {
            WaveAtual++;
            _ondaAtual.Clear();

            int qtdInimigos = 5 * WaveAtual;
            int vidaInimigo = 10 * WaveAtual;

            for (int i = 0; i < qtdInimigos; i++)
            {
                _ondaAtual.Add(new Inimigo($"Inimigo {i + 1}", vidaInimigo));
            }

            Console.WriteLine($"--- WAVE {WaveAtual} INICIADA! {qtdInimigos} Inimigos apareceram ---");
        }


        public void Atualizar()
        {
            // Simulate the Attack
            if (_ondaAtual.Count > 0)
            {
                int indiceAleatorio = _random.Next(0, _ondaAtual.Count);
                _ondaAtual[indiceAleatorio].TomarDano(15);
            }

            //Clear the Dead Enemies
            _ondaAtual.RemoveAll(inimigos => !inimigos.EstaVivo());

            //If its Over, Generate a new Wave
            if (_ondaAtual.Count == 0)
            {
                GerarWave();
            }

        }

        public void MostrarStatus()
        {
            Console.WriteLine($"Wave: {WaveAtual} | Inimigos Restantes: {_ondaAtual.Count}");

            foreach (var inimigo in _ondaAtual)
            {
                Console.WriteLine($"- {inimigo.Nome} | Vida: {inimigo.Vida}");
            }
        }

    }


    internal class Program
    {
        static void Main(string[] args)
        {

            WaveController controller = new WaveController();
            controller.GerarWave();
            
            while (controller.WaveAtual <= 3)
                {
                    Console.Clear();
                    controller.Atualizar();
                    controller.MostrarStatus();
                    Thread.Sleep(800);
                }
        }
    }
}
