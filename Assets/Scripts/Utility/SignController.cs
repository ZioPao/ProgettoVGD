using System;
using Player;
using UnityEngine;

namespace Utility
{
    public class SignController : MonoBehaviour
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
        
    }
}
