using System.Linq;
using HostsRewriter.Domain;
using NUnit.Framework;

namespace HostsRewriter.Tests
{
	[TestFixture]
	public class HostsProfileTests
	{
		private const string _sampleText = 
@"127.0.0.1  google.com
		#128.0.0.1 yandex.ru
				#some random text
				  192.168.0.100 localresource.me
			";

		[Test]
		public void Parsing_IsSuccessful()
		{
			var name = "great_name";

			var entries = HostsProfile.FromText(name, _sampleText);
			Assert.AreEqual(name, entries.Name);
			Assert.AreEqual(2, entries.Entries.Count);
			Assert.AreEqual("127.0.0.1 google.com", entries.Entries.First().ToString());
			Assert.AreEqual("192.168.0.100 localresource.me", entries.Entries.Last().ToString());
		}

		[Test]
		public void ApplyToText_IsCorrect()
		{
			var entries = new HostsProfile("name", new[]
			{
				HostEntry.FromString("127.1.1.1 google.com"),
				HostEntry.FromString("nothing localresource.me"),
				HostEntry.FromString("127.1.1.3 newurl.ru"),
			}.Cast<HostEntry>().ToList());

			var expectedResult =
				@"127.1.1.1 google.com
		#128.0.0.1 yandex.ru
				#some random text
			
127.1.1.3 newurl.ru";

			Assert.AreEqual(expectedResult, entries.ApplyToText(_sampleText));

		}
	}
}