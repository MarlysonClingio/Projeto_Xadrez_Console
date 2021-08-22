using System;
using Projeto_Xadrez_Console.Tabuleiro;
using Projeto_Xadrez_Console.Unidades_Xadrez;

namespace Projeto_Xadrez_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PartidaDeXadrez partida = new PartidaDeXadrez();

                while (!partida.terminada)
                {
                    try
                    {
                        Console.Clear();
                        Tela.imprimirPartida(partida);

                        Console.WriteLine("\n-------------------------- Faça sua Jogada --------------------------");
                        Console.Write("\nEscolha a Peça: ");
                        Posicao origem = Tela.lerPosicaoXadrez().toPosicao();
                        partida.validarPosicaoDeOrigem(origem);

                        bool[,] posicoesPossiveis = partida.tab.peca(origem).movimentosPossiveis();

                        Console.Clear();
                        Tela.imprimirTabuleiro(partida.tab, posicoesPossiveis);
                        Console.WriteLine();
                        Tela.imprimirPecasCapturadas(partida);
                        Console.WriteLine("\nTurno: " + partida.turno);
                        Console.WriteLine("Aguardando Jogada: " + partida.jogadorAtual);

                        Console.WriteLine("\n-------------------------- Continue sua Jogada --------------------------");
                        Console.Write("\nEscolha a posição: ");
                        Posicao destino = Tela.lerPosicaoXadrez().toPosicao();
                        partida.validarPosicaoDeDestino(origem, destino);

                        partida.realizaJogada(origem, destino);
                    }
                    catch (TabuleiroException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("\nAperte 'Enter' para retornar...");
                        Console.ReadKey();
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Console.WriteLine("\nIndex error: " + e.Message);
                        Console.WriteLine("\nAperte 'Enter' para retornar...");
                        Console.ReadKey();
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("\nFormat error: " + e.Message);
                        Console.WriteLine("\nAperte 'Enter' para retornar...");
                        Console.ReadKey();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\nUnexpected error: " + e.Message);
                        Console.WriteLine("\nAperte 'Enter' para retornar...");
                        Console.ReadKey();
                    }
                }
                Console.Clear();
                Tela.imprimirPartida(partida);
            }
            catch (TabuleiroException e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nUnexpected error: " + e.Message);
            }
        }
    }
}
