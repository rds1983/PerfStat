rem delete existing
rmdir "ZipPackage" /Q /S

rem Create required folders
mkdir "ZipPackage"
mkdir "ZipPackage\MonoGame"
mkdir "ZipPackage\FNA"

set "CONFIGURATION=Release\net45"

rem Copy output files
copy "src\PerfStat\bin\MonoGame\%CONFIGURATION%\PerfStat.dll" "ZipPackage\MonoGame" /Y
copy "src\PerfStat\bin\MonoGame\%CONFIGURATION%\PerfStat.pdb" "ZipPackage\MonoGame" /Y
copy "src\PerfStat\bin\FNA\%CONFIGURATION%\PerfStat.dll" "ZipPackage\FNA" /Y
copy "src\PerfStat\bin\FNA\%CONFIGURATION%\PerfStat.pdb" "ZipPackage\FNA" /Y