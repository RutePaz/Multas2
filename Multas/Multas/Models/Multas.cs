using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Multas.Models {
   public class Multas {


        // id, data, valor, infracao, FK viatura, FK agente, FK condutor
        public int ID { get; set; }

        [Display(Name = "Infração")]
        public string Infracao { get; set; }

        [Display (Name = "Local da Multa")]
        public string LocalDaMulta { get; set; }

        [Display(Name = "Valor da Multa")]
        public decimal ValorMulta { get; set; }

        [Display(Name = "Data da Multa")]
        public DateTime DataDaMulta { get; set; }

        //Criar chaves forasteiras
        //para a base de dados 
        [ForeignKey("Agente")]
        public int AgenteFK { get; set; }
        //para o C#
        public Agentes Agente { get; set; }

        //FK para o Condutor
        //para a base de dados 
        [ForeignKey("Condutor")]
        public int CondutorFK { get; set; }
        //para o C#
        public virtual Condutores Condutor { get; set; }

        //FK para a Viatura
        //para a base de dados 
        [ForeignKey("Viatura")]
        public int ViaturaFK { get; set; }
        //para o C#
        public virtual Viaturas Viatura{ get; set; }







    }
}