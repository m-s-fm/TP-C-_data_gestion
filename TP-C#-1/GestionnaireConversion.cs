using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace music_tp 
{
    public class GestionnaireConversion
    {
        private List<EnregistrementSource> DonneesSource { get; set; } = new List<EnregistrementSource>();

        // Chargement des données (Source)

        public bool ChargerDonnees(string cheminFichier)
        {
            DonneesSource.Clear();

            if (string.IsNullOrWhiteSpace(cheminFichier) || !File.Exists(cheminFichier))
            {
                Console.WriteLine($" Fichier non trouvé au chemin: '{cheminFichier}'");
                Console.WriteLine("Le traitement ne peut pas continuer sans données valides.");
                return false;
            }

            try
            {
                string jsonString = File.ReadAllText(cheminFichier);
                var donneesChargees = JsonSerializer.Deserialize<List<EnregistrementSource>>(jsonString);

                if (donneesChargees != null && donneesChargees.Any())
                {
                    DonneesSource = donneesChargees;
                    Console.WriteLine($"\n {DonneesSource.Count} enregistrements chargés à partir du fichier {Path.GetFileName(cheminFichier)}.");
                    return true;
                }
                else
                {
                    Console.WriteLine(" Fichier chargé mais ne contient aucune donnée valide ou l'objet JSON est vide.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Erreur lors de la lecture ou de la désérialisation du JSON : {ex.Message}");
                return false;
            }
        }

        public List<EnregistrementSource> ObtenirToutesLesDonnees()
        {
            return DonneesSource;
        }

        // ----------------------------------------------------
        // Recherche et Tri 
        // ----------------------------------------------------

        public List<EnregistrementSource> RechercherParNom(string termeRecherche)
        {
            var resultats = DonneesSource
                .Where(e => e.Nom.Contains(termeRecherche, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Console.WriteLine($"\n Recherche pour '{termeRecherche}' terminée. {resultats.Count} résultats trouvés.");
            return resultats;
        }

        public List<EnregistrementSource> TrierParMontant(List<EnregistrementSource> listeATrier)
        {
            var resultatsTries = listeATrier
                .OrderByDescending(e => e.Montant)
                .ToList();

            Console.WriteLine("↔️ Tri effectué par Montant (Décroissant).");
            return resultatsTries;
        }

        // ----------------------------------------------------
        // Afficher les résultats 
        // ----------------------------------------------------

        public void AfficherResultats(List<EnregistrementSource> resultats)
        {
            Console.WriteLine("\n--- AFFICHAGE DES RÉSULTATS (Total: " + resultats.Count + ") ---");
            if (resultats.Any())
            {
                foreach (var enregistrement in resultats)
                {
                    Console.WriteLine(enregistrement);
                }
            }
            else
            {
                Console.WriteLine("--- Aucun enregistrement à afficher. ---");
            }
            Console.WriteLine("--------------------------------------");
        }

        // Exportation 
        public void ExporterResultats(
            List<EnregistrementSource> resultats,
            Dictionary<string, Func<EnregistrementSource, object>> champsAExporter)
        {
            if (resultats == null || !resultats.Any())
            {
                Console.WriteLine(" Impossible d'exporter en CSV : la liste de résultats est vide.");
                return;
            }

            string enTete = string.Join(";", champsAExporter.Keys);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(enTete);

            foreach (var enregistrement in resultats)
            {
                var valeurs = champsAExporter.Values
                    .Select(extracteur => extracteur(enregistrement)?.ToString() ?? string.Empty)
                    .ToList();

                sb.AppendLine(string.Join(";", valeurs));
            }

            Console.WriteLine("\n--- DÉBUT EXPORT CSV SIMULÉ ---");
            Console.Write(sb.ToString());
            Console.WriteLine("--- FIN EXPORT CSV SIMULÉ ---");
        }

        // Exportation XML
        public void ExporterResultatsXML(List<EnregistrementSource> resultats, string cheminSortie)
        {
            if (resultats == null || !resultats.Any())
            {
                Console.WriteLine(" Impossible d'exporter en XML : la liste de résultats est vide.");
                return;
            }

            try
            {
                // Utilise XmlSerializer pour convertir la List<EnregistrementSource> en XML
                XmlSerializer serializer = new XmlSerializer(typeof(List<EnregistrementSource>));

                // Écrit le résultat dans le fichier spécifié
                using (TextWriter writer = new StreamWriter(cheminSortie))
                {
                    serializer.Serialize(writer, resultats);
                }

                Console.WriteLine($"\n Export XML réussi. Fichier généré : {cheminSortie}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Erreur lors de l'export XML : {ex.Message}");
            }
        }
    }
}
