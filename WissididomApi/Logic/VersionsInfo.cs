using System.Diagnostics;
using System.Reflection;

namespace WissididomApi.Logic;

public class VersionsInfo : IVersionsInfo
{
    public string GetAssemblyVersions()
    {
        var assemblyNames = new[] { "WissididomApi" };
        var assemblyInformation = $"{"Assembly",-20} {"Assemblyversion",-20} {"Fileversion",-20} {"Productversion",-20}";
        foreach (var assembly in assemblyNames)
        {
            var assemblyFile = GetAssemblyByName(assembly);
            if (assemblyFile is null) continue;
            var assemblyVersion = assemblyFile.GetName().Version?.ToString() ?? assembly;
            var fileVersion =  FileVersionInfo.GetVersionInfo(assemblyFile.Location).FileVersion;
            var productVersion  = FileVersionInfo.GetVersionInfo(assemblyFile.Location).ProductVersion;
            assemblyInformation += $"\n{assembly,-20} {assemblyVersion,-20} {fileVersion,-20} {productVersion,-20}";
        }
        return assemblyInformation;
    }

    public string GetWissididomApiAssemblyVersion()
    {
        const string assembly = "WissididomApi";
        var assemblyFile = GetAssemblyByName(assembly);
        var assemblyVersion = assemblyFile?.GetName().Version?.ToString() ?? assembly;
        return assemblyVersion;
    }

    private Assembly? GetAssemblyByName(string assemblyName)
    {
        //var t = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName().Name);
        return AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == assemblyName);
    }
}
