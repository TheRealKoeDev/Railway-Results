using Microsoft.VisualStudio.TestTools.UnitTesting;
using Railway.Test;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Railway-Test")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Railway-Test")]
[assembly: AssemblyCopyright("Copyright ©  2019")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: Guid("7edee823-2ac1-4351-bdbc-a39d679d9072")]

// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: Parallelize(Workers = Settings.WorkerCount, Scope = Settings.Scope)]
