using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenBitly
{
    internal class PathHelpers
    {
        private static readonly Regex escapeSegments = new Regex(@"\A(?:[?|&]\w*=)\Z", RegexOptions.Compiled);

         public static string ReplaceUriTemplateTokens(List<object> segments, string path)
         {
             var regexObj = new Regex(@"\{\w*\}", RegexOptions.Compiled);
             var matches = regexObj.Matches(path);
             return (from Match match in matches select match.Value.Substring(1, match.Value.Length - 2)).Aggregate(path, (current, token) => ReplacePathSegment(current, token, segments));
         }

         public static string ReplacePathSegment(string path, string value, List<object> segments)
         {
             var segment = segments.SingleOrDefault(s => s.Equals(string.Format("?{0}=", value)) || s.Equals(string.Format("&{0}=", value)));
             if (segment != null)
             {
                 var index = segments.IndexOf(segment);
                 path = path.Replace(string.Format("{{{0}}}", value), string.Concat(segments[index + 1]));
                 segments.RemoveRange(index, 2);

                 // Replace missing ? if the segment was first in the series (after format)
                 if (index == 1 && segments.Count > 1)
                 {
                     var first = segments[index].ToString();
                     if (first.StartsWith("&"))
                     {
                         segments[index] = string.Concat("?", first.Substring(1));
                     }
                 }
             }
             else
             {
                 path = path.Replace(string.Format("/{{{0}}}", value), "");
             }
             return path;
         }

         public static void EscapeDataContainingUrlSegments(IList<object> segments)
         {
             var names = segments.Where(s => escapeSegments.IsMatch(s.ToString()));
             var indexes = names.Select(n => segments.IndexOf(n) + 1).ToList();
             for (var i = 0; i < indexes.Count(); i++)
             {
                 var segment = segments[indexes[i]];
                 var value = segment.ToString();
                 segments[indexes[i]] = Uri.EscapeDataString(value);
             }
         }
    }
}