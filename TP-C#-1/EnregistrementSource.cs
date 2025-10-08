// 📁 DataModel.cs

namespace music_tp 
{
    public class EnregistrementSource
    {
        // Propriétés (champs) de l'enregistrement source
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
        public double Montant { get; set; }
        public bool EstActif { get; set; }

        // Utile pour l'affichage des résultats (Étape 3)
        public override string ToString()
        {
            return $"[ID: {Id}] {Nom} ({Email}) - Montant: {Montant:C} - Actif: {EstActif}";
        }
    }
}