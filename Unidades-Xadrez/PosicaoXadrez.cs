using Projeto_Xadrez_Console.Tabuleiro;

namespace Projeto_Xadrez_Console.Unidades_Xadrez
{
    class PosicaoXadrez
    {
        public char coluna { get; set; }
        public int linha { get; set; }

        public PosicaoXadrez(char coluna, int linha)
        {
            this.coluna = coluna;
            this.linha = linha;
        }

        public Posicao toPosicao()
        {
            return new Posicao(8 - linha, coluna - 'A');
        }

        public override string ToString()
        {
            return "" + coluna + linha;
        }
    }
}
