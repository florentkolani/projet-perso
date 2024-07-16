namespace Projet_Kolani.Models
{
    public class Facture
    {
     
            public int FactureId { get; set; }
            public DateTime Date { get; set; }
            public decimal MontantTotal { get; set; }
            // D'autres propriétés nécessaires

            public int ProprietaireId { get; set; }
            public Proprietaire? Proprietaire { get; set; }
        

    }
}
