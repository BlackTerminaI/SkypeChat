using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKYPE4COMLib;
using System.Windows.Forms;

namespace SkypeMessage
{
    public enum SkypeStatus : uint
    {
        /// <summary>
        ///  Skype est connecté à l'API, aucun problème à recensser.
        /// </summary>
        Success = 0,
        /// <summary>
        /// Skype demande l'accès à l'API, une action manuelle de l'utilisateur est alors demandée. 
        /// Le client n'est alors pas connecté, pour vérifier : <see cref="SkypeStatus.Success"/>
        /// </summary>
        PendingAuthorizaion = 1,
        /// <summary>
        /// L'utilisateur a refusé l'accès à l'API.
        /// </summary>
        Refused = 2,
        /// <summary>
        /// L'accès à l'API n'est pas disponible, ce qui peut être dû au fait que l'utilisateur n'est pas connecté. 
        /// Le client n'est alors pas connecté à l'API, pour vérifier : <see cref="SkypeStatus.Available"/>
        /// </summary>
        NotAvailable = 3,
        /// <summary>
        /// Quand l'API est disponible, SkypeMessage affiche ce message à tous : <see cref="SkypeStatus.Available"/> 
        /// </summary>
        Available = 0x8001,
        /// <summary>
        /// État Inconnu.
        /// </summary>
        Unknown = 99
    }
    class Program
    {
        static void write(string e)
        {
            Console.WriteLine(e);
        }
        static void Main(string[] args)
        {
            Boolean confirm = true;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "SkypeChat";
            Skype skype = new Skype();
            if (!skype.Client.IsRunning)
            {
                skype.Client.Start(true, true);
            }
            // Utilisation de la version 7 de Skype
            skype.Attach(7, false);
            write("Conversation ou discussion privée? [C/D]");
            string choix = Console.ReadLine();
            choix = choix.ToUpper();
            switch (choix)
            {
                case "D":
                    DiscuPrivée:
                    Console.Clear();
                    write("Veuillez entrer un pseudonyme: ");
            string username = Console.ReadLine();
            foreach (User user in skype.SearchForUsers(username))
            {
                write(user.FullName);
            }
            Console.Clear();
            write("La conversation avec votre contact a été lancé \n chaque message sera suivi sur Skype d'un \" - Envoyé avec SkypeChat \". \n Pour voir les commandes disponibles, tapez juste \"#menu\".");
        Chat:
            string message = Console.ReadLine();

            if (confirm == true)
            {
                write("Veuillez appuyer sur Entrée pour confirmer l'envoi de la commande ou du message.");
                Console.ReadKey();
            }
            write("[POUR] " + username + " : " + message);
                    switch (message)
                    {
                        default:
                            // MESSAGE
                            if (message.Contains("#"))
                            {

                            }
                            else
                            {
                                write("[ENVOYÉ] " + username + " : " + message);
                            }

                        case "#menu":
                            // COMMANDES
                            write("#clear        : Nettoyer le CMD.");
                            write("#menu         : Affiche ce menu.");
                            write("#leave        : Fermer la conversation.");
                            write("#exit         : Fermer le logiciel.");
                            write("#call         : Appeler l'utilisateur cible.");
                            write("#confirm      : Désactive/Active la vérification d'envoi de message. (Actuel : " + confirm + ")");
                            write("#conversation : Créer une conversation via SkypeChat avec l'utilisateur cible.");
                            write("#sendfile     : Envoyer un fichier dans la discussion");
                            MessageBox.Show("La commande #confirm ne fonctionne pas pour le moment, mais elle devrait être mis à jour pour la 2.03", "Error");

                            goto Chat;
                        case "#clear":

                            Console.Clear();
                            goto Chat;

                        case "#leave":
                            Console.Clear();
                            goto DiscuPrivée;

                        case "":
                            write("Vous ne pouvez pas envoyer un message vide.");
                            goto Chat;

                        case "#exit":
                            Environment.Exit(1);
                            break;

                        case "#call":
                            skype.PlaceCall(username);
                            write("Veuillez appuyer sur Entrée à la fin de votre appel.");
                            Console.ReadKey();
                            write("Retour au mode texte en cours..");
                            goto Chat;

                        case "#conversation":
                            skype.CreateGroup("Conversation créer par SkypeChat");
                            goto Chat;

                        case "#sendfile":
                            skype.Client.OpenFileTransferDialog(username, "C:\\");
                            goto Chat;

                        case "#confirm":
                            if (confirm == true)
                            {
                                confirm = false;
                                write("Demande de confirmation: Désactivée !");
                            }
                            goto Chat;
                            if (confirm == false)
                            {
                                confirm = true;
                                write("Demande de confirmation: Activée !");

                            }
                            goto Chat;
                    }
                    break;

                case "C":
                    MessageBox.Show("Le mode conversation n'est pas encore prêt, passage automatique en mode discussion privée","Erreur");
                    goto DiscuPrivée;
            }

        }
    }
}
