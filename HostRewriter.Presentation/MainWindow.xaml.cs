using HostsRewriter.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using System.Windows.Controls;
using System.IO;

namespace HostRewriter.Presentation
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ProfileLoader _loader;
		public MainWindow()
		{
			Width = 0;
			Height = 0;
			WindowStyle = WindowStyle.None;
			ShowInTaskbar = false;
			ShowActivated = false;
			Visibility = Visibility.Hidden;

			InitializeComponent();
			_loader = new ProfileLoader(
				ConfigurationManager.AppSettings["configFileFolder"],
				ConfigurationManager.AppSettings["configFileName"]);
			_loader.Init();

			foreach (var hostsProfile in _loader.Profiles)
			{
				var item = new MenuItem {Header = hostsProfile.Name};
				item.Click += (s, e) => OnItemClick(hostsProfile);
				ContextMenu.Items.Add(item);
			}
			var closeItem = new MenuItem { Header = "Close" };
			closeItem.Click += (s, e) => Close();
			ContextMenu.Items.Add(closeItem);
		}

		private void OnItemClick(HostsProfile profile)
		{
			var path = ConfigurationManager.AppSettings["hostsFilePath"];
			var text = File.ReadAllText(path);
			text = profile.ApplyToText(text);
			File.WriteAllText(path, text);
		}
	}
}
