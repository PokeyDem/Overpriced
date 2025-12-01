using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace ManagerScripts
{
    public class RandomEventManager : MonoBehaviour
    {
        private int currentEventChance = 25;
        private bool isEventRunning = false;
        private int isFirstRun = 0;
        private Random rng = new Random();
        [SerializeField] 
        private List<RandomEvent> randomEvents = new List<RandomEvent>();
        private RandomEvent currentEvent = null;
        public UnityEvent<RandomEvent> OnEventRaised;
        public UnityEvent<string> OnEventRaisedDescription;
        public UnityEvent OnEventCleared;

        public void runEvents()
        {
            if (isFirstRun == 0)
            {
                isFirstRun++;
            }
            else
            {
                if (getIsEventRunning())
                {
                    setCurrentEventChance(25);
                    currentEvent = null;
                    setIsEventRunning(false);
                    OnEventCleared?.Invoke();
                    Debug.Log("event none");
                }
                else
                {
                    if (currentEventChance > getRandom().Next(1, 100)) {
                        //activate a random event here
                        currentEvent = randomEvents[getRandom().Next(0,randomEvents.Count)];
                        setIsEventRunning(true);
                        OnEventRaised?.Invoke(currentEvent);
                        OnEventRaisedDescription?.Invoke(currentEvent.eventDescription);
                        Debug.Log("event " + currentEvent.eventDescription);
                    }
                    else IncreaseEventChance();
                }
            }
        }
        
        public void IncreaseEventChance()
        {
            setCurrentEventChance(getCurrentEventChance() + 42);
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