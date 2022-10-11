@echo off
title OpenRA - ML
cd %~dp0%
bin\OpenRA.exe Engine.EngineDir=".." Engine.LaunchPath="%~dpf0" Game.Mod="ai" AI.Bots="rush,rush" AI.Seed="1337"

pause