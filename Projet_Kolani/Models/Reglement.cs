namespace Projet_Kolani.Models
{
    public class Reglement
    {
       
            public int ReglementId { get; set; }
            public DateTime Date { get; set; }
            public decimal Montant { get; set; }
            // D'autres propriétés nécessaires

            public int FactureId { get; set; }
            public Facture? Facture { get; set; }
        

    }
}
