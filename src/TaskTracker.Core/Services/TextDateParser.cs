using System.Text.RegularExpressions;

namespace TodoCodexPoc.Services;

public static class TextDateParser
{
    public static DateTimeOffset? Parse(ref string text)
    {
        // Explicit yyyy-mm-dd[ hh:mm]
        var explicitMatch = Regex.Match(text, @"\b(\d{4}-\d{2}-\d{2})(?:\s+(\d{1,2}:\d{2}))?\b");
        if (explicitMatch.Success)
        {
            var date = explicitMatch.Groups[1].Value;
            var time = explicitMatch.Groups[2].Success ? explicitMatch.Groups[2].Value : "00:00";
            if (DateTimeOffset.TryParse($"{date} {time}", out var result))
            {
                text = text.Remove(explicitMatch.Index, explicitMatch.Length).Trim();
                return result;
            }
        }

        // Today or Tomorrow optionally with time and AM/PM
        var relMatch = Regex.Match(text, @"\b(today|tomorrow)(?:\s+(\d{1,2})(?::(\d{2}))?\s*(am|pm)?)?\b", RegexOptions.IgnoreCase);
        if (relMatch.Success)
        {
            var baseDate = DateTimeOffset.Now.Date;
            if (string.Equals(relMatch.Groups[1].Value, "tomorrow", StringComparison.OrdinalIgnoreCase))
            {
                baseDate = baseDate.AddDays(1);
            }

            int hour = 0;
            int minute = 0;
            if (relMatch.Groups[2].Success)
            {
                hour = int.Parse(relMatch.Groups[2].Value);
                if (relMatch.Groups[4].Success)
                {
                    var ampm = relMatch.Groups[4].Value.ToLower();
                    if (ampm == "pm" && hour < 12)
                        hour += 12;
                    if (ampm == "am" && hour == 12)
                        hour = 0;
                }
                if (relMatch.Groups[3].Success)
                {
                    minute = int.Parse(relMatch.Groups[3].Value);
                }
            }

            var result = baseDate.AddHours(hour).AddMinutes(minute);
            text = text.Remove(relMatch.Index, relMatch.Length).Trim();
            return result;
        }

        return null;
    }
}
