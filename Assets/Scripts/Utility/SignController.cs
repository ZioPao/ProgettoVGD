using System;
using Player;
using UnityEngine;

namespace Utility
{
    public class SignController : MonoBehaviour, IInteractableMidGame
    {
        [SerializeField] private int signID = 0;


        private string signText = "I Am Error";
        private int currentSignID;

        public string GetSignText()
        {
            switch (currentSignID)
            {
                case -1:
                    signText = "Qualcosa nel codice ha deciso di utilizzare l'id del sign parent." +
                               Environment.NewLine +
                               "Cio', in base ai test eseguiti, non dovrebbe essere possibile, di conseguenza visualizzare questo messaggio comporta un problema."
                               + Environment.NewLine +
                               "Premi Q e dimentica cio' che hai visto";
                    break;

                case 0:
                    signText = "Su questo cartello saranno presenti le regole del gioco." + Environment.NewLine +
                               "Probabilmente ti verra' detto di premere dei tasti, oppure di raccogliere oggetti di vario tipo, ma qualsiasi cosa accada, questo e' e restera' per sempre un cartello." +
                               Environment.NewLine +
                               "Non lasciare che esso guidi la tua vita." + Environment.NewLine + Environment.NewLine +
                               "Corri contro le pareti della mappa e trova tutte le collisioni rotte che puoi." +
                               Environment.NewLine + Environment.NewLine + Environment.NewLine +
                               "Premi Q per tornare al gioco.";
                    break;

                case 1:
                    signText = "Questo cartello e' una prova";
                    break;
                case 2:
                    signText = "Hai Vinto!" +
                               Environment.NewLine + Environment.NewLine +
                               "Premi Q per raggiungere la schermata finale!";
                    break;

                case 3:
                    signText = "Benvenuto!" +
                               Environment.NewLine +
                               "Il gioco e' composto da 3 livelli distinti." +
                               Environment.NewLine +
                               "Per superare un livello, raggiungi l'arena del boss e sconfiggilo, poi dirigiti verso l'uscita." +
                               Environment.NewLine +
                               "Lungo la tua strada troverai nemici da sconfiggere e utili consumabili che ti saranno d'aiuto." +
                               Environment.NewLine +
                               "Sfrutta al meglio tutto cio' che riesci a trovare! Buona Fortuna." +
                               Environment.NewLine + Environment.NewLine +
                               "Premi Q per tornare al gioco.";
                    break;

                case 4:
                    signText = "Comandi di Gioco:" +
                               Environment.NewLine +
                               "W - A - S - D: Controlla il Movimento del giocatore." +
                               Environment.NewLine +
                               "Spazio: Salta." +
                               Environment.NewLine +
                               "Shift: Scatta nella direzione in cui ti stai muovendo." +
                               Environment.NewLine +
                               "1 - 2 - 3: Scegli l'arma corrente." +
                               Environment.NewLine +
                               "Tasto sinistro del Mouse: Attacca con l'arma corrente." +
                               Environment.NewLine +
                               "R: Ricarica l'arma (Il migliore amico di ogni giocatore)." +
                               Environment.NewLine +
                               "Esc: Accedi al menu di pausa." +
                               Environment.NewLine +
                               "F5: Salva la partita." +
                               Environment.NewLine +
                               "F6: Carica la partita." +
                               Environment.NewLine + Environment.NewLine +
                               "Note:" +
                               Environment.NewLine +
                               "(Non tutte le armi sono disponibili dall'inizio della partita)" +
                               Environment.NewLine +
                               "(Non e' possibile salvare durante le bossfight)" +
                               Environment.NewLine + Environment.NewLine +
                               "Premi Q per tornare al gioco.";
                    break;

                case 5:
                    signText = "Oggetti Disponibili:" +
                               Environment.NewLine +
                               "Cura: Ripristina parte della vita." +
                               Environment.NewLine +
                               "Potenziamento: Incrementa permanentemente il valore massimo di una statistica." +
                               Environment.NewLine +
                               "Cassa di Munizioni: Aggiunge munizioni alle scorte." +
                               Environment.NewLine +
                               "Chiave: Permette di aprire una porta bloccata." +
                               Environment.NewLine + Environment.NewLine +
                               "Premi Q per tornare al gioco.";
                    break;

                case 6:
                    signText = "Tutti hanno messo in dubbio la mia sanita' mentale quando ho nascosto un cartello sott'acqua." +
                               Environment.NewLine +
                               "Oooooh, sicuramente ora se ne staranno pentendo." +
                               Environment.NewLine +
                               "Quelle dannate bestie nascondono un segreto, e questo e' l'unico luogo che non controllerebbero mai." +
                               Environment.NewLine +
                               "Non e' ancora troppo tardi, possono ancora essere fermate, a patto che la creatura nascosta nei paraggi venga sconfitta." +
                               Environment.NewLine +
                               "Dovrebbe trovarsi in una caverna, ma purtroppo non so altro." +
                               Environment.NewLine + Environment.NewLine +
                               "Premi Q per tornare al gioco.";
                    break;

                case 7:
                    signText = "Non disturbare il guardiano." +
                               Environment.NewLine + Environment.NewLine +
                               "Premi Q per tornare al gioco.";
                    break;

                case 8:
                    signText = "La tentazione e' stata troppo forte, non abbiamo resistito." +
                               Environment.NewLine +
                               "Il Signore abbia pieta' per le povere anime che decideranno di abbassare quella leva." +
                               Environment.NewLine + Environment.NewLine +
                               "Premi Q per tornare al gioco.";
                    break;

                default:
                    signText = "418 Sono una teiera";
                    break;
            }

            return signText;
        }

        public int GetSignID()
        {
            return signID;
        }

        public int GetCurrentSignID()
        {
            return currentSignID;
        }

        public void SetCurrentSignID(int value)
        {
            currentSignID = value;
        }

        public void InteractableBehaviour()
        {
            //Cerca il sign controller

            SignController signController;
            if (!Values.GetCurrentSignController())
            {
                signController = GetComponentInParent<SignController>();
                Values.SetCurrentSignController(signController);
            }
            else
            {
                signController = Values.GetCurrentSignController();
            }
            
            signController.SetCurrentSignID(GetSignID());
            Values.SetIsReadingSign(true);
            Values.SetCanPause(false);
            Values.SetIsFrozen(true);
        }

        public void ForceActivation()
        {
            return;
        }

        public bool GetIsEnabled()
        {
            return enabled;
        }
    }
}