using System;
using Player;
using UnityEngine;

namespace Utility
{
    public class SignController : MonoBehaviour
    {

        [SerializeField] private int signID = 0;

        private int currentSignID;

        public string GetSignText()
        {

            string signText = "I Am Error";

            switch (currentSignID)
            {
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
                    signText = "Questo cartello è una prova";
                    break;

                default:
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
