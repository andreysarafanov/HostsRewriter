using System;
using System.Linq;
using System.Net;

namespace HostsRewriter.Domain
{
	public struct HostEntry
	{
		public IPAddress Ip { get; }
		public string Url { get; }

		public HostEntry(IPAddress ip, string url)
		{
			Ip = ip;
			Url = url;
		}

		public override string ToString() => $"{Ip.ToString()} {Url}";

		public static HostEntry? FromString(string s)
		{
			var parts = s.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).ToList();
			if (parts.Count == 2)
			{
				IPAddress ip;
				if (parts[0].Equals("nothing"))
				{
					return new HostEntry(null, parts[1]);
				}
				else if (IPAddress.TryParse(parts[0], out ip))
				{
					return new HostEntry(ip, parts[1]);
				}
			}
			return null;
		}
	}
}
