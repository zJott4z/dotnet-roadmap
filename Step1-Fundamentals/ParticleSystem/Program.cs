using System;
using System.Collections.Generic;
using System.Threading;

namespace ParticleSystem
{
    public class Particula
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int TempoDeVidaMax { get; private set; }
        public int TempoDeVidaAtual { get; private set; }

        public Particula(int x, int y, int tempoVida)
        {
            X = x; Y = y;
            TempoDeVidaMax = tempoVida;
            TempoDeVidaAtual = tempoVida;
        }

        public void Atualizar()
        {
            Y -= 1;
            TempoDeVidaAtual--;
        }

        public bool EstaViva() => TempoDeVidaAtual > 0;

        public char GetCaractere()
        {
            float opacidade = (float)TempoDeVidaAtual / TempoDeVidaMax;
            if (opacidade > 0.7f) return '@'; // Núcleo forte
            if (opacidade > 0.4f) return '#'; // Médio
            return '*'; // Fraco
        }
    }

    public class EmissorDeFogo
    {
        private List<Particula> _particulas = new List<Particula>();
        private Random _random = new Random();
        private int _larguraEmissor = 9;
        private int _alturaEmissor = 5; // Grid menor
        private int _alturaFogoMax = 3; // Fogo só sobe até 3
        private int _posXBase = 35;
        private int _posYBase = 20;

        private int _timer = 0; // Controla o ciclo do vento
        private int _cicloTotal = 80; // 80 frames = ~4 segundos

        private int CalcularTempoVida(int x, int y)
        {
            int centroX = _larguraEmissor / 2;
            int distanciaX = Math.Abs(x - centroX);
            int intensidadeX = _larguraEmissor - distanciaX;

            int intensidadeY = (_alturaFogoMax - y); // Usa altura do fogo, não do grid

            return Math.Max(1, (intensidadeX + intensidadeY * 2) / 2); // Y pesa mais
        }

        public void Atualizar()
        {
            _timer++;
            if (_timer > _cicloTotal) _timer = 0; // Reseta o ciclo

            float progressoCiclo = (float)_timer / _cicloTotal; // 0.0 a 1.0

            // Só cria fogo se estiver nos primeiros 60% do ciclo
            if (progressoCiclo < 0.6f)
            {
                int alturaAtual = (int)(_alturaFogoMax * (progressoCiclo / 0.6f)); // Sobe gradual

                for (int y = 0; y < alturaAtual; y++)
                {
                    for (int x = 0; x < _larguraEmissor; x++)
                    {
                        // 30% de chance de spawnar pra não ficar sólido
                        if (_random.NextDouble() < 0.3)
                        {
                            int xComTremido = _posXBase + x + _random.Next(-1, 1);
                            int yComTremido = _posYBase - y + _random.Next(-1, 1);
                            int tempoVida = CalcularTempoVida(x, y);
                            _particulas.Add(new Particula(xComTremido, yComTremido, tempoVida));
                        }
                    }
                }
            }

            foreach (var p in _particulas) p.Atualizar();
            _particulas.RemoveAll(p => !p.EstaViva());
        }

        public void Desenhar()
        {
            Console.Clear();
            foreach (var p in _particulas)
            {
                if (p.X > 0 && p.Y > 0) // Segurança pra não quebrar
                {
                    Console.SetCursorPosition(p.X, p.Y);
                    Console.Write(p.GetCaractere());
                }
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            EmissorDeFogo emissor = new EmissorDeFogo();
            Console.CursorVisible = false; // Esconde o cursor pra ficar bonito

            while (true)
            {
                emissor.Atualizar();
                emissor.Desenhar();
                Thread.Sleep(80);
            }
        }
    }
}