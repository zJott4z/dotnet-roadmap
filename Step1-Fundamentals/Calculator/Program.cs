using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Entrada
{ 
public double N1 { get; }
public double N2 { get; }

    public Entrada(double n1, double n2)
    {
        N1 = n1;
        N2 = n2;
    }
}

public record RegistroCalculo(
    double N1, 
    double N2, 
    string Operacao, 
    double Resultado, 
    DateTime Data
    );

public class Historico
{
    private readonly List<RegistroCalculo> _registros = new();
    public void Adicionar(double n1,double n2, string op, double res)
    {
        var registro = new RegistroCalculo(n1, n2, op, res, DateTime.Now);
        _registros.Add(registro);
    }

    public void Mostrar()
    {
        Console.WriteLine("\n----------- HISTÓRICO -----------");
        if (_registros.Count == 0)
        {
            Console.WriteLine("Nenhum Calculo foi Feito Ainda!");
            return;
        }

        foreach (var r in _registros)
        {
            Console.WriteLine($"{r.Data:HH:mm:ss} | {r.N1} {r.Operacao} {r.N2} = {r.Resultado}");
        }
        Console.WriteLine("----------------------------------\n");
    }

}

public class Calculadora // <- 2. Padrão estável com fluxo interno
{
    public double Somar(Entrada entrada) => entrada.N1 + entrada.N2;
    public double Subtrair(Entrada entrada) => entrada.N1 - entrada.N2;
    public double Multiplicar(Entrada entrada) => entrada.N1 * entrada.N2;

    public double Dividir(Entrada entrada)
    {
        if (entrada.N2 == 0)
        {
            Console.WriteLine("Não é possível divisão por zero");
            return 0;
        }
        return entrada.N1 / entrada.N2;
    }
}

public class Input
{
    public Entrada _entradaInput;
    private readonly Calculadora _calculadora = new();
    private readonly Historico _historico = new();

    public void RodarCalculadora()
    {
        string resposta = "S";

        while (resposta.ToUpper() == "S" || resposta.ToUpper() == "H")
        {
            if (resposta.ToUpper() == "H")
            {
                _historico.Mostrar();
            }
            else
            {
                LerEntrada();
                string operacao = LerOperacaoEscolhida();
                double resultado = Calcular(operacao);

                Console.WriteLine($"Result: {resultado}");
                _historico.Adicionar(_entradaInput.N1, _entradaInput.N2, operacao, resultado);

                Console.WriteLine("-------------------");
            }
            Console.WriteLine("Calcular Novamente? S | Ver Historico? H | Sair? N");
            resposta = Console.ReadLine();
        }

        Console.WriteLine("Thank you for using the Calculator!");
    }

    public void LerEntrada() // <- 1. Entrada de variação
    {
        Console.WriteLine("-------- Calculator --------");

        double n1;
        Console.WriteLine("Type the First Number: ");
        while (!double.TryParse(Console.ReadLine(), out n1))
        {
            Console.WriteLine("Invalid Value. Type the Number Again:");
        }

        double n2;
        Console.WriteLine("Type the Second Number: ");
        while (!double.TryParse(Console.ReadLine(), out n2))
        {
            Console.WriteLine("Invalid Value. Type the Number Again:");
        }

        _entradaInput = new Entrada(n1, n2); // <- 2. Padrão estável guardado
    }

    private string LerOperacaoEscolhida()
    {
        Console.WriteLine("Choose the Desired Operation: +, -, *, /");
        return Console.ReadLine();
    }

    private double Calcular(string operacao)
    {
        return operacao switch
        {
            "+" => _calculadora.Somar(_entradaInput),
            "-" => _calculadora.Subtrair(_entradaInput),
            "*" => _calculadora.Multiplicar(_entradaInput),
            "/" => _calculadora.Dividir(_entradaInput),
            _ => 0
        };
    }

    public static void Main(string[] args)
    {
        var app = new Input();
        app.RodarCalculadora();
    }
}

















