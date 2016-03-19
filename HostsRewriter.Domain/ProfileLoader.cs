using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HostsRewriter.Domain
{
	public class ProfileLoader
	{
		private string _configFileFolder;
		private string _configFileName;
		private bool _isInitialized = false;
		private IReadOnlyCollection<HostsProfile> _profiles;

		public IReadOnlyCollection<HostsProfile> Profiles
		{
			get
			{
				if (!_isInitialized)
				{
					throw new InvalidOperationException("Cannot access profiles before the loader has been initialized");
				}
				return _profiles;
			}
			private set { _profiles = value; }
		}

		public ProfileLoader(string configFileFolder, string configFileName)
		{
			_configFileFolder = configFileFolder;
			_configFileName = configFileName;
		}

		public void Init()
		{
			if (_isInitialized)
			{
				throw new InvalidOperationException("Cannot initialize a loader twice");
			}
			var configFilePath = Path.Combine(_configFileFolder, _configFileName);
			var lines = File.ReadAllLines(configFilePath).ToList();
			var collections = new List<HostsProfile>(lines.Count);
			foreach (var line in lines)
			{
				var path = Path.IsPathRooted(line)
					? line
					: Path.Combine(_configFileFolder, line);
				collections.Add(HostsProfile.FromText(Path.GetFileNameWithoutExtension(path), File.ReadAllText(path)));
			}
			Profiles = collections;
			_isInitialized = true;
		}
	}
}