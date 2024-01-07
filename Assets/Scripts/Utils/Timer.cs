using UnityEngine;

namespace Utils
{
    public class Timer
    {
        /// <summary>
        /// The clock time when the timer started
        /// </summary>
        private float startTicks;

        /// <summary>
        /// The ticks stored when the timer was paused
        /// </summary>
        private float pausedTicks;

        //The timer status
        private bool paused;
        private bool started;

        public Timer()
        {

            startTicks = 0;
            pausedTicks = 0;
            paused = false;
            started = false;
        }

        public void Start()
        {
            //Start the timer
            started = true;

            //Unpause the timer
            paused = false;

            //Get the current clock time
            startTicks = TimeStamp();
            pausedTicks = 0;
        }

        public void Stop()
        {
            //Stop the timer
            started = false;

            //Unpause the timer
            paused = false;

            //Clear tick variables
            startTicks = 0;
            pausedTicks = 0;
        }

        public void Pause()
        {
            //If the timer is running and isn't already paused
            if (started && !paused)
            {
                //Pause the timer
                paused = true;

                //Calculate the paused ticks
                pausedTicks = TimeStamp() - startTicks;
                startTicks = 0;
            }
        }

        public void Unpause()
        {
            //If the timer is running and paused
            if (started && paused)
            {
                //Unpause the timer
                paused = false;

                //Reset the starting ticks
                startTicks = TimeStamp() - pausedTicks;

                //Reset the paused ticks
                pausedTicks = 0;
            }
        }

        public float GetTicks()
        {
            //The actual timer time
            float time = 0;

            //If the timer is running
            if (started)
            {
                //If the timer is paused
                if (paused)
                {
                    //Return the number of ticks when the timer was paused
                    time = pausedTicks;
                }
                else
                {
                    //Return the current time minus the start time
                    time = TimeStamp() - startTicks;

                }
            }

            return time;
        }

        public bool IsStarted()
        {
            //Timer is running and paused or unpaused
            return started;
        }

        public bool IsPaused()
        {
            //Timer is running and paused
            return paused && started;
        }

        public static float TimeStamp()
        {
            return Time.realtimeSinceStartup;
        }

    }
}