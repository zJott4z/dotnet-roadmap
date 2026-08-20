using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem
{
    public class Inventario
    {
        private List<Item> _itens = new List<Item>(); 
        private int _capacidadeMaxima = 10;

        public void AdicionarItem(Item novoItem)
        {
            // 1. Verifica se já existe esse item na lista
            Item itemExistente = _itens.FirstOrDefault(item => item.Nome == novoItem.Nome);

            if (itemExistente != null)
            {
                // 2. Se existe, só soma a quantidade. Não ocupa slot novo
                itemExistente.AdicionarQuantidade(novoItem.Quantidade);
                Console.WriteLine($"Adicionado +{novoItem.Quantidade} {novoItem.Nome}. Total: {itemExistente.Quantidade}");
            }
            else
            {
                // 3. Se NÃO existe, aí sim verifica capacidade e adiciona
                if (_itens.Count >= _capacidadeMaxima)
                {
                    Console.WriteLine("Inventário Cheio!");
                    return;
                }
                _itens.Add(novoItem);
                Console.WriteLine($"Item {novoItem.Nome} Adicionado ao Inventário.");
            }
        }

        public void RemoverItem(string nomeItem)
        {
            Item itemParaRemover = _itens.FirstOrDefault(item => item.Nome == nomeItem);

            if (itemParaRemover != null)
            {
                _itens.Remove(itemParaRemover);
                Console.WriteLine($"Item {nomeItem} Removido do Inventario");
            }
            else
            {
                Console.WriteLine("Item não encontrado no Inventário.");
            }
        }

        public void UsarItem(string nomeItem)
        {
            Item itemParaUsar = _itens.FirstOrDefault(item => item.Nome == nomeItem);

            if (itemParaUsar != null)
            {
                itemParaUsar.Usar();
                Console.WriteLine($"Usou 1 {nomeItem}. Restam: {itemParaUsar.Quantidade}");

                if (itemParaUsar.Quantidade <= 0)
                {
                    _itens.Remove(itemParaUsar);
                    Console.WriteLine($"Item {nomeItem} esgotado e removido do inventário.");
                }
            }
            else
            {
                Console.WriteLine("Item não encontrado no inventário.");
            }
        }

        public void MostrarInventario()
        {
            Console.WriteLine("---------- INVENTARIO ----------");

            if (_itens.Count == 0)
            {
                Console.WriteLine("Inventário Vazio!");
                return;
            }

            foreach (Item item in _itens)
            {
                Console.WriteLine($"Item: {item.Nome} - Quantidade: {item.Quantidade}.");
            }
            Console.WriteLine($"Slots usados: {_itens.Count}/{_capacidadeMaxima}");
        }
    }

    public class Item
    {
        public string Nome { get; private set; }
        public int Quantidade { get; private set; }

        public Item(string nome, int quantidade)
        {
            Nome = nome;
            Quantidade = quantidade;
        }

        public void Usar()
        {
            if (Quantidade > 0)
            {
                Quantidade--;
            }
        }

        public void AdicionarQuantidade(int qtd)
        {
            Quantidade += qtd;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Inventario meuInventario = new Inventario();

            Item espada = new Item("Espada de Ferro", 1);
            Item pocao = new Item("Poção de Vida", 5);

            meuInventario.AdicionarItem(espada);
            meuInventario.AdicionarItem(pocao);

            // TESTE DO EMPILHAMENTO
            Item maisPocao = new Item("Poção de Vida", 3);
            meuInventario.AdicionarItem(maisPocao); // Deveria somar pra 8

            meuInventario.MostrarInventario();

            // TESTE DO USAR
            meuInventario.UsarItem("Poção de Vida");
            meuInventario.UsarItem("Poção de Vida");

            meuInventario.MostrarInventario();
        }
    }
}
