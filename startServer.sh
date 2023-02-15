#!/bin/bash

rm -rf Source/.vs &> /dev/null
rm -rf Source/.vscode &> /dev/null
rm -rf Source/Seom.Application/bin &> /dev/null
rm -rf Source/Seom.Application/obj &> /dev/null
rm -rf Source/Seom.Webapp/bin &> /dev/null
rm -rf Source/Seom.Webapp/obj &> /dev/null

dotnet watch run -c Debug --project Source/Seom.Webapp
