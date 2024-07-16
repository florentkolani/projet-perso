namespace Projet_Kolani.Models
{
    public class Engin
    {
        
            public int EnginId { get; set; }
            public string Immatriculation { get; set; }
            public string Categorie { get; set; } // Peut être "Voiture" ou "Moto"
            public decimal CotationAssurance { get; set; }
            public decimal MajorationEconomat { get; set; }

            public int ProprietaireId { get; set; }
            public Proprietaire? Proprietaire { get; set; }
        

    }
}
