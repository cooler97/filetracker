/*
 * Created by SharpDevelop.
 * User: Ilya
 * Date: 21.04.2020
 * Time: 23:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Atechnology.ecad;

namespace FileTracker
{
	/// <summary>
	/// Description of SettingVarWrapper.
	/// </summary>
	public class SettingVarWrapper
	{
		SettingVar _var;
		
		public SettingVarWrapper(SettingVar var)
		{
			this._var = var;
		}
		
		public string Name {
			
			get { return _var.name; }
			
		}
		
		public SettingVar Value {
			
			get { return _var; }
			
		}
		
		public override string ToString()
		{
			return _var.name;
		}

	}
}
