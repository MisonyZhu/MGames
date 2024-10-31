#!/bin/bash

WORKSPACE=..\..
LUBAN_DLL=$WORKSPACE/Tools/Luban/Luban.dll
CONF_ROOT==$WORKSPACE/DataTables

dotnet $LUBAN_DLL \
    -t all \
	-c cs-simple-json \
    -d json \
    --conf luban.conf \
	-x outputCodeDir=outputcode \
    -x outputDataDir=output