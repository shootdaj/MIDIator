mkdir NugetPackage
cd NugetPackage
del *.* /Q
cd ..
nuget pack MIDIator.csproj -Prop Configuration=Release -OutputDirectory NugetPackage
nuget push NugetPackage\* -s http://nugetify.azurewebsites.net/ B450BD41-22CA-455D-AD9B-9D746BB1802A
::nuget push NugetPackage\* 00d77a87-1c0c-4710-9a59-c901a70e5af8