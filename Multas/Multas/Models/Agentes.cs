using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas.Models {
    public class Agentes {

        public Agentes() {
            ListaMultas = new HashSet<Multas>();
        }

        // id, nome, esquadra, foto


        public int ID { get; set; }
        [Required(ErrorMessage = "Por favor, insira o nome do agente.")]
        //validar um nome
        //letras, formando palavras
        //letras isoladas, em alguns casos. ex:e, de, do, dos, etc.
        //espaço em branco entre palavras 
        //uso de simbolos especiais. ex: letras acedelhadas, -, '
        // \d{4}-\{3}\w* - indicação para o código postal
        //[]- identifica conjunto
        //() - agrupamentos, juntam um conjunto de regras
        //+ - quanrificador : 1 ou mais
        //* - quantificador : 1  ou mais
        //? -quantufucador: 0 ou 1
        //()+ o agrupamento ira acontecer um ou mais vezes 
        //()* o agrupamento ira acontecer as vezes que o utilizador desejar
        //{i} quantificador: indica que são exatamente i valores 
        //{i,j} quantificador: indica que são exatamente entre i e j valores
        // | - seprara opções de escolha
        [RegularExpression("([A-ZÁÉÍÓÚa-záéíóúàèìòùãõâêîôûäëïöüçñ]+( |-|')?)+", ErrorMessage = "Só pode escrever letras no Nome. Deve começar por uma maiúcula ")]

        //(((((e|de|da|do|das|dos) )?)|-|')+[A-ZÁÉÍÓÚ][a-záéíóúàèìòùãõâêîôûäëïöüçñ]+){2,3}
        public string Nome { get; set; }
        [Required(ErrorMessage = "Por favor, insira a esquadra onde o agente trabalha.")]
       // [RegularExpression ("Tomar|Ourém|Lisboa|Leiria|Alcanena|Torres Novas")]
        public string Esquadra { get; set; }
        
        public string Fotografia { get; set; }

        //identifica as multas passadas pelo Agente
        //o virtual vai "dizer" para associar as multas ao agente
        public virtual ICollection<Multas> ListaMultas { get; set; }



    }
}