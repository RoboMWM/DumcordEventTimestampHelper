using System;
using Microsoft.Maui.Controls;

namespace DumcordEventTimestampHelper
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void SetClipboardButton_Clicked(object sender, EventArgs e)
        {
            // Get the prepared message from the textbox
            string preparedMessage = PreparedMessageEntry.Text;

            // Get the selected time from the time picker
            TimeSpan selectedTime = TimestampTimePicker.Time;

            // Convert the time to the user's local time
            DateTime userLocalTime = ConvertToUserLocalTime(selectedTime);

            //AI wrong answer // Convert the local time to Unix timestamp (seconds since Jan 1, 1970)
            //AI wrong answer //long unixTimestamp = (long)(userLocalTime - new DateTime(1970, 1, 1)).TotalSeconds;
            //I think issue here is that DateTime doesn't include timezone information (it does but not used in most operations),
            //so this operation is acting as if `userLocalTime` is in UTC+0 timezone

            // Convert the DateTime to a DateTimeOffset
            TimeZoneInfo userTimeZone = TimeZoneInfo.Local;
            DateTimeOffset dateTimeOffset = new DateTimeOffset(userLocalTime, userTimeZone.GetUtcOffset(userLocalTime));

            // Convert the DateTimeOffset to a Unix timestamp
            long unixTimestamp = dateTimeOffset.ToUnixTimeSeconds();

            // Format the timestamp as desired (e.g., <t:1716049200:t>)
            string discordTimestamp = $"<t:{unixTimestamp}:t>";

            // Combine the prepared message and Discord timestamp
            string finalMessage = $"{preparedMessage} {discordTimestamp}";

            // Set the message to the clipboard
            Clipboard.SetTextAsync(finalMessage);

            // Optionally, show a confirmation to the user
            DisplayAlert("Clipboard", "Message copied to clipboard!", "OK");
        }

        private DateTime ConvertToUserLocalTime(TimeSpan selectedTime)
        {
            // Get the current date in the user's local timezone
            DateTime currentDate = DateTime.Now;
            TimeZoneInfo userTimeZone = TimeZoneInfo.Local;

            // Combine the current date and selected time
            DateTime userLocalTime = currentDate.Date + selectedTime;

            // Convert to the user's local time
            userLocalTime = TimeZoneInfo.ConvertTime(userLocalTime, userTimeZone);

            return userLocalTime;
        }
    }
}
