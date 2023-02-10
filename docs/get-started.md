# Configure Development Environment
## .NET SDK 7.0.102
You can install .NET SDKS using `winget`:
```
winget install --id Microsoft.DotNet.SDK.7 --version 7.0.102
```

## Installing Worklaods
To install required workloads, run this command in the solution folder:
```
dotnet workload restore
```