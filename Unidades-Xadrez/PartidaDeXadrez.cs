using System.Collections.Generic;
using Projeto_Xadrez_Console.Tabuleiro;

namespace Projeto_Xadrez_Console.Unidades_Xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro.Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }
        public Peca vulneravelEnPassant { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro.Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Red;
            terminada = false;
            xeque = false;
            vulneravelEnPassant = null;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);


            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }

            // Jogada Especial: Roque Pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retirarPeca(origemT);
                T.incrementarQteMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            // Jogada Especial: Roque Grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.retirarPeca(origemT);
                T.incrementarQteMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            // Jogada Especial: En Passant
            if (p is Peao)
            {
                if (origem.coluna != destino.coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.cor == Cor.Red)
                    {
                        posP = new Posicao(destino.linha + 1, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.linha - 1, destino.coluna);
                    }
                    pecaCapturada = tab.retirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQteMovimentos();
            if (pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);

            // Jogada Especial: Roque Pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retirarPeca(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }

            // Jogada Especial: Roque Grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.retirarPeca(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }

            // Jogada Especial: En Passant
            if (p is Peao)
            {
                if (origem.coluna != destino.coluna && pecaCapturada == vulneravelEnPassant)
                {
                    Peca peao = tab.retirarPeca(destino);
                    Posicao posP;
                    if (p.cor == Cor.Red)
                    {
                        posP = new Posicao(3, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.coluna);
                    }
                    tab.colocarPeca(peao, posP);
                }
            }
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("\nVoçê não pode se colocar em Xeque ou você já está em Xeque!!");
            }

            Peca p = tab.peca(destino);
            // Jogada Especial: Promoção
            if (p is Peao)
            {
                if ((p.cor == Cor.Red && destino.linha == 0) || (p.cor == Cor.Yellow && destino.linha == 7))
                {
                    p = tab.retirarPeca(destino);
                    pecas.Remove(p);
                    Peca dama = new Dama(tab, p.cor);
                    tab.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }


            if (estaEmXeque(adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }
            if (testeXequeMate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                mudaJogador();
            }

            // Jogada Especial: En Passant
            if (p is Peao && (destino.linha == origem.linha - 2 || destino.linha == origem.linha + 2))
            {
                vulneravelEnPassant = p;
            }
            else
            {
                vulneravelEnPassant = null;
            }
        }

        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if (tab.peca(pos) == null)
            {
                throw new TabuleiroException("\nNão existe peça na posiçao de origem escolhida!");
            }
            if (jogadorAtual != tab.peca(pos).cor)
            {
                throw new TabuleiroException("\nA peça de origem escolhida não é sua!");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("\nNão há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).movimentoPossivel(destino))
            {
                throw new TabuleiroException("\nPosição de Destino Inválida!");
            }
        }


        private void mudaJogador()
        {
            if (jogadorAtual == Cor.Red)
            {
                jogadorAtual = Cor.Yellow;
            }
            else
            {
                jogadorAtual = Cor.Red;
            }
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca peca in capturadas)
            {
                if (peca.cor == cor)
                {
                    aux.Add(peca);
                }
            }
            return aux;
        }


        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca peca in pecas)
            {
                if (peca.cor == cor)
                {
                    aux.Add(peca);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.Red)
            {
                return Cor.Yellow;
            }
            else
            {
                return Cor.Red;
            }
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca peca in pecasEmJogo(cor))
            {
                if (peca is Rei)
                {
                    return peca;
                }
            }
            return null;
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroException("\nNão tem rei da cor " + cor + " no tabuleiro!");
            }

            foreach (Peca peca in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = peca.movimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool testeXequeMate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }

            foreach (Peca peca in pecasEmJogo(cor))
            {
                bool[,] mat = peca.movimentosPossiveis();
                for (int i = 0; i < tab.linhas; i++)
                {
                    for (int j = 0; j < tab.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = peca.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }


        private void colocarPecas()
        {
            colocarNovaPeca('A', 1, new Torre(tab, Cor.Red));
            colocarNovaPeca('B', 1, new Cavalo(tab, Cor.Red));
            colocarNovaPeca('C', 1, new Bispo(tab, Cor.Red));
            colocarNovaPeca('D', 1, new Dama(tab, Cor.Red));
            colocarNovaPeca('E', 1, new Rei(tab, Cor.Red, this));
            colocarNovaPeca('F', 1, new Bispo(tab, Cor.Red));
            colocarNovaPeca('G', 1, new Cavalo(tab, Cor.Red));
            colocarNovaPeca('H', 1, new Torre(tab, Cor.Red));

            colocarNovaPeca('A', 2, new Peao(tab, Cor.Red, this));
            colocarNovaPeca('B', 2, new Peao(tab, Cor.Red, this));
            colocarNovaPeca('C', 2, new Peao(tab, Cor.Red, this));
            colocarNovaPeca('D', 2, new Peao(tab, Cor.Red, this));
            colocarNovaPeca('E', 2, new Peao(tab, Cor.Red, this));
            colocarNovaPeca('F', 2, new Peao(tab, Cor.Red, this));
            colocarNovaPeca('G', 2, new Peao(tab, Cor.Red, this));
            colocarNovaPeca('H', 2, new Peao(tab, Cor.Red, this));



            colocarNovaPeca('A', 8, new Torre(tab, Cor.Yellow));
            colocarNovaPeca('B', 8, new Cavalo(tab, Cor.Yellow));
            colocarNovaPeca('C', 8, new Bispo(tab, Cor.Yellow));
            colocarNovaPeca('D', 8, new Dama(tab, Cor.Yellow));
            colocarNovaPeca('E', 8, new Rei(tab, Cor.Yellow, this));
            colocarNovaPeca('F', 8, new Bispo(tab, Cor.Yellow));
            colocarNovaPeca('G', 8, new Cavalo(tab, Cor.Yellow));
            colocarNovaPeca('H', 8, new Torre(tab, Cor.Yellow));

            colocarNovaPeca('A', 7, new Peao(tab, Cor.Yellow, this));
            colocarNovaPeca('B', 7, new Peao(tab, Cor.Yellow, this));
            colocarNovaPeca('C', 7, new Peao(tab, Cor.Yellow, this));
            colocarNovaPeca('D', 7, new Peao(tab, Cor.Yellow, this));
            colocarNovaPeca('E', 7, new Peao(tab, Cor.Yellow, this));
            colocarNovaPeca('F', 7, new Peao(tab, Cor.Yellow, this));
            colocarNovaPeca('G', 7, new Peao(tab, Cor.Yellow, this));
            colocarNovaPeca('H', 7, new Peao(tab, Cor.Yellow, this));
        }
    }
}