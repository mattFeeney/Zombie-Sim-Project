using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;


namespace Ini
{
    /// <summary>

    /// Create a New INI file to store or load data

    /// </summary>

    public class IniFile
    {
        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
		
        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW",
    	SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = 
    	CallingConvention.StdCall)]
		private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault,
        string lpReturnString, int nSize, string lpFilename);

		

        public void IniWriteValue(string Section, string Key, string Value, string filename)
        {
            WritePrivateProfileString(Section, Key, Value, filename);
        }
		

        public string IniReadValue(string Section, string Key, string filename)
        {
            String temp = new String(' ',255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, filename);
            return temp;

        }
		
		public List<string> GetKeys(string Section, string filename)
		{
		    String returnString = new String(' ', 32000);
		    GetPrivateProfileString(Section, null, null, returnString, 32000, filename);
		    List<string> result = new List<string>(returnString.ToString().Split('\0'));
		    result.RemoveRange(result.Count-2,2);
		    return result;
		}
    }
}