### Built with

- .NET 6 Console App
- DotNet Tool & nupkg

### Prerequisites

- Make sure you are running on the latest .NET 6 SDK (SDK 6.0 and above only).
- Visual Studio 2022 or Rider 2021.3.1

##### Build the nupkg

```shell
dotnet pack
```

then install this nupkg as global tools with the local <b>./nupkg</b> source by using this command line

```shell
dotnet tool install --global --add-source ./nupkg releaseupdate.cli
```

Finally, we can execute this CLI tool anywhere by enter <b>releaseupdate</b> in command line
