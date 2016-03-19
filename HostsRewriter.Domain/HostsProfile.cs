using System;
using System.Collections.Generic;
using System.Linq;

namespace HostsRewriter.Domain
{
	public class HostsProfile
	{
		public string Name { get; }
		public IReadOnlyCollection<HostEntry> Entries { get; }

		public HostsProfile(string name, IReadOnlyCollection<HostEntry> entries)
		{
			Name = name;
			Entries = entries;
		}

		public string ApplyToText(string text)
		{
			var initialLines = SplitToLines(text);
			var resultLines = new List<string>(initialLines.Count + Entries.Count);
			var trackedEntries = Entries.Select(e => new TrackerHostEntry(e)).ToList();
			bool lineAdded;
			foreach (var line in initialLines)
			{
				HostEntry? he = HostEntry.FromString(line);
				lineAdded = false;
				if (he.HasValue)
				{
					var entry = trackedEntries.FirstOrDefault(e => e.Entry.Url.Equals(he.Value.Url, StringComparison.InvariantCultureIgnoreCase));
					if (entry != null)
					{
						entry.Used = true;
						if (entry.Entry.Ip != null)
						{
							resultLines.Add(entry.Entry.ToString());
						}
						lineAdded = true;
					}
				}
				if (!lineAdded)
				{
					resultLines.Add(line);
				}
			}
			foreach (var e in trackedEntries.Where(e => !e.Used))
			{
				resultLines.Add(e.Entry.ToString());
			}
			return String.Join(Environment.NewLine, resultLines);
		}

		public static HostsProfile FromText(string name, string text)
		{
			var lines = SplitToLines(text);
			var entries = lines.Select(l => HostEntry.FromString(l)).Where(he => he != null).Cast<HostEntry>().ToList();
			return new HostsProfile(name, entries);
		}

		private static List<string> SplitToLines(string text)
		{
			return text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
		}

		private class TrackerHostEntry
		{
			public HostEntry Entry { get; }
			public bool Used { get; set; }

			public TrackerHostEntry(HostEntry entry)
			{
				Entry = entry;
				Used = false;
			}
		}
	}
}