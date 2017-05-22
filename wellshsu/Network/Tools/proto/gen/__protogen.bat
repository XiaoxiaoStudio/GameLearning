@echo off

cd ..\
protobuf-net\ProtoGen\protogen.exe ^
-i:hello.txt ^
-i:rank.txt ^
-o:PBMessage\PBMessage.cs -ns:PBMessage
cd gen
