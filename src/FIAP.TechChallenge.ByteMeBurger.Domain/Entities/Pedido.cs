using FIAP.TechChallenge.ByteMeBurger.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Status { get; set; }
        public Cliente Cliente { get; set; }
        public double PrecoTotal { get; set; }
        public double ValorPago { get; set; }
        public DateTime DataPagamento { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public List<ItemPedido> Produtos { get; set; }
    }
}
