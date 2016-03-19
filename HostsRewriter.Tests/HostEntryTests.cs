using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HostsRewriter.Domain;
using NUnit.Framework;

namespace HostsRewriter.Tests
{
	[TestFixture]
	public class HostEntryTests
	{
		[Test]
		public void Parsing_CorrectHostEntry_IsSuccessful()
		{
			var he = HostEntry.FromString("127.0.0.1  google.com	");
			Assert.IsTrue(he.HasValue);
			Assert.AreEqual(IPAddress.Parse("127.0.0.1"), he.Value.Ip);
			Assert.AreEqual("google.com", he.Value.Url);
		}

		[Test]
		public void Parsing_NoUrl_IsSuccessful()
		{
			var he = HostEntry.FromString("nothing  google.com	");
			Assert.IsTrue(he.HasValue);
			Assert.AreEqual(null, he.Value.Ip);
			Assert.AreEqual("google.com", he.Value.Url);
		}

		[TestCase("127.0.0.256 google.com")]
		[TestCase("127.0.0.1 google.com google.ru")]
		[TestCase("#127.0.0.1 google.com")]
		[TestCase("# Just some commented text")]
		public void Parsing_IncorrectHostEntry_IsUnsuccessful(string s)
		{
			var he = HostEntry.FromString(s);
			Assert.IsFalse(he.HasValue);
		}

		[Test]
		public void ToString_IsCorrect()
		{
			var str = "127.0.0.1 google.com";
			var he = HostEntry.FromString(str);
			Assert.AreEqual(str, he.ToString());
		}
	}
}
