using FIAP.TechChallenge.ByteMeBurger.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public CategoriaProduto Categoria { get; set; }
        public double Preco { get; set; }
        public string Descricao { get; set; }
        public List<string> Imagens { get; set; } // Lista de URLs das imagens
    }
}
