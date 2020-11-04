using System;
using Atechnology.Components;
using Atechnology.Components.AtLogWatcher;

namespace FileTracker
{
	public static class ClassLoader
	{
		public static void Load(bool log)
		{
			AtUserControl form = null;
			AtLogWatcher logWatcher = new AtLogWatcher();
			
			try
			{
				
				if (log)
				{
					MdiManager.Add(logWatcher);
					AtLog.LogWatcher = logWatcher;
				}
				
				form = new MainForm();
				MdiManager.Add((AtUserControl)form);

			}
			finally
			{
				if(form != null)
				{
					MdiManager.tabs.Remove(form);
				}
			}
		}
		
	}
}
