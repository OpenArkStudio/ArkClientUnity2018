echo "generate protobuff files"

for /f "delims=" %%i in ('dir /b/a-d/oN *.proto') do "proto-gen/protoc.exe" -I=./ --csharp_out=./ ./%%~ni.proto

pause