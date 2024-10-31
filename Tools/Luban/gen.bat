
set GEN_CLIENT=Luban\Luban.dll
set CONF_ROOT=DataTables

dotnet %GEN_CLIENT% ^
    -t all ^
    -c cs-simple-json ^
    -d json  ^
    --conf luban.conf ^
    -x outputCodeDir=..\..\Assets\Src\Game\Config\Generator ^
    -x outputDataDir=..\..\Assets\Res\Config\Table 

pause