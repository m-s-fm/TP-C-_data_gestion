using System;
using System.Collections.Generic;
using System.Linq;


namespace music_tp 
{
    public class Program

    // le nom du fichier JSON doit etre donnees_source.json 
    {
        public static void Main(string[] args)
        {
            var gestionnaire = new GestionnaireConversion();
            List<EnregistrementSource> resultatsActuels = new List<EnregistrementSource>();

            Console.WriteLine("==========================================");
            Console.WriteLine("LOGICIEL DE CONVERSION ET TRAITEMENT DE DONNÉES");
            Console.WriteLine("==========================================");

            Console.WriteLine("\n[1. CHARGEMENT DE LA SOURCE]");
            Console.Write("Entrez le chemin vers votre fichier de données (ex: donnees_source.json) : ");

            // Capture le chemin saisi par l'utilisateur
            string chemin = Console.ReadLine();

            if (!gestionnaire.ChargerDonnees(chemin))
            {
                // Si le chargement échoue (fichier non trouvé ou erreur JSON), on arrête.
                Console.WriteLine("\n Arrêt du programme en raison d'une erreur de chargement des données.");
                Console.ReadKey();
                return;
            }

            // Récupère la liste chargée pour les opérations suivantes
            resultatsActuels = gestionnaire.ObtenirToutesLesDonnees();


            Console.WriteLine("\n[2. TRI DES RÉSULTATS]");
            Console.WriteLine("Comment souhaitez-vous trier les résultats ?");
            Console.WriteLine("1 : Par Montant (Moins Élevé -> Plus Élevé)");
            Console.WriteLine("2 : Par Montant (Plus Élevé -> Moins Élevé)");
            Console.WriteLine("3 : Par Nom (Ordre Alphabétique A-Z)");

            Console.Write("Votre choix (1, 2 ou 3) : ");
            string choixTri = Console.ReadLine();

            Console.WriteLine("\n--- AFFICHAGE APRÈS TRI ---");

            if (choixTri == "1")
            {
                Console.WriteLine("➡️ Tri par Montant Croissant sélectionné.");
                resultatsActuels = resultatsActuels.OrderBy(e => e.Montant).ToList();
            }
            else if (choixTri == "2")
            {
                Console.WriteLine("➡️ Tri par Montant Décroissant sélectionné.");
                resultatsActuels = gestionnaire.TrierParMontant(resultatsActuels);
            }
            else if (choixTri == "3")
            {
                Console.WriteLine("➡️ Tri par Nom (Alphabétique) sélectionné.");
                resultatsActuels = resultatsActuels.OrderBy(e => e.Nom).ToList();
            }
            else
            {
                Console.WriteLine(" Choix de tri non valide. Affichage de la liste non triée.");
            }

            gestionnaire.AfficherResultats(resultatsActuels);

            // RECHERCHE / FILTRAGE (Interaction utilisateur)

            Console.WriteLine("\n[3. RECHERCHE / FILTRAGE]");
            Console.Write("Entrez un terme à rechercher dans le nom (laissez vide pour ignorer) : ");
            string termeRecherche = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(termeRecherche))
            {
                // La recherche s'applique aux résultats triés
                resultatsActuels = gestionnaire.RechercherParNom(termeRecherche);
                Console.WriteLine("\n--- AFFICHAGE APRÈS RECHERCHE ---");
                gestionnaire.AfficherResultats(resultatsActuels);
            }
            else
            {
                Console.WriteLine(" Recherche ignorée. Les résultats triés sont conservés.");
            }

            // EXPORTATION

            Console.WriteLine("\n[4. EXPORTATION DES DONNÉES]");
            Console.WriteLine("Les résultats finaux (triés et filtrés) seront exportés au format CSV (champs sélectifs) et XML.");

            //Simulation dans la console
            var champsChoisis = new Dictionary<string, Func<EnregistrementSource, object>>
            {
                { "ID_Client", e => e.Id },
                { "Nom_Client", e => e.Nom.ToUpper() },
                { "Montant_Transaction", e => e.Montant },
                { "Statut_Actif", e => e.EstActif ? "OUI" : "NON" }
            };

            gestionnaire.ExporterResultats(resultatsActuels, champsChoisis);

            // Exportation XML 
            string cheminSortieXML = "export_donnees_final.xml";
            gestionnaire.ExporterResultatsXML(resultatsActuels, cheminSortieXML);

            Console.WriteLine("\n==========================================");
            Console.WriteLine("Traitement terminé.");
            Console.ReadKey();
        }
    }
}
