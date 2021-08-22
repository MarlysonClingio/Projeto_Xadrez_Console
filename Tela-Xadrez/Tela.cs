using System;
using System.Collections.Generic;
using Projeto_Xadrez_Console.Tabuleiro;
using Projeto_Xadrez_Console.Unidades_Xadrez;

namespace Projeto_Xadrez_Console
{
    class Tela
    {

        public static void imprimirPartida(PartidaDeXadrez partida)
        {
            imprimirTabuleiro(partida.tab);
            Console.WriteLine();
            imprimirPecasCapturadas(partida);
            Console.WriteLine("\nTurno: " + partida.turno);
            if (!partida.terminada)
            {
                Console.WriteLine("Aguardando Jogada: " + partida.jogadorAtual);
                if (partida.xeque)
                {
                    Console.WriteLine("\nXEQUE!");
                }
            }
            else
            {
                Console.WriteLine("\nXEQUEMATE!");
                Console.WriteLine("\nVENCEDOR: " + partida.jogadorAtual);
            }
        }

        public static void imprimirPecasCapturadas(PartidaDeXadrez partida)
        {
            Console.WriteLine("Peças Capturadas");
            Console.Write("Red: ");
            ConsoleColor auxRed = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            imprimirConjunto(partida.pecasCapturadas(Cor.Red));
            Console.ForegroundColor = auxRed;


            Console.Write("\nYellow: ");
            ConsoleColor auxYellow = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            imprimirConjunto(partida.pecasCapturadas(Cor.Yellow));
            Console.ForegroundColor = auxYellow;

            Console.WriteLine();
        }

        public static void imprimirConjunto(HashSet<Peca> conjunto)
        {
            Console.Write("[");
            foreach (Peca peca in conjunto)
            {
                Console.Write(peca + " ");
            }
            Console.Write("]");
        }


        public static void imprimirTabuleiro(Tabuleiro.Tabuleiro tab)
        {
            for (int i = 0; i < tab.linhas; i++)
            {
                Console.Write(8 - i + "  ");
                for (int j = 0; j < tab.colunas; j++)
                {
                    imprimirPeca(tab.peca(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n   A B C D E F G H");
        }

        public static void imprimirTabuleiro(Tabuleiro.Tabuleiro tab, bool[,] posicoesPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            for (int i = 0; i < tab.linhas; i++)
            {
                Console.Write(8 - i + "  ");
                for (int j = 0; j < tab.colunas; j++)
                {
                    if (posicoesPossiveis[i, j])
                    {
                        Console.BackgroundColor = fundoAlterado;
                    }
                    else
                    {
                        Console.BackgroundColor = fundoOriginal;
                    }
                    imprimirPeca(tab.peca(i, j));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n   A B C D E F G H");
            Console.BackgroundColor = fundoOriginal;
        }

        public static PosicaoXadrez lerPosicaoXadrez()
        {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new PosicaoXadrez(coluna, linha);
        }

        public static void imprimirPeca(Peca peca)
        {

            if (peca == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (peca.cor == Cor.Red)
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }

        }
    }
}
