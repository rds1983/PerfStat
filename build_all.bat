dotnet --version
dotnet build build\Monogame\PerfStat.sln /p:Configuration=Release --no-incremental
dotnet build build\FNA\PerfStat.sln /p:Configuration=Release --no-incremental
call copy_zip_package_files.bat
rename "ZipPackage" "PerfStat.%APPVEYOR_BUILD_VERSION%"
7z a PerfStat.%APPVEYOR_BUILD_VERSION%.zip PerfStat.%APPVEYOR_BUILD_VERSION%
