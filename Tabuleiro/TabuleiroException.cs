using System;

namespace Projeto_Xadrez_Console.Tabuleiro
{
    class TabuleiroException : Exception
    {
        public TabuleiroException(string msg) : base(msg) { }
    }
}
