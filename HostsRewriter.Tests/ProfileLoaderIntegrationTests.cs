using System;
using System.IO;
using System.Linq;
using HostsRewriter.Domain;
using NUnit.Framework;

namespace HostsRewriter.Tests
{
	[TestFixture]
	public class ProfileLoaderIntegrationTests
	{
		private const string _profileFileName = "profiles.txt";
		private const string _correctFilesFolderName = "Initialization_WithCorrectData_Success";

		[Test]
		public void Initialization_WithCorrectData_Success()
		{
			var path = GetFileFolder(_correctFilesFolderName);
			var loader = new ProfileLoader(path, _profileFileName);
			loader.Init();
			Assert.AreEqual(2, loader.Profiles.Count);
			Assert.AreEqual(3, loader.Profiles.First().Entries.Count);
			Assert.AreEqual(2, loader.Profiles.Last().Entries.Count);
		}

		[Test]
		public void Initialization_WithCorrectDataByAbsolutePath_Success()
		{
			var path = GetFileFolder("Initialization_WithCorrectDataByAbsolutePath_Success");
			File.WriteAllLines(Path.Combine(path, _profileFileName), new []
			{
				Path.Combine(path, "folder1", "a.txt"),
				Path.Combine(path, "folder2", "b.txt")
			});
			var loader = new ProfileLoader(path, _profileFileName);
			loader.Init();
			Assert.AreEqual(2, loader.Profiles.Count);
			Assert.AreEqual(3, loader.Profiles.First().Entries.Count);
			Assert.AreEqual(2, loader.Profiles.Last().Entries.Count);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Initialization_Twice_Exception()
		{
			var path = GetFileFolder(_correctFilesFolderName);
			var loader = new ProfileLoader(path, _profileFileName);
			loader.Init();
			loader.Init();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Initialization_AccessUninitializedData_Exception()
		{
			var path = GetFileFolder(_correctFilesFolderName);
			var loader = new ProfileLoader(path, _profileFileName);
			var a = loader.Profiles;
		}

		[Test]
		public void Initialization_WithWrongPath_Exception()
		{
			Assert.Throws(Is.InstanceOf<IOException>(), () =>
			{
				var path = GetFileFolder("ObviouslyWrongPath");
				var loader = new ProfileLoader(path, _profileFileName);
				loader.Init();
			});
		}

		[Test]
		public void Initialization_WithWrongPathByAbsolutePath_Exception()
		{
			Assert.Throws(Is.InstanceOf<IOException>(), () =>
			{
				var path = GetFileFolder("Initialization_WithCorrectDataByAbsolutePath_Success");
				File.WriteAllLines(Path.Combine(path, _profileFileName), new[]
				{
					Path.Combine(path, "folder1", "ObviouslyWrongPath", "a.txt"),
					Path.Combine(path, "folder2", "ObviouslyWrongPath", "b.txt")
				});
				var loader = new ProfileLoader(path, _profileFileName);
				loader.Init();
				Assert.AreEqual(2, loader.Profiles.Count);
				Assert.AreEqual(3, loader.Profiles.First().Entries.Count);
				Assert.AreEqual(2, loader.Profiles.Last().Entries.Count);
			});
		}

		[Test]
		public void Initialization_WithWrongConfigFile_Exception()
		{
			Assert.Throws(Is.InstanceOf<IOException>(), () =>
			{
				var path = GetFileFolder("Initialization_WithWrongConfigFile_Exceptions");
				var loader = new ProfileLoader(path, _profileFileName);
				loader.Init();
			});
		}

		private string GetFileFolder(string folderName)
		{
			return Path.Combine(Environment.CurrentDirectory, "files", folderName);
		}
	}
}