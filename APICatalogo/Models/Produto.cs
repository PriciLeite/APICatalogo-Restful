using APICatalogo.Models;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using APICatalogo.Validations;

namespace APICatalogo.Model;

public class Produto 
{
    [Key]
    public int ProdutoId { get; set; }

    [Required]
    [StringLength(80)]
    [PrimeiraLetraMaiuscula]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string?  Descricao { get; set; }

    [Required]
    [Column(TypeName="decimal(10,2)")]
    [Range(1,1000, ErrorMessage = "O preço válido entre {1} e {2}")]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }


    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }    
    public int CategoriaId { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }

}
