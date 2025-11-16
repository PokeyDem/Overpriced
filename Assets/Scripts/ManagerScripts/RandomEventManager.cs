using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ScriptableObjects;
using UnityEngine;
using Random = System.Random;

namespace ManagerScripts
{
    public class RandomEventManager : MonoBehaviour
    {
        private int currentEventChance = 5;
        private bool isEventRunning = false;
        private Random rng = new Random();
        [SerializeField] 
        private List<RandomEvent> randomEvents = new List<RandomEvent>();
        private RandomEvent currentEvent = null;

        public void runEvents()
        {
            if (getIsEventRunning())
            {
                setCurrentEventChance(5);
                currentEvent = null;
            }
            else
            {
                if (currentEventChance > getRandom().Next(1, 100))
                {
                    //activate a random event here
                    currentEvent = randomEvents[getRandom().Next(0,randomEvents.Count)];
                    setIsEventRunning(true);
                }
                else IncreaseEventChance();
            }
        }
        
        public void IncreaseEventChance()
        {
            setCurrentEventChance(getCurrentEventChance() + 50);
        }

        public int getCurrentEventChance()
        {
            return currentEventChance;
        }

        public bool getIsEventRunning()
        {
            return isEventRunning;
        }

        public Random getRandom()
        {
            return rng;
        }

        public void setCurrentEventChance(int value)
        {
            currentEventChance = value;
        }

        public void setIsEventRunning(bool value)
        {
            isEventRunning = value;
        }
    }
}