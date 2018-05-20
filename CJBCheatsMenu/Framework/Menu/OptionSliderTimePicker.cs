using System;

namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// Convenience class for creating a slider used to pick a time.
    /// </summary>
    public class OptionSliderTimePicker : OptionSlider
    {
        /// <summary>
        /// Number of hours in a stardew valley day (6AM -> 2AM)
        /// </summary>
        private const int HOURS_IN_DAY = 20;

        /// <summary>
        /// Number of minutes that pass per time tick.
        /// </summary>
        private const int MINUTES_PER_TIME_CHANGE = 10;

        /// <summary>
        /// Number of ticks per hour.
        /// </summary>
        private const int TIME_CHANGES_PER_HOUR = 60 / MINUTES_PER_TIME_CHANGE;

        /// <summary>
        /// The minimum time the time picker can choose (6am).
        /// </summary>
        private const int MINIMUM_START_TIME = 600;

        /// <summary>
        /// The maximum time the time picker can choose (2am).
        /// </summary>
        private const int MAXIMUM_END_TIME = MINIMUM_START_TIME + HOURS_IN_DAY * 100;

        /// <summary>
        /// Converts the selected slider value to a stardew valley integer time.
        /// </summary>
        /// <param name="sliderValue">Value indicating the number of ticks since 6am.</param>
        /// <returns>Number where the hundreds digit represents the hour, and the tens/ones digit represents the minutes.</returns>
        private static int ConvertSliderValueToTime(int sliderValue)
        {
            int minutes = (sliderValue % TIME_CHANGES_PER_HOUR) * MINUTES_PER_TIME_CHANGE;
            int hours = sliderValue / TIME_CHANGES_PER_HOUR;
            return hours * 100 + minutes + MINIMUM_START_TIME;
        }

        /// <summary>
        /// Converts stardew valley integer time to a slider value.
        /// </summary>
        /// <param name="time">Number where the hundreds digit represents the hour, and the tens/ones digit represents the minutes.</param>
        /// <returns>Value indicating the number of ticks since 6am.</returns>
        private static int ConvertTimeToSliderValue(int time)
        {
            int hours = (time - MINIMUM_START_TIME) / 100;
            int minutes = time % 100;
            return hours * TIME_CHANGES_PER_HOUR + minutes / MINUTES_PER_TIME_CHANGE;
        }

        /// <summary>
        /// Constructor for a time picker slider option.
        /// </summary>
        /// <param name="label">The label displayed for this option.</param>
        /// <param name="initialTimeSelected">Initial time the slider has selected (e.g. 1320 is 1:20pm)</param>
        /// <param name="timeSelectedChangedCallback">Action called whenever the selected time changes (when mouse drag released).</param>
        /// <param name="minTime">The minimum time that the slider can select.</param>
        /// <param name="maxTime">The maximum time that the slider can select.</param>
        /// <param name="stepInMinutes">The number of minutes the selected time increases with each step of the slider.</param>
        public OptionSliderTimePicker(string label, int initialTimeSelected, Action<int> timeSelectedChangedCallback, int minTime = MINIMUM_START_TIME, int maxTime = MAXIMUM_END_TIME, int stepInMinutes = MINUTES_PER_TIME_CHANGE)
            : base(label, ConvertTimeToSliderValue(initialTimeSelected), newValue => timeSelectedChangedCallback(ConvertSliderValueToTime(newValue)))
        {
            if (minTime < MINIMUM_START_TIME)
            {
                throw new ArgumentException(String.Format("Minimum time cannot be less than {0} got {1}", MINIMUM_START_TIME, minTime));
            } else if (maxTime > MAXIMUM_END_TIME)
            {
                throw new ArgumentException(String.Format("Maximum time cannot be greater than {0} got {1}", MAXIMUM_END_TIME, maxTime));
            } else if (minTime >= maxTime)
            {
                throw new ArgumentException(String.Format("Minimum time must be less than Maximum time, got {0}, {1}", minTime, maxTime));
            } else if (stepInMinutes % MINUTES_PER_TIME_CHANGE != 0)
            {
                throw new ArgumentException(String.Format("Step must be divisible by {0}. Which is the smallest about of time that passes, got {1}", MINUTES_PER_TIME_CHANGE, stepInMinutes));
            } else if (stepInMinutes <= 0)
            {
                throw new ArgumentException(String.Format("Step must be positive, got {0}", stepInMinutes));
            }

            this.MinValue = ConvertTimeToSliderValue(minTime);
            this.MaxValue = ConvertTimeToSliderValue(maxTime);
            this.Step = stepInMinutes / MINUTES_PER_TIME_CHANGE;
        }

        /// <summary>
        /// The current time that is selected by the slider.
        /// </summary>
        public int TimeSelected
        {
            get => ConvertSliderValueToTime(this.Value);
            set => this.Value = ConvertTimeToSliderValue(value);
        }

        /// <summary>
        /// Converts the slider value to a human readable time string.
        /// </summary>
        /// <param name="value">A valid value the slider can be set to.</param>
        /// <returns>A human readable time string based on the value passed in.</returns>
        public override string ConvertValueToString(int value)
        {
            return StardewValley.Game1.getTimeOfDayString(ConvertSliderValueToTime(value));
        }
    }
}
