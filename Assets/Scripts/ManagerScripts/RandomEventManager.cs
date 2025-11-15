using System;

namespace ManagerScripts
{
    public class RandomEventManager
    {
        private int currentEventChance = 5;
        private bool isEventRunning = false;
        private Random rng = new Random();
        

        public void Start()
        {
            if (getIsEventRunning())
            {
                setCurrentEventChance(5);
            }
            else
            {
                if (currentEventChance > getRandom().Next(1, 100))
                {
                    //activate a random event here
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